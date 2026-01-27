using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour {

    [SerializeField] private Button _iStartButton;
    [SerializeField] private Button _iQuitButton;

    [SerializeField] private LevelManager.Levels levelToLoad;

    private void Awake() {
        _iStartButton.onClick.AddListener(StartGame);
        _iQuitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy() {
        _iStartButton.onClick.RemoveAllListeners();
        _iQuitButton.onClick.RemoveAllListeners();
    }

    public void StartGame() {
        LevelManager.Instance.LoadScene(levelToLoad);
    }

    public void Quit() {
        Application.Quit();
    }

}
