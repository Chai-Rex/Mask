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
    public InputAction _MoveAction { get; private set; }
    public InputAction _LookAction { get; private set; }
    public InputAction _InteractAction { get; private set; }
    public InputAction _CrouchAction { get; private set; }
    public InputAction _JumpAction { get; private set; }
    public InputAction _SprintAction { get; private set; }
    public InputAction _CancelAction { get; private set; }

    // UI
    public InputAction _NavigateAction { get; private set; }

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
        _MoveAction = BuildAction("Move");
        _LookAction = BuildAction("Look");
        _InteractAction = BuildAction("Interact");
        _CrouchAction = BuildAction("Crouch");
        _JumpAction = BuildAction("Jump");
        _SprintAction = BuildAction("Sprint");
        _CancelAction = BuildAction("Cancel");
        _NavigateAction = BuildAction("Navigate");
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
        _MoveAction.Enable();
        _LookAction.Enable();
        _InteractAction.Enable();
        _CrouchAction.Enable();
        _JumpAction.Enable();
        _SprintAction.Enable();
        _CancelAction.Enable();
    }

    public void DisablePlayerActions() {
        _MoveAction.Disable();
        _LookAction.Disable();
        _InteractAction.Disable();
        _CrouchAction.Disable();
        _JumpAction.Disable();
        _SprintAction.Disable();
        _CancelAction.Disable();
    }

    public void EnableUIActions() {
        _NavigateAction.Enable();
    }

    public void DisableUIActions() {
        _NavigateAction.Disable();
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
