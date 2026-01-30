using UnityEngine;
using UtilitySingletons;
public class DeathManager : Singleton<DeathManager> {

    [SerializeField] private EyesCanvas _iEyeCanvas;
    [SerializeField] private LevelManager.Levels levelToLoad;

    private StateVariable _hasDiedState = new StateVariable("hasPlayerDied", false);

    private void Start() {
        AwakenSequence();
    }

    private async void AwakenSequence() {
        _iEyeCanvas.gameObject.SetActive(true);
        InputManager.Instance.SetDeathActionMap();
        await _iEyeCanvas.OpenEyes();
        InputManager.Instance.SetPlayerActionMap();
        _iEyeCanvas.gameObject.SetActive(false);

        InputManager.Instance._PlayerJumpAction.started += _PlayerJumpAction_started; // remove
        InputManager.Instance._DeathRespawnAction.started += _DeathRespawnAction_started;
    }

    private void OnDestroy() {
        if (InputManager.Instance == null) return;
        InputManager.Instance._PlayerJumpAction.started -= _PlayerJumpAction_started; // remove
        InputManager.Instance._DeathRespawnAction.started -= _DeathRespawnAction_started;
    }

    private void _PlayerJumpAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        Die("BEEs");
    }

    private void _DeathRespawnAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {

        // reset blackboard
        StoryStateSO.Instance.ResetState();
        _hasDiedState.SetValueAndUpdateBlackboard(true);

        InputManager.Instance.SetPlayerActionMap();
        LevelManager.Instance.LoadScene(levelToLoad);

        
    }

    public async void Die(string i_causeOfDeath) {

        InputManager.Instance.SetDeathActionMap();

        _iEyeCanvas.gameObject.SetActive(true);

        await _iEyeCanvas.CloseEyes();

        _iEyeCanvas.SetCauseOfDeathText($"[ {TimeManager.instance.GetTime()} ] died to {i_causeOfDeath}");

        InputManager.Instance.SetDeathActionMap();
    }




}
