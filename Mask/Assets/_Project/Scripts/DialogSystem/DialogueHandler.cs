using AudioSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UtilitySingletons;

public class DialogueHandler : Singleton<DialogueHandler> {

    [Header("References")]
    [SerializeField] private DialogueCanvas _iDialogueCanvas;
    [SerializeField] private StoryStateSO _iStoryState;
    [SerializeField] private DialogueAnimationHandler _iSpeakingAnimationHandler;

    [Header("Typing Settings")]
    [SerializeField] private bool _iCanSkip = true;

    [Header("Silent Preset")]
    [SerializeField] private string _silentPresetName = "na";

    private CancellationTokenSource _typingCancellation;
    private int _currentDialogIndex = 0;
    private bool _isTyping = false;
    private bool _skipRequested = false;
    private bool _isPaused = false;
    private bool _isPlayerInput = false;

    private CharacterDialogSO _currentDialog;
    private DialogSoundSO _currentDialogSound;
    private DialogueAnimationHandler _currentNPCAnimator;

    private DialogueAnimationParser.AnimationTag _currentAnimation = null;
    private string _currentActivePreset = null; // Track current preset name

    private InteractableTalk _interactableSpeaker;

    private void Start() {
        if (InputManager.Instance != null) {
            InputManager.Instance._DialogueContinueAction.started += OnSkipInput;
        }

        _iStoryState.LoadRegisteredStates();
    }

    private void OnDestroy() {
        if (InputManager.Instance != null) {
            InputManager.Instance._DialogueContinueAction.started -= OnSkipInput;
        }

        _typingCancellation?.Cancel();
        _typingCancellation?.Dispose();
    }

    private void OnSkipInput(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        if (_iCanSkip && _isTyping && !_isPaused) {
            _skipRequested = true;
        } else if (!_isPaused) {
            _isPlayerInput = true;
        }
    }

    public async void StartDialogueTree(InteractableTalk i_speaker, CharacterDialogSO i_dialogueTree, string i_name, DialogSoundSO i_dialogSound, DialogueAnimationHandler i_npcAnimator = null) {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InputManager.Instance.SetDialogueActionMap();

        _iDialogueCanvas.gameObject.SetActive(true);
        _iDialogueCanvas.SetName(i_name);

        _currentDialog = i_dialogueTree;
        _currentDialogSound = i_dialogSound;
        _currentNPCAnimator = i_npcAnimator;
        _currentActivePreset = null; // Reset preset tracking

        _interactableSpeaker = i_speaker;

        await i_speaker.RotateTowardsAsync(gameObject.transform);

        _currentNPCAnimator.SetOriginTransforms();

        ContinueDialogue();
    }

    public async void ContinueDialogue() {

        _currentDialogIndex = 0;

        while (_currentDialogIndex < _currentDialog.DialogText.Count) {
            await SetDialogue(_currentDialog.DialogText[_currentDialogIndex]);
            _currentDialogIndex++;

            await PlayerInput();
        }

        // player choice
        for (int id = 0; id < _currentDialog.decisionOptions.Count; id++) {
            _iDialogueCanvas.AddResponse(_currentDialog.decisionOptions[id].text, id);
        }
    }

    public async Task PlayerInput() {
        while (!_isPlayerInput) {
            await Awaitable.NextFrameAsync();
        }
        _isPlayerInput = false;
    }

