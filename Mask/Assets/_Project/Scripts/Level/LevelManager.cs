using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UtilitySingletons;

public class LevelManager : PersistentSingleton<LevelManager> {

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image progressImage;

    private float _target;

    public enum Levels {
        MainMenuScene,
        ChaiScene,
    }

    public async void LoadScene(Levels sceneName) {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        _target = 0;
        progressImage.fillAmount = 0;
        int fixedDeltaTime = (int)(Time.fixedDeltaTime * 1000);

        var scene = SceneManager.LoadSceneAsync(sceneName.ToString());
        scene.allowSceneActivation = false;

        loadingCanvas.gameObject.SetActive(true);

        do {
            await Task.Delay(fixedDeltaTime);
            _target = scene.progress;

            progressImage.fillAmount = Mathf.MoveTowards(progressImage.fillAmount, _target, 3 * Time.fixedDeltaTime);
        } while (scene.progress < 0.9f);
        progressImage.fillAmount = 1f;

        loadingCanvas.gameObject.SetActive(false);

        scene.allowSceneActivation = true;

    }

}
