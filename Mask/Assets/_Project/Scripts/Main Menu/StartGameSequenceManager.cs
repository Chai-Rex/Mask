using System.Threading.Tasks;
using UnityEngine;
using UtilitySingletons;
public class StartGameSequenceManager : Singleton<StartGameSequenceManager> {

    [Header("UI")]
    [SerializeField] private GameObject _iMainMenu;

    [Header("Door")]
    [SerializeField] private Animator _iDoorAnimator;

    [Header("Camera Movement")]
    [SerializeField] private GameObject _iCamera;
    [SerializeField] private Transform _iCameraTarget;
    [SerializeField] private float _iMoveDurationSeconds;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Level Loading")]
    [SerializeField] private LevelManager.Levels levelToLoad;
    public async void StartGame() {

        TurnOffUI();
        OpenDoor();
        await MoveCamera();
        LoadLevel();
    }


    private void TurnOffUI() {
        _iMainMenu.SetActive(false);
    }

    private void OpenDoor() {
        _iDoorAnimator.Play("OpenDoors");
    }

    private async Task MoveCamera() {

            Vector3 startPosition = _iCamera.transform.position;
            float elapsed = 0f;

            while (elapsed < _iMoveDurationSeconds) {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _iMoveDurationSeconds);
                float curveT = moveCurve.Evaluate(t);

                _iCamera.transform.position = Vector3.Lerp(
                    startPosition,
                    _iCameraTarget.transform.position,
                    curveT
                );

                await Task.Yield();
            }

        // Snap exactly at the end
        _iCamera.transform.position = _iCameraTarget.transform.position;
        }

    private void LoadLevel() {
        LevelManager.Instance.LoadScene(levelToLoad);
    }

}