    private void FinishDialogue() {
        _interactableSpeaker.RotateBackToStartAsync();

        InputManager.Instance.SetPlayerActionMap();
        _iDialogueCanvas.gameObject.SetActive(false);

        _currentNPCAnimator = null;
        _currentAnimation = null;
        _currentActivePreset = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SelectResponse(int i_id) {
        _iDialogueCanvas.ClearResponses();

        PlayerDecision chosenDecision = _currentDialog.decisionOptions[i_id];

        if (chosenDecision.affectsState) {
            _iStoryState.SetValue(chosenDecision.stateVariable, chosenDecision.stateValue);
        }

        _currentDialog = chosenDecision.nextDialog;
        if (_currentDialog == null) { FinishDialogue(); return; }

        ContinueDialogue();
    }

    public async Task SetDialogue(string i_dialogue) {
        _typingCancellation?.Cancel();
        _typingCancellation?.Dispose();
        _typingCancellation = new CancellationTokenSource();

        _iDialogueCanvas.SetDialogue("");
        _skipRequested = false;
        _isTyping = true;

        try {
            await TypeTextWithRichText(i_dialogue, _typingCancellation.Token);
        } catch (OperationCanceledException) {
            // Parse to get clean text for instant display
            var (cleanText, _) = DialogueAnimationParser.Parse(i_dialogue);
            _iDialogueCanvas.SetDialogue(cleanText);
        } finally {
            _isTyping = false;
        }
    }

    private async Task TypeTextWithRichText(string i_text, CancellationToken i_cancellationToken) {

        // Start NPC speaking animation with default preset at the beginning
        if (_currentNPCAnimator != null) {
            _currentNPCAnimator.StartSpeaking();
            _currentActivePreset = null; // Default preset (no name)
        }

        // Parse animation tags
        var (cleanText, animations) = DialogueAnimationParser.Parse(i_text);

        StringBuilder displayedText = new StringBuilder();
        bool insideTag = false;
        int cleanTextIndex = 0; // Track position in clean text (for animation lookup)

        for (int i = 0; i < cleanText.Length; i++) {

            // Wait while paused
            while (_isPaused) {
                i_cancellationToken.ThrowIfCancellationRequested();
                await Awaitable.NextFrameAsync(i_cancellationToken);
            }

            if (_skipRequested) {
                _iDialogueCanvas.SetDialogue(cleanText);
                _skipRequested = false;
                break; // Exit loop but still stop animation below
            }

            i_cancellationToken.ThrowIfCancellationRequested();

            char currentChar = cleanText[i];

            // Check if we're entering or exiting a rich text tag (like <color>)
            if (currentChar == '<') {
                insideTag = true;
            }

            displayedText.Append(currentChar);
            _iDialogueCanvas.SetDialogue(displayedText.ToString());

            if (currentChar == '>') {
                insideTag = false;
                continue;
            }

            // Only delay and play sound for visible characters
            if (!insideTag) {
                // Check if entering a new animation range
                if (DialogueAnimationParser.IsEnteringAnimationRange(cleanTextIndex, animations, out var enteringAnim)) {
                    _currentAnimation = enteringAnim;
                    HandleAnimationEnter(enteringAnim);
                }

                // Check if exiting an animation range
                if (DialogueAnimationParser.IsExitingAnimationRange(cleanTextIndex, animations, out var exitingAnim)) {
                    HandleAnimationExit(exitingAnim);
                    _currentAnimation = null;
                }

                if (char.IsLetterOrDigit(currentChar)) {
                    // Only play typing sound if not in silent preset
                    if (!IsCurrentPreset(_silentPresetName)) {
                        PlayTypingSound();
                    }
                }

                if (!char.IsPunctuation(currentChar)) {
                    await Awaitable.WaitForSecondsAsync(_currentDialogSound._iTypingSpeed, i_cancellationToken);
                } else {
                    await Awaitable.WaitForSecondsAsync(_currentDialogSound._iPunctuationPauseLength, i_cancellationToken);
                }

                cleanTextIndex++;
            }
        }

        // Stop NPC speaking animation when text is done displaying
        if (_currentNPCAnimator != null) {
            _currentNPCAnimator.StopSpeaking();
        }

        // Reset animation and preset tracking when done
        _currentAnimation = null;
        _currentActivePreset = null;
    }

    private void HandleAnimationEnter(DialogueAnimationParser.AnimationTag i_animationTag) {
        if (_currentNPCAnimator == null) return;

        if (i_animationTag._CommandType == DialogueAnimationParser.AnimationCommandType.Preset) {
            // Change to the new emotion preset (stops and restarts with new preset)
            _currentNPCAnimator.SetEmotionPreset(i_animationTag._AnimationType);
            _currentActivePreset = i_animationTag._AnimationType;
        } else {
            // Trigger one-time animation (doesn't stop base animation, layers on top)
            TriggerAnimation(i_animationTag._AnimationType);
        }
    }

    private void HandleAnimationExit(DialogueAnimationParser.AnimationTag i_animationTag) {
        if (_currentNPCAnimator == null) return;

        // If exiting a preset, return to default preset
        if (i_animationTag._CommandType == DialogueAnimationParser.AnimationCommandType.Preset) {
            ReturnToBasePreset();
        }
    }

    private void ReturnToBasePreset() {
        if (_currentNPCAnimator == null) return;
        // Return to default preset (restarts animation with default)
        _currentNPCAnimator.SetEmotionPreset();
        _currentActivePreset = null; // Back to default (no name)
    }

    /// <summary>
    /// Check if a specific preset is currently active
    /// </summary>
    public bool IsCurrentPreset(string i_presetName) {
        if (string.IsNullOrEmpty(i_presetName)) {
            // Check if we're using default preset (no name)
            return string.IsNullOrEmpty(_currentActivePreset);
        }

        return _currentActivePreset != null &&
               _currentActivePreset.Equals(i_presetName, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Get the name of the currently active preset (null if default)
    /// </summary>
    public string GetCurrentPresetName() {
        return _currentActivePreset;
    }

    private void TriggerAnimation(string i_animationType) {
        if (_currentNPCAnimator == null) return;

        // Trigger specific animations based on type
        switch (i_animationType.ToLower()) {
            case "yes":
            case "nod":
                _currentNPCAnimator.TriggerNod();
                break;

            case "no":
            case "deny":
                _currentNPCAnimator.TriggerHeadShake();
                break;

            case "emphasis":
            case "important":
                _currentNPCAnimator.TriggerEmphasis();
                break;

            case "shake":
                _currentNPCAnimator.TriggerShake(0.1f, 0.3f);
                break;

            default:
                Debug.LogWarning($"Unknown trigger animation type: {i_animationType}");
                break;
        }
    }

    private void PlayTypingSound() {
        SoundManager.Instance.CreateSound()
            .WithRandomPitch()
            .Play(_currentDialogSound._iTypingSoundData);
    }

    public void Pause() {
        _isPaused = true;
    }

    public void Resume() {
        _isPaused = false;
    }

    #region Public Properties
    public bool IsTyping => _isTyping;
    public bool IsPaused => _isPaused;
    #endregion
}