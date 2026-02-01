using AudioSystem;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UtilitySingletons;
public class DeathManager : Singleton<DeathManager> {

    [SerializeField] private EyesCanvas _iEyeCanvas;
    [SerializeField] private LevelManager.Levels levelToLoad;

    public Action HasSlept;

    private StateVariable _hasDiedState = new StateVariable("hasPlayerDied", false);

    [SerializeField]
    private SoundData _deathAudio;
    [SerializeField]
    private SoundData _fallAudio;

    private void Start() {
        AwakenSequence();
    }

    private async void AwakenSequence() {
        _iEyeCanvas.gameObject.SetActive(true);
        InputManager.Instance.SetDeathActionMap();
        await _iEyeCanvas.OpenEyes();
        InputManager.Instance.SetPlayerActionMap();
        _iEyeCanvas.gameObject.SetActive(false);

        InputManager.Instance._DeathRespawnAction.started += _DeathRespawnAction_started;
    }

    private void OnDestroy() {
        if (InputManager.Instance == null) return;
        InputManager.Instance._DeathRespawnAction.started -= _DeathRespawnAction_started;
    }

    private void _DeathRespawnAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {

        // reset blackboard
        
        _hasDiedState.SetValueAndUpdateBlackboard(true);

        InputManager.Instance.SetPlayerActionMap();
        LevelManager.Instance.LoadScene(levelToLoad);

        
    }

    public async void Die(string i_causeOfDeath, string i_deathTag) {
        StoryStateSO.Instance.RegisterDeath("hasDiedTo"+i_deathTag);
        InputManager.Instance.SetDeathActionMap();

        _iEyeCanvas.gameObject.SetActive(true);

        SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_deathAudio);

        await _iEyeCanvas.CloseEyes();

        SoundManager.Instance.CreateSound()
                .WithPosition(gameObject.transform.position)
                .Play(_fallAudio);

        _iEyeCanvas.SetCauseOfDeathText($"[ {TimeManager.instance.GetTime()} ] died to {i_causeOfDeath}");

        InputManager.Instance.SetDeathActionMap();

        StoryStateSO.Instance.ResetState();

    }

    public async void Sleep()
    {
        _iEyeCanvas.gameObject.SetActive(true);

        await _iEyeCanvas.CloseEyes();

        HasSlept?.Invoke();

        await Task.Delay(3500);

        await _iEyeCanvas.OpenEyes();

        _iEyeCanvas.gameObject.SetActive(true);
    }




}
