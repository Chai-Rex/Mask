using UnityEngine;

[CreateAssetMenu(fileName = "EmotionPreset", menuName = "Dialogue/Emotion Preset")]
public class EmotionPresetSO : ScriptableObject {
    [Header("Preset Info")]
    public string _PresetName = "Neutral";

    [Header("Animation Toggles")]
    public bool _UseSquashStretch = false;
    public bool _UseSway = true;
    public bool _UseBob = false;
    public bool _UseBreathe = false;
    public bool _UseHeadTilt = true;

    [Header("Squash & Stretch Settings")]
    public float _SquashAmount = 0.05f;
    public float _SquashSpeed = 0.15f;
    public AnimationCurve _SquashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Sway Settings")]
    public float _SwayAmount = 2f;
    public float _SwaySpeed = 1.5f;
    public AnimationCurve _SwayCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool _RandomizeSwayDirection = true;

    [Header("Bob Settings")]
    public float _BobAmount = 0.05f;
    public float _BobSpeed = 2f;
    public AnimationCurve _BobCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Breathe Settings")]
    public float _BreatheAmount = 0.02f;
    public float _BreatheSpeed = 3f;
    public AnimationCurve _BreatheCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Head Tilt Settings")]
    public float _TiltAmount = 5f;
    public float _TiltSpeed = 0.8f;
    public float _TiltChangeInterval = 2f;
    public AnimationCurve _TiltCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Speed & Intensity Modifiers")]
    [Range(0.1f, 3f)]
    public float _SpeedMultiplier = 1f;
    [Range(0.1f, 3f)]
    public float _IntensityMultiplier = 1f;
}