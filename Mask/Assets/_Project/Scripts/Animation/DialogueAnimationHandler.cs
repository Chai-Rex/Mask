using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class DialogueAnimationHandler : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform _iBodyTransform;
    [SerializeField] private Transform _iHeadTransform;

    [Header("Default EmotionPreset")]
    [SerializeField] private EmotionPresetSO _iEmotionPreset;

    [Header("Animation Profile")]
    [SerializeField] private NPCAnimationProfile _iAnimationProfile;

    private Vector3 _originalScale;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Quaternion _originalHeadRotation;

    private Sequence _speakingSequence;
    private CancellationTokenSource _animationCancellation;

    private bool _isSpeaking = false;

    // Store current settings values
    private bool _enableSquashStretch;
    private bool _enableSway;
    private bool _enableBob;
    private bool _enableBreathe;
    private bool _enableHeadTilt;
    private float _squashAmount;
    private float _squashSpeed;
    private AnimationCurve _squashCurve;
    private float _swayAmount;
    private float _swaySpeed;
    private AnimationCurve _swayCurve;
    private bool _randomizeSwayDirection;
    private float _bobAmount;
    private float _bobSpeed;
    private AnimationCurve _bobCurve;
    private float _breatheAmount;
    private float _breatheSpeed;
    private AnimationCurve _breatheCurve;
    private float _tiltAmount;
    private float _tiltSpeed;
    private float _tiltChangeInterval;
    private AnimationCurve _tiltCurve;
    private float _currentSpeedMultiplier;
    private float _currentIntensityMultiplier;

    public bool IsSpeaking => _isSpeaking;

    private void Awake() {
        if (_iBodyTransform == null) {
            _iBodyTransform = transform;
        }

        _originalScale = _iBodyTransform.localScale;
        _originalPosition = _iBodyTransform.localPosition;
        _originalRotation = _iBodyTransform.localRotation;

        if (_iHeadTransform != null) {
            _originalHeadRotation = _iHeadTransform.localRotation;
        }

        // Initialize current settings with defaults
        ResetToDefaults();

        // Register presets with parser
        if (_iAnimationProfile != null) {
            _iAnimationProfile.RegisterPresetsWithParser();
        }
    }

    private void OnDestroy() {
        StopSpeaking();
        _animationCancellation?.Cancel();
        _animationCancellation?.Dispose();
    }

    private void ResetToDefaults() {
        _enableSquashStretch = _iEmotionPreset._UseSquashStretch;
        _enableSway = _iEmotionPreset._UseSway;
        _enableBob = _iEmotionPreset._UseBob;
        _enableBreathe = _iEmotionPreset._UseBreathe;
        _enableHeadTilt = _iEmotionPreset._UseHeadTilt;
        _squashAmount = _iEmotionPreset._SquashAmount;
        _squashSpeed = _iEmotionPreset._SquashSpeed;
        _squashCurve = _iEmotionPreset._SquashCurve;
        _swayAmount = _iEmotionPreset._SwayAmount;
        _swaySpeed = _iEmotionPreset._SwaySpeed;
        _swayCurve = _iEmotionPreset._SwayCurve;
        _randomizeSwayDirection = _iEmotionPreset._RandomizeSwayDirection;
        _bobAmount = _iEmotionPreset._BobAmount;
        _bobSpeed = _iEmotionPreset._BobSpeed;
        _bobCurve = _iEmotionPreset._BobCurve;
        _breatheAmount = _iEmotionPreset._BreatheAmount;
        _breatheSpeed = _iEmotionPreset._BreatheSpeed;
        _breatheCurve = _iEmotionPreset._BreatheCurve;
        _tiltAmount = _iEmotionPreset._TiltAmount;
        _tiltSpeed = _iEmotionPreset._TiltSpeed;
        _tiltChangeInterval = _iEmotionPreset._TiltChangeInterval;
        _tiltCurve = _iEmotionPreset._TiltCurve;
        _currentSpeedMultiplier = _iEmotionPreset._SpeedMultiplier;
        _currentIntensityMultiplier = _iEmotionPreset._IntensityMultiplier;
    }

    public void StartSpeaking() {
        StartSpeaking(null);
    }

    /// <summary>
    /// Start speaking with an optional emotion preset
    /// </summary>
    public void StartSpeaking(string i_emotionPresetName = null) {
        if (_isSpeaking) {
            return;
        }

        // Apply emotion preset if specified
        if (!string.IsNullOrEmpty(i_emotionPresetName)) {
            ApplyEmotionPreset(i_emotionPresetName);
        } else {
            ResetToDefaults();
        }

        _isSpeaking = true;

        _speakingSequence?.Kill();
        _iBodyTransform.DOKill();
        if (_iHeadTransform != null) {
            _iHeadTransform.DOKill();
        }

        if (_enableSquashStretch) {
            StartSquashStretch();
        }

        if (_enableSway) {
            StartSway();
        }

        if (_enableBob) {
            StartBob();
        }

        if (_enableBreathe) {
            StartBreathe();
        }

        if (_enableHeadTilt && _iHeadTransform != null) {
            _animationCancellation?.Cancel();
            _animationCancellation?.Dispose();
            _animationCancellation = new CancellationTokenSource();
            StartHeadTilt(_animationCancellation.Token);
        }
    }

    private void ApplyEmotionPreset(string i_presetName = null) {
        if (_iAnimationProfile == null) {
            Debug.LogWarning($"No animation profile assigned to {gameObject.name}");
            ResetToDefaults();
            return;
        }

        EmotionPresetSO preset = _iAnimationProfile.GetPreset(i_presetName);
        if (preset != null) {
            // Apply toggles
            _enableSquashStretch = preset._UseSquashStretch;
            _enableSway = preset._UseSway;
            _enableBob = preset._UseBob;
            _enableBreathe = preset._UseBreathe;
            _enableHeadTilt = preset._UseHeadTilt;

            // Apply squash & stretch settings
            _squashAmount = preset._SquashAmount;
            _squashSpeed = preset._SquashSpeed;
            _squashCurve = preset._SquashCurve;

            // Apply sway settings
            _swayAmount = preset._SwayAmount;
            _swaySpeed = preset._SwaySpeed;
            _swayCurve = preset._SwayCurve;
            _randomizeSwayDirection = preset._RandomizeSwayDirection;

            // Apply bob settings
            _bobAmount = preset._BobAmount;
            _bobSpeed = preset._BobSpeed;
            _bobCurve = preset._BobCurve;

            // Apply breathe settings
            _breatheAmount = preset._BreatheAmount;
            _breatheSpeed = preset._BreatheSpeed;
            _breatheCurve = preset._BreatheCurve;

            // Apply head tilt settings
            _tiltAmount = preset._TiltAmount;
            _tiltSpeed = preset._TiltSpeed;
            _tiltChangeInterval = preset._TiltChangeInterval;
            _tiltCurve = preset._TiltCurve;

            // Apply multipliers
            _currentSpeedMultiplier = preset._SpeedMultiplier;
            _currentIntensityMultiplier = preset._IntensityMultiplier;
        } else {
            Debug.LogWarning($"Preset '{i_presetName}' not found in animation profile for {gameObject.name}");
            ResetToDefaults();
        }
    }

    public void StopSpeaking() {
        if (!_isSpeaking) {
            return;
        }

        _isSpeaking = false;
        _animationCancellation?.Cancel();

        _speakingSequence?.Kill();

        // Kill body animations
        _iBodyTransform.DOKill();

        // Only kill head tilt loop, not trigger animations like nod
        if (_iHeadTransform != null) {
            // DOTween will allow trigger animations with SetId("trigger") to continue
            DOTween.Kill(_iHeadTransform, false); // false = don't complete tweens
        }

        _iBodyTransform.DOScale(_originalScale, 0.2f).SetEase(Ease.OutQuad);
        _iBodyTransform.DOLocalMove(_originalPosition, 0.2f).SetEase(Ease.OutQuad);
        _iBodyTransform.DOLocalRotate(_originalRotation.eulerAngles, 0.2f).SetEase(Ease.OutQuad);

        // Don't reset head rotation here - let trigger animations finish naturally
    }

    private void StartSquashStretch() {
        float adjustedAmount = _squashAmount * _currentIntensityMultiplier;
        float adjustedSpeed = _squashSpeed / _currentSpeedMultiplier;

        Vector3 squashScale = new Vector3(
            _originalScale.x * (1 + adjustedAmount),
            _originalScale.y * (1 - adjustedAmount),
            _originalScale.z * (1 + adjustedAmount)
        );

        _iBodyTransform.DOScale(squashScale, adjustedSpeed)
            .SetEase(_squashCurve)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StartSway() {
        float swayDirection = _randomizeSwayDirection ? UnityEngine.Random.Range(-1f, 1f) : 1f;
        float adjustedAmount = _swayAmount * _currentIntensityMultiplier;
        float adjustedSpeed = _swaySpeed / _currentSpeedMultiplier;
        float swayAngle = adjustedAmount * swayDirection;

        _iBodyTransform.DOLocalRotate(new Vector3(0, 0, swayAngle), adjustedSpeed)
            .SetEase(_swayCurve)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StartBob() {
        float adjustedAmount = _bobAmount * _currentIntensityMultiplier;
        float adjustedSpeed = _bobSpeed / _currentSpeedMultiplier;

        Vector3 bobPosition = _originalPosition + Vector3.up * adjustedAmount;

        _iBodyTransform.DOLocalMove(bobPosition, adjustedSpeed / 2f)
            .SetEase(_bobCurve)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StartBreathe() {
        float adjustedAmount = _breatheAmount * _currentIntensityMultiplier;
        float adjustedSpeed = _breatheSpeed / _currentSpeedMultiplier;

        Vector3 breatheScale = new Vector3(
            _originalScale.x * (1 + adjustedAmount),
            _originalScale.y * (1 + adjustedAmount),
            _originalScale.z * (1 + adjustedAmount)
        );

        Sequence breatheSequence = DOTween.Sequence();
        breatheSequence.Append(_iBodyTransform.DOScale(breatheScale, adjustedSpeed / 2f).SetEase(_breatheCurve));
        breatheSequence.Append(_iBodyTransform.DOScale(_originalScale, adjustedSpeed / 2f).SetEase(_breatheCurve));
        breatheSequence.SetLoops(-1);
    }

    private async void StartHeadTilt(CancellationToken i_cancellationToken) {
        while (!i_cancellationToken.IsCancellationRequested && _isSpeaking) {
            try {
                float adjustedAmount = _tiltAmount * _currentIntensityMultiplier;
                float adjustedSpeed = _tiltSpeed / _currentSpeedMultiplier;

                float tiltX = UnityEngine.Random.Range(-adjustedAmount, adjustedAmount);
                float tiltZ = UnityEngine.Random.Range(-adjustedAmount, adjustedAmount);

                Vector3 targetRotation = _originalHeadRotation.eulerAngles + new Vector3(tiltX, 0, tiltZ);

                await _iHeadTransform.DOLocalRotate(targetRotation, adjustedSpeed)
                    .SetEase(_tiltCurve)
                    .AsyncWaitForCompletion();

                await Task.Delay(TimeSpan.FromSeconds(_tiltChangeInterval / _currentSpeedMultiplier), i_cancellationToken);
            } catch (OperationCanceledException) {
                break;
            }
        }
    }

    /// <summary>
    /// Change animation style on the fly during speaking
    /// </summary>
    public void SetEmotionPreset(string i_presetName = null) {
        // Apply new preset
        if (!string.IsNullOrEmpty(i_presetName)) {
            ApplyEmotionPreset(i_presetName);
        } else {
            ResetToDefaults();
        }

        // Only restart animations if we're currently speaking
        if (!_isSpeaking) return;

        // Kill current looping animations (but not one-shot triggers like nod)
        _speakingSequence?.Kill();
        _iBodyTransform.DOKill();

        // Only kill head tilt (async loop), not one-shot head animations
        _animationCancellation?.Cancel();

        // Restart with new settings
        if (_enableSquashStretch) {
            StartSquashStretch();
        }

        if (_enableSway) {
            StartSway();
        }

        if (_enableBob) {
            StartBob();
        }

        if (_enableBreathe) {
            StartBreathe();
        }

        if (_enableHeadTilt && _iHeadTransform != null) {
            _animationCancellation?.Cancel();
            _animationCancellation?.Dispose();
            _animationCancellation = new CancellationTokenSource();
            StartHeadTilt(_animationCancellation.Token);
        }
    }

    public void TriggerEmphasis() {
        if (!_isSpeaking) return;

        Sequence emphasisSequence = DOTween.Sequence();
        Vector3 emphasisScale = _originalScale * 1.15f;

        emphasisSequence.Append(_iBodyTransform.DOScale(emphasisScale, 0.1f).SetEase(Ease.OutQuad));
        emphasisSequence.Append(_iBodyTransform.DOScale(_originalScale, 0.2f).SetEase(Ease.InOutQuad));
    }

    public void TriggerShake(float intensity = 0.1f, float duration = 0.3f) {
        if (!_isSpeaking) return;
        _iBodyTransform.DOShakePosition(duration, intensity * _currentIntensityMultiplier, 10, 90, false, true);
    }

    public void TriggerNod() {
        if (_iHeadTransform == null) return;

        // Don't check if speaking - allow this to trigger anytime
        Sequence nodSequence = DOTween.Sequence();
        Vector3 nodDown = _originalHeadRotation.eulerAngles + new Vector3(15, 0, 0);

        // Use SetId to prevent this from being killed by SetEmotionPreset
        nodSequence.Append(_iHeadTransform.DOLocalRotate(nodDown, 0.15f).SetEase(Ease.OutQuad));
        nodSequence.Append(_iHeadTransform.DOLocalRotate(_originalHeadRotation.eulerAngles, 0.15f).SetEase(Ease.InQuad));
        nodSequence.SetLoops(2);
        nodSequence.SetId("trigger"); // Mark as trigger animation
    }

    public void TriggerHeadShake() {
        if (_iHeadTransform == null) return;

        Sequence shakeSequence = DOTween.Sequence();
        Vector3 shakeLeft = _originalHeadRotation.eulerAngles + new Vector3(0, -15, 0);
        Vector3 shakeRight = _originalHeadRotation.eulerAngles + new Vector3(0, 15, 0);

        shakeSequence.Append(_iHeadTransform.DOLocalRotate(shakeLeft, 0.15f).SetEase(Ease.InOutQuad));
        shakeSequence.Append(_iHeadTransform.DOLocalRotate(shakeRight, 0.15f).SetEase(Ease.InOutQuad));
        shakeSequence.Append(_iHeadTransform.DOLocalRotate(_originalHeadRotation.eulerAngles, 0.15f).SetEase(Ease.InOutQuad));
        shakeSequence.SetLoops(2);
        shakeSequence.SetId("trigger"); // Mark as trigger animation
    }
}