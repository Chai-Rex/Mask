using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UtilitySingletons;

public class DialogueHandler : Singleton<DialogueHandler> {

    [Header("References")]
    [SerializeField] private DialogueCanvas _iDialogueCanvas;
    [SerializeField] private StoryStateSO _iStoryState;

    [Header("Typing Settings")]
    [SerializeField] private float _iTypingSpeed = 0.05f;
    [SerializeField] private bool _iCanSkip = true;

    [Header("Audio (Testing)")]
    [SerializeField] private AudioSource _iTypingAudioSource;
    [SerializeField] private AudioClip[] _iTypingSounds;
    [SerializeField] private float _iPunctuationPauseLength;
    [SerializeField] private float _iMinPitchModulation = 1;
    [SerializeField] private float _iMaxPitchModulation = 1;

    private CancellationTokenSource _typingCancellation;
    private int _currentDialogIndex = 0;
    private bool _isTyping = false;
    private bool _skipRequested = false;
    private bool _isPaused = false;
    private bool _isPlayerInput = false;

    private CharacterDialogSO currentDialog;

    private void Start() {
        if (InputManager.Instance != null) {
            InputManager.Instance._DialogueContinueAction.started += OnSkipInput;
        }
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

    public void StartDialogueTree(CharacterDialogSO i_dialogueTree, string i_name) {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InputManager.Instance.SetDialogueActionMap();

        _iDialogueCanvas.gameObject.SetActive(true);
        _iDialogueCanvas.SetName(i_name);

        currentDialog = i_dialogueTree;

        ContinueDialogue();
    }

    public async void ContinueDialogue() {

        _currentDialogIndex = 0;

        while (_currentDialogIndex < currentDialog.DialogText.Count) {
            await SetDialogue(currentDialog.DialogText[_currentDialogIndex]);
            _currentDialogIndex++;

            await PlayerInput();
        }

        // player choice
        for (int id = 0; id < currentDialog.decisionOptions.Count; id++) {
            _iDialogueCanvas.AddResponse(currentDialog.decisionOptions[id].text, id);
        }
    }

    public async Task PlayerInput() {
        while (!_isPlayerInput) {
            await Awaitable.NextFrameAsync();
        }
        _isPlayerInput = false;
    }

    private void FinishDialogue() {
        InputManager.Instance.SetPlayerActionMap();
        _iDialogueCanvas.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SelectResponse(int i_id) {
        _iDialogueCanvas.ClearResponses();

        PlayerDecision chosenDecision = currentDialog.decisionOptions[i_id];

        if (chosenDecision.affectsState) {
            _iStoryState.SetValue(chosenDecision.stateVariable, chosenDecision.stateValue);
        }

        currentDialog = chosenDecision.nextDialog;
        if (currentDialog == null) { FinishDialogue(); return; }

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
            _iDialogueCanvas.SetDialogue(i_dialogue);
        } finally {
            _isTyping = false;
        }
    }

    private async Task TypeTextWithRichText(string i_text, CancellationToken i_cancellationToken) {
        StringBuilder displayedText = new StringBuilder();
        bool insideTag = false;

        for (int i = 0; i < i_text.Length; i++) {

            // Wait while paused
            while (_isPaused) {
                i_cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            if (_skipRequested) {
                _iDialogueCanvas.SetDialogue(i_text);
                _skipRequested = false;
                return;
            }

            i_cancellationToken.ThrowIfCancellationRequested();

            char currentChar = i_text[i];

            // Check if we're entering or exiting a tag
            if (currentChar == '<') {
                insideTag = true;
            }

            displayedText.Append(currentChar);
            _iDialogueCanvas.SetDialogue(displayedText.ToString());

            if (currentChar == '>') {
                insideTag = false;
                continue; // Don't delay or play sound for closing bracket
            }

            // Only delay and play sound for visible characters
            if (!insideTag) {
                if (char.IsLetterOrDigit(currentChar))
                {
                    PlayTypingSound();
                }
                
                if(!char.IsPunctuation(currentChar))
                {
                    await Task.Delay(TimeSpan.FromSeconds(_iTypingSpeed), i_cancellationToken);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(_iPunctuationPauseLength), i_cancellationToken);
                }
            }
        }
    }

    private void PlayTypingSound() {
        if (_iTypingAudioSource != null && _iTypingSounds != null && _iTypingSounds.Length > 0) {
            AudioClip clip = _iTypingSounds[UnityEngine.Random.Range(0, _iTypingSounds.Length)];
            _iTypingAudioSource.pitch = UnityEngine.Random.Range(_iMinPitchModulation, _iMaxPitchModulation);
            _iTypingAudioSource.PlayOneShot(clip);
        }
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
    public float TypingSpeed {
        get => _iTypingSpeed;
        set => _iTypingSpeed = Mathf.Max(0.01f, value);
    }
    #endregion

}