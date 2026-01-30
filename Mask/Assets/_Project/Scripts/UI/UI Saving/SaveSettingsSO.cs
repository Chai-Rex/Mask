using UnityEngine;

[CreateAssetMenu(fileName = "SaveSettingsSO", menuName = "Scriptable Objects/SaveSettingsSO")]
public class SaveSettingsSO : ScriptableObject {


    [Range(0, 1)] public float MasterVolume;
    [Range(0, 1)] public float MusicVolume;
    [Range(0, 1)] public float EffectsVolume;
    [Range(0, 1)] public float DialogueVolume;
    [Range(0, 1)] public float Sensitivity;


}