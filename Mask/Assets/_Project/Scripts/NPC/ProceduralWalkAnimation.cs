using DG.Tweening;
using UnityEngine;

public class ProceduralWalkAnimation : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform _iBody;
    [SerializeField] private Transform _iHead;

    [Header("Timing")]
    [SerializeField] private float _iStepDuration = 0.4f;

    [Header("Body Squash & Stretch")]
    [SerializeField] private float _iSquashAmount = 0.08f;
    [SerializeField] private float _iStretchAmount = 0.1f;

    [Header("Head Motion")]
    [SerializeField] private float _iHeadBobHeight = 0.05f;
    [SerializeField] private float _iHeadTiltAngle = 5f;

    [Header("Easing")]
    [SerializeField] private Ease _iWalkEase = Ease.InOutSine;

    private Vector3 _bodyBaseScale;
    private Vector3 _headBaseLocalPos;
    private Quaternion _headBaseLocalRot;

    private Tween _walkTween;

    private void Awake() {
        _bodyBaseScale = _iBody.localScale;
        _headBaseLocalPos = _iHead.localPosition;
        _headBaseLocalRot = _iHead.localRotation;
    }

    /// <summary>
    /// Call when the NPC starts moving.
    /// Speed is used to scale animation intensity.
    /// </summary>
    public void StartWalking(float speed01) {
        StopWalking();

        float speedMultiplier = Mathf.Clamp01(speed01);
        float duration = _iStepDuration / Mathf.Lerp(0.6f, 1.2f, speedMultiplier);

        _walkTween = DOTween.Sequence()
            .SetLoops(-1)
            .SetEase(_iWalkEase)

            // BODY: squash
            .Append(_iBody.DOScale(
                new Vector3(
                    _bodyBaseScale.x + _iSquashAmount,
                    _bodyBaseScale.y - _iSquashAmount,
                    _bodyBaseScale.z + _iSquashAmount
                ),
                duration * 0.5f))

            // HEAD: down + tilt
            .Join(_iHead.DOLocalMoveY(
                _headBaseLocalPos.y - _iHeadBobHeight * speedMultiplier,
                duration * 0.5f))
            .Join(_iHead.DOLocalRotate(
                new Vector3(_iHeadTiltAngle * speedMultiplier, 0f, 0f),
                duration * 0.5f))

            // BODY: stretch
            .Append(_iBody.DOScale(
                new Vector3(
                    _bodyBaseScale.x - _iStretchAmount,
                    _bodyBaseScale.y + _iStretchAmount,
                    _bodyBaseScale.z - _iStretchAmount
                ),
                duration * 0.5f))

            // HEAD: up + reset tilt
            .Join(_iHead.DOLocalMoveY(
                _headBaseLocalPos.y + _iHeadBobHeight * speedMultiplier,
                duration * 0.5f))
            .Join(_iHead.DOLocalRotate(
                Vector3.zero,
                duration * 0.5f));
    }

    /// <summary>
    /// Call when movement stops.
    /// </summary>
    public void StopWalking() {
        _walkTween?.Kill();

        _iBody.localScale = _bodyBaseScale;
        _iHead.localPosition = _headBaseLocalPos;
        _iHead.localRotation = _headBaseLocalRot;
    }
}
