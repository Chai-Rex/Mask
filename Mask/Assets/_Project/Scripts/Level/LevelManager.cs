using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UtilitySingletons;

public class LevelManager : PersistentSingleton<LevelManager> {
    [SerializeField] private GameObject _iLoadingCanvas;
    [SerializeField] private Image _iProgressImage;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private DialogueHandler _dialogueHandler;

    public enum Levels {
        MainMenuScene,
        Lvl_Mansion_Blockout,
        ChaiScene,
        EventTestScene,
        DialogTestScene
    }

    public void LoadScene(Levels i_sceneName) {
        StartCoroutine(LoadSceneAsync(i_sceneName));
    }

    private IEnumerator LoadSceneAsync(Levels i_sceneName) {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        _iProgressImage.fillAmount = 0;
        _iLoadingCanvas.gameObject.SetActive(true);

        var scene = SceneManager.LoadSceneAsync(i_sceneName.ToString());
        scene.allowSceneActivation = false;

        while (scene.progress < 0.9f) {
            float targetProgress = scene.progress;
            _iProgressImage.fillAmount = Mathf.MoveTowards(_iProgressImage.fillAmount, targetProgress, 3 * Time.deltaTime);
            yield return null; // Wait for next frame
        }

        // Smoothly fill to 100%
        while (_iProgressImage.fillAmount < 1f) {
            _iProgressImage.fillAmount = Mathf.MoveTowards(_iProgressImage.fillAmount, 1f, 3 * Time.deltaTime);
            yield return null;
        }

        scene.allowSceneActivation = true;
        _iLoadingCanvas.gameObject.SetActive(false);
    }

    public GameObject GetPlayerObject()
    {
        return _playerObject;
    }

    public DialogueHandler GetDialogueHandler()
    {
        return _dialogueHandler;
    }
}