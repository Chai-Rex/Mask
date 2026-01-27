using UnityEngine;
using UtilitySingletons;

public class UIManager : Singleton<UIManager> {

    #region Base

    private Canvas _currentUI;

    public bool IsCurrentUIVisible() {
        return _currentUI.gameObject.activeSelf;
    }

    public bool ToggleCurrentUI() {

        bool isActive = _currentUI.gameObject.activeSelf;
        if (isActive) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        isActive = !isActive;
        _currentUI.gameObject.SetActive(isActive);
        return isActive;
    }

    public void SwitchCurrentUI(Canvas newCurrentUI) {
        _currentUI?.gameObject.SetActive(false);
        _currentUI = newCurrentUI;
        ToggleCurrentUI();
    }

    public void HideCurrentCanvas() {
        _currentUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion

    [Header("Canvas")]
    [SerializeField] private Canvas _iShopCanvas;
    [SerializeField] private Canvas _iSettingsCanvas;

    public void PauseGame() {

    }

    public void UnPauseGame() {

    }

}
