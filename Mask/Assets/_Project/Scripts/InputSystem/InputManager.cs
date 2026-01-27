using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilitySingletons;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : PersistentSingleton<InputManager> {

    public event UnityAction<string> OnControlsChanged;

    // Player
    public InputAction MoveAction { get; private set; }
    public InputAction LookAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction CrouchAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction SprintAction { get; private set; }
    public InputAction CancelAction { get; private set; }

    // UI
    public InputAction NavigateAction { get; private set; }

    private PlayerInput _playerInput;
    private InputActionAsset _inputActionsAsset;
    private Gamepad _gamepad;

    protected override void Awake() {
        base.Awake();

        _playerInput = GetComponent<PlayerInput>();

        _playerInput.onControlsChanged += PlayerInput_onControlsChanged;

        _inputActionsAsset = _playerInput.actions;

        GetInputActions();
    }

    private void OnDestroy() {
        _playerInput.onControlsChanged -= PlayerInput_onControlsChanged;
    }

    private void PlayerInput_onControlsChanged(PlayerInput obj) {
        if (_playerInput.currentControlScheme == "Gamepad") _gamepad = Gamepad.current;

        OnControlsChanged?.Invoke(_playerInput.currentControlScheme);
    }

    private void GetInputActions() {
        MoveAction = BuildAction("Move");
        LookAction = BuildAction("Look");
        InteractAction = BuildAction("Interact");
        CrouchAction = BuildAction("Crouch");
        JumpAction = BuildAction("Jump");
        SprintAction = BuildAction("Sprint");
        CancelAction = BuildAction("Cancel");
        NavigateAction = BuildAction("Navigate");
    }

    private InputAction BuildAction(string i_name) {
        InputAction action = _inputActionsAsset?.FindAction(i_name, throwIfNotFound: false);
        if (action != null)
            action.Enable();
        else
            Debug.LogWarning($"Action '{i_name}' not found!");
        return action;
    }

    public void EnablePlayerActions() {
        MoveAction.Enable();
        LookAction.Enable();
        InteractAction.Enable();
        CrouchAction.Enable();
        JumpAction.Enable();
        SprintAction.Enable();
        CancelAction.Enable();
    }

    public void DisablePlayerActions() {
        MoveAction.Disable();
        LookAction.Disable();
        InteractAction.Disable();
        CrouchAction.Disable();
        JumpAction.Disable();
        SprintAction.Disable();
        CancelAction.Disable();
    }

    public void EnableUIActions() {
        NavigateAction.Enable();
    }

    public void DisableUIActions() {
        NavigateAction.Disable();
    }

    #region Rumble
    public void SetRumble(float lowFrequency, float highFrequency) {

        if (_playerInput.currentControlScheme != "Gamepad") return;
        _gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
    }

    public void StopRumble() {

        if (_playerInput.currentControlScheme != "Gamepad") return;
        _gamepad.SetMotorSpeeds(0f, 0f);
    }

    public void RumbleDuration(AnimationCurve lowFrequencyCurve, AnimationCurve highFrequencyCurve, float duration) {

        if (_playerInput.currentControlScheme != "Gamepad") return;
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
