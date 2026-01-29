using UnityEngine;

[CreateAssetMenu(fileName = "NPCAnimationProfile", menuName = "Dialogue/NPC Animation Profile")]
public class NPCAnimationProfile : ScriptableObject {
    [Header("Emotion Presets")]
    [Tooltip("Emotion presets available for this NPC")]
    public EmotionPresetSO[] _EmotionPresets;

    /// <summary>
    /// Register all presets with the parser when this profile is used
    /// </summary>
    public void RegisterPresetsWithParser() {
        if (_EmotionPresets == null) return;

        foreach (var preset in _EmotionPresets) {
            if (preset != null) {
                DialogueAnimationParser.RegisterPresetAnimation(preset._PresetName);
            }
        }
    }

    /// <summary>
    /// Get a preset by name
    /// </summary>
    public EmotionPresetSO GetPreset(string i_PresetName) {
        if (_EmotionPresets == null) return null;

        foreach (var preset in _EmotionPresets) {
            if (preset != null && preset._PresetName.Equals(i_PresetName, System.StringComparison.OrdinalIgnoreCase)) {
                return preset;
            }
        }
        return null;
    }

    /// <summary>
    /// Check if this profile has a specific preset
    /// </summary>
    public bool HasPreset(string i_PresetName) {
        return GetPreset(i_PresetName) != null;
    }
}