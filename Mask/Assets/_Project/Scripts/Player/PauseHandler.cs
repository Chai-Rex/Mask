using UnityEngine;

public class PauseHandler : MonoBehaviour {

    [SerializeField] private PauseCanvas _iPauseCanvas;
    [SerializeField] private DialogueCanvas _iDialogueCanvas;
    [SerializeField] private DialogueHandler _iDialogueHandler;

    private void Start() {
        // Subscribe to input events
        InputManager.Instance._PlayerPauseAction.started += OnPauseStarted;
        InputManager.Instance._DialoguePauseAction.started += OnPauseStarted;
        InputManager.Instance._UIResumeAction.started += OnResumeStarted;

        // Hide pause canvas
        _iPauseCanvas.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        if (InputManager.Instance == null) return;
        InputManager.Instance._PlayerPauseAction.started -= OnPauseStarted;
        InputManager.Instance._DialoguePauseAction.started -= OnPauseStarted;
        InputManager.Instance._UIResumeAction.started -= OnResumeStarted;
    }

    private void OnPauseStarted(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        PauseGame();
    }
    private void OnResumeStarted(UnityEngine.InputSystem.InputAction.CallbackContext context) {

        ResumeGame();
    }

    public void PauseGame() {
        _iPauseCanvas.gameObject.SetActive(true);
        InputManager.Instance.SetUIActionMap();

        if (_iDialogueCanvas.isActiveAndEnabled) {
            _iDialogueHandler.Pause();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame() {
        _iPauseCanvas.gameObject.SetActive(false);

        if (_iDialogueCanvas.isActiveAndEnabled) {
            InputManager.Instance.SetDialogueActionMap();
            _iDialogueHandler.Resume();
        } else {
            InputManager.Instance.SetPlayerActionMap();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
