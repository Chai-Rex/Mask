using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UtilitySingletons;

public class LevelManager : PersistentSingleton<LevelManager> {

    [SerializeField] private GameObject _iLoadingCanvas;
    [SerializeField] private Image _iProgressImage;

    private float _target;

    public enum Levels {
        MainMenuScene,
        ChaiScene,
    }

    public async void LoadScene(Levels i_sceneName) {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        _target = 0;
        _iProgressImage.fillAmount = 0;
        int fixedDeltaTime = (int)(Time.fixedDeltaTime * 1000);

        var scene = SceneManager.LoadSceneAsync(i_sceneName.ToString());
        scene.allowSceneActivation = false;

        _iLoadingCanvas.gameObject.SetActive(true);

        do {
            await Task.Delay(fixedDeltaTime);
            _target = scene.progress;

            _iProgressImage.fillAmount = Mathf.MoveTowards(_iProgressImage.fillAmount, _target, 3 * Time.fixedDeltaTime);
        } while (scene.progress < 0.9f);
        _iProgressImage.fillAmount = 1f;

        _iLoadingCanvas.gameObject.SetActive(false);

        scene.allowSceneActivation = true;

    }

}
