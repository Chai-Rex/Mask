using UnityEngine;

public class PauseHandler : MonoBehaviour {

    [SerializeField] private PauseCanvas _iPauseCanvas;
    [SerializeField] private DialogueCanvas _iDialogueCanvas;
    [SerializeField] private DialogueHandler _iDialogueHandler;

    private void Start() {
        // Subscribe to input events
        SubscribeToInput();

        // Hide pause canvas
        _iPauseCanvas.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        // Unsubscribe from input events
        UnsubscribeFromInput();
    }

    private void SubscribeToInput() {
        if (InputManager.Instance == null) return;


        InputManager.Instance._PlayerPauseAction.started += OnPauseStarted;
        InputManager.Instance._DialoguePauseAction.started += OnPauseStarted;
        InputManager.Instance._UIUnpauseAction.started += OnUnpauseStarted;
    }

    private void UnsubscribeFromInput() {
        if (InputManager.Instance == null) return;


        InputManager.Instance._PlayerPauseAction.started -= OnPauseStarted;
        InputManager.Instance._DialoguePauseAction.started -= OnPauseStarted;
        InputManager.Instance._UIUnpauseAction.started -= OnUnpauseStarted;
    }


    private void OnPauseStarted(UnityEngine.InputSystem.InputAction.CallbackContext context) {

        _iPauseCanvas.gameObject.SetActive(true);
        InputManager.Instance.SetUIActionMap();

        if (_iDialogueCanvas.isActiveAndEnabled) {
            _iDialogueHandler.Pause();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void OnUnpauseStarted(UnityEngine.InputSystem.InputAction.CallbackContext context) {

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
