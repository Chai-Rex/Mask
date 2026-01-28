using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilitySingletons;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : PersistentSingleton<InputManager> {

    public event UnityAction<string> _OnControlsChanged;

    // Player
    // subscribe to these
    public InputAction _PlayerMoveAction { get; private set; }
    public InputAction _PlayerLookAction { get; private set; }
    public InputAction _PlayerInteractAction { get; private set; }
    public InputAction _PlayerCrouchAction { get; private set; }
    public InputAction _PlayerJumpAction { get; private set; }
    public InputAction _PlayerSprintAction { get; private set; }
    public InputAction _PlayerPauseAction { get; private set; }
    public InputAction _PlayerDropAction { get; private set; }

    // UI
    public InputAction _UINavigateAction { get; private set; }
    public InputAction _UIUnpauseAction { get; private set; }
    public InputAction _UISubmitAction { get; private set; }

    // Dialogue
    public InputAction _DialogueNavigateAction { get; private set; }
    public InputAction _DialogueContinueAction { get; private set; }
    public InputAction _DialoguePauseAction { get; private set; }

    // Death
    public InputAction _DeathRespawnAction { get; private set; }

    ///////////////////////

    [SerializeField] private PlayerInput _iPlayerInput;
    private InputActionAsset _inputActionsAsset;
    private Gamepad _gamepad;

    protected override void Awake() {
        base.Awake();

        _iPlayerInput = GetComponent<PlayerInput>();

        _iPlayerInput.onControlsChanged += PlayerInput_onControlsChanged;

        _inputActionsAsset = _iPlayerInput.actions;

        GetInputActions();
    }

    private void OnDestroy() {
        _iPlayerInput.onControlsChanged -= PlayerInput_onControlsChanged;
    }

    private void PlayerInput_onControlsChanged(PlayerInput obj) {
        if (_iPlayerInput.currentControlScheme == "Gamepad") _gamepad = Gamepad.current;

        _OnControlsChanged?.Invoke(_iPlayerInput.currentControlScheme);
    }

    private void GetInputActions() {
        _PlayerMoveAction = BuildAction("Move");
        _PlayerLookAction = BuildAction("Look");
        _PlayerInteractAction = BuildAction("Interact");
        _PlayerCrouchAction = BuildAction("Crouch");
        _PlayerJumpAction = BuildAction("Jump");
        _PlayerSprintAction = BuildAction("Sprint");
        _PlayerPauseAction = BuildAction("PlayerPause");
        _PlayerDropAction = BuildAction("Drop");

        _UINavigateAction = BuildAction("UINavigation");
        _UIUnpauseAction = BuildAction("Unpause");
        _UISubmitAction = BuildAction("Submit");

        _DialogueNavigateAction = BuildAction("DialogueNavigation");
        _DialogueContinueAction = BuildAction("Continue");
        _DialoguePauseAction = BuildAction("DialoguePause");

        _DeathRespawnAction = BuildAction("Respawn");
    }

    private InputAction BuildAction(string i_name) {
        InputAction action = _inputActionsAsset?.FindAction(i_name, throwIfNotFound: false);
        if (action != null)
            action.Enable();
        else
            Debug.LogWarning($"Action '{i_name}' not found!");
        return action;
    }

    public void SetPlayerActionMap() {
        EnablePlayerActions();
        DisableUIActions();
        DisableDialogueActions();
        DisableDeathActions();
    }

    public void SetUIActionMap() {
        DisablePlayerActions();
        EnableUIActions();
        DisableDialogueActions();
        DisableDeathActions();
    }

    public void SetDialogueActionMap() {
        DisablePlayerActions();
        DisableUIActions();
        EnableDialogueActions();
        DisableDeathActions();

    }

    public void SetDeathActionMap() {
        DisablePlayerActions();
        DisableUIActions();
        DisableDialogueActions();
        EnableDeathActions();
    }

    public void EnablePlayerActions() {
        _PlayerMoveAction.Enable();
        _PlayerLookAction.Enable();
        _PlayerInteractAction.Enable();
        _PlayerCrouchAction.Enable();
        _PlayerJumpAction.Enable();
        _PlayerSprintAction.Enable();
        _PlayerPauseAction.Enable();
        _PlayerDropAction.Enable();
    }

    public void DisablePlayerActions() {
        _PlayerMoveAction.Disable();
        _PlayerLookAction.Disable();
        _PlayerInteractAction.Disable();
        _PlayerCrouchAction.Disable();
        _PlayerJumpAction.Disable();
        _PlayerSprintAction.Disable();
        _PlayerPauseAction.Disable();
        _PlayerDropAction.Disable();
    }

    public void EnableUIActions() {
        _UINavigateAction.Enable();
        _UIUnpauseAction.Enable();
        _UISubmitAction.Enable();
    }

    public void DisableUIActions() {
        _UINavigateAction.Disable();
        _UIUnpauseAction.Disable();
        _UISubmitAction.Disable();
    }

    public void EnableDialogueActions() {
        _DialogueNavigateAction.Enable();
        _DialogueContinueAction.Enable();
        _DialoguePauseAction.Enable();
    }

    public void DisableDialogueActions() {
        _DialogueNavigateAction.Disable();
        _DialogueContinueAction.Disable();
        _DialoguePauseAction.Disable();
    }

    public void EnableDeathActions() {
        _DeathRespawnAction.Enable();
    }

    public void DisableDeathActions() {
        _DeathRespawnAction.Disable();
    }

    #region Rumble
    public void SetRumble(float lowFrequency, float highFrequency) {

        if (_iPlayerInput.currentControlScheme != "Gamepad") return;
        _gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
    }

    public void StopRumble() {

        if (_iPlayerInput.currentControlScheme != "Gamepad") return;
        _gamepad.SetMotorSpeeds(0f, 0f);
    }

    public void RumbleDuration(AnimationCurve lowFrequencyCurve, AnimationCurve highFrequencyCurve, float duration) {

        if (_iPlayerInput.currentControlScheme != "Gamepad") return;
        StopAllCoroutines();
        StartCoroutine(RumbleRoutine(lowFrequencyCurve, highFrequencyCurve, duration, _gamepad));
    }

    private IEnumerator RumbleRoutine(AnimationCurve lowFrequencyCurve, AnimationCurve highFrequencyCurve, float duration, Gamepad gamepad) {

        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float percentCompleted = elapsedTime / duration;
            _gamepad.SetMotorSpeeds(lowFrequencyCurve.Evaluate(percentCompleted), highFrequencyCurve.Evaluate(percentCompleted));
            yield return null;
        }

        gamepad.SetMotorSpeeds(0f, 0f);
    }
    #endregion


}
