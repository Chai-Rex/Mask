using AudioSystem;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour {

    [Header("Buttons")]
    [SerializeField] private Button _iStartButton;
    [SerializeField] private Button _iQuitButton;

    [Header("Sliders")]
    [SerializeField] private Slider _iMasterVolumeSlider;

    private SaveSettingsSO _settings;

    const string MIXER_MASTER = "MasterVolume";

    private void Start() {

        _settings = SaveManager.Instance.SaveSettingsSO;

        _iMasterVolumeSlider.value = _settings.MasterVolume;

        // Slider
        _iMasterVolumeSlider.onValueChanged.AddListener((float value) => { _settings.MasterVolume = value; SoundManager.Instance.SetMixerFloat(MIXER_MASTER, value); });

        // Buttons
        _iStartButton.onClick.AddListener(StartGame);
        _iQuitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy() {

        _iMasterVolumeSlider.onValueChanged.RemoveAllListeners();

        _iStartButton.onClick.RemoveAllListeners();
        _iQuitButton.onClick.RemoveAllListeners();
    }

    private void OnDisable() {
        SaveManager.Instance.SaveSettings();
    }

    public void StartGame() {
        StartGameSequenceManager.Instance.StartGame();
    }

    public void Quit() {
        Application.Quit();
    }

}
