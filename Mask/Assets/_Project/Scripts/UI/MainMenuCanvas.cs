using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour {

    [SerializeField] private Button _StartButton;
    [SerializeField] private Button _QuitButton;

    [SerializeField] private LevelManager.Levels levelToLoad;

    private void Awake() {
        _StartButton.onClick.AddListener(StartGame);
        _QuitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy() {
        _StartButton.onClick.RemoveAllListeners();
        _QuitButton.onClick.RemoveAllListeners();
    }

    public void StartGame() {
        LevelManager.Instance.LoadScene(levelToLoad);
    }

    public void Quit() {
        Application.Quit();
    }

}
