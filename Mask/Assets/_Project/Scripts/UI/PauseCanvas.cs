using AudioSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour {

    [SerializeField] private LevelManager.Levels levelToLoad;
    [SerializeField] private PauseHandler pauseHandler;

    [Header("Buttons")]
    [SerializeField] private Button _iResumeButton;
    [SerializeField] private Button _iMainMenuButton;
    [SerializeField] private Button _iQuitButton;
    [SerializeField] private Button _iFullScreenButton;

    [Header("Sliders")]
    [SerializeField] private Slider _iMasterVolumeSlider;
    [SerializeField] private Slider _iMusicVolumeSlider;
    [SerializeField] private Slider _iEffectsVolumeSlider;
    [SerializeField] private Slider _iDialogueVolumeSlider;
    [SerializeField] private Slider _iLookSensitivitySlider;

    private SaveSettingsSO _settings;

    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";
    const string MIXER_DIALOGUE = "DialogueVolume";

    private void Start() {

        _settings = SaveManager.Instance.SaveSettingsSO;

        _iMasterVolumeSlider.value = _settings.MasterVolume;
        _iMusicVolumeSlider.value = _settings.MusicVolume;
        _iEffectsVolumeSlider.value = _settings.EffectsVolume;
        _iDialogueVolumeSlider.value = _settings.DialogueVolume;
        _iLookSensitivitySlider.value = _settings.Sensitivity;

        // Slider
        _iMasterVolumeSlider.onValueChanged.AddListener((float value) => { _settings.MasterVolume = value; SoundManager.Instance.SetMixerFloat(MIXER_MASTER, value); });
        _iMusicVolumeSlider.onValueChanged.AddListener((float value) => { _settings.MusicVolume = value; SoundManager.Instance.SetMixerFloat(MIXER_MUSIC, value); });
        _iEffectsVolumeSlider.onValueChanged.AddListener((float value) => { _settings.EffectsVolume = value; SoundManager.Instance.SetMixerFloat(MIXER_SFX, value); });
        _iDialogueVolumeSlider.onValueChanged.AddListener((float value) => { _settings.DialogueVolume = value; SoundManager.Instance.SetMixerFloat(MIXER_DIALOGUE, value); });
        _iLookSensitivitySlider.onValueChanged.AddListener((float value) => { _settings.Sensitivity = value; });

        // Buttons
        _iResumeButton.onClick.AddListener(Resume);
        _iMainMenuButton.onClick.AddListener(ReturnToMenu);
        _iQuitButton.onClick.AddListener(Quit);
        _iFullScreenButton.onClick.AddListener(FullScreen);
    }

    private void OnDestroy() {

        _iMasterVolumeSlider.onValueChanged.RemoveAllListeners();
        _iMusicVolumeSlider.onValueChanged.RemoveAllListeners();
        _iEffectsVolumeSlider.onValueChanged.RemoveAllListeners();
        _iDialogueVolumeSlider.onValueChanged.RemoveAllListeners();
        _iLookSensitivitySlider.onValueChanged.RemoveAllListeners();

        _iResumeButton.onClick.RemoveAllListeners();
        _iMainMenuButton.onClick.RemoveAllListeners();
        _iQuitButton.onClick.RemoveAllListeners();
        _iFullScreenButton.onClick.RemoveAllListeners();
    }

    private void OnDisable() {
        SaveManager.Instance.SaveSettings();
    }

    public void Resume() {
        pauseHandler.ResumeGame();
    }

    public void ReturnToMenu() {
        LevelManager.Instance.LoadScene(levelToLoad);
    }

    public void Quit() {
        Application.Quit();
    }

    public void FullScreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }

}
