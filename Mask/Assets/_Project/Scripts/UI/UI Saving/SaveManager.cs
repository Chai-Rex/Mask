using System.IO;
using UnityEngine;
using UtilitySingletons;
public class SaveManager : Singleton<SaveManager> {


    [SerializeField] private SaveSettingsSO _iSaveSettings;
#if UNITY_EDITOR
    [SerializeField] private bool _iIsDebug = false;
#endif
    public SaveSettingsSO SaveSettingsSO { get { return _iSaveSettings; } }

    private string SavePath => Path.Combine(Application.persistentDataPath, "settings.json");

    protected override void Awake() {
        base.Awake();

        if (!LoadSettings()) {
            // If no save file exists, initialize defaults
            _iSaveSettings.MasterVolume = 0.5f;
            _iSaveSettings.MusicVolume = 0.5f;
            _iSaveSettings.EffectsVolume = 0.5f;
            _iSaveSettings.DialogueVolume = 0.5f;
            _iSaveSettings.Sensitivity = 0.5f;
        }
    }

    public void SaveSettings() {
        string json = JsonUtility.ToJson(_iSaveSettings, true); // pretty print for debugging
        File.WriteAllText(SavePath, json);
#if UNITY_EDITOR
        if (_iIsDebug) Debug.Log($"Settings saved to {SavePath}");
#endif
    }

    /// <summary>
    /// Loads settings from disk. Returns true if successful.
    /// </summary>
    private bool LoadSettings() {
        if (File.Exists(SavePath)) {
            string json = File.ReadAllText(SavePath);
            JsonUtility.FromJsonOverwrite(json, _iSaveSettings);
#if UNITY_EDITOR
            if (_iIsDebug) Debug.Log($"Settings loaded from {SavePath}");
#endif
            return true;
        }
        return false;
    }
}
