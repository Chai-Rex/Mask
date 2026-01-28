using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable {
    [SerializeField] private string _iVerb = "Open";
    [SerializeField] private string _iVerbWhenOpen = "Close";
    [SerializeField] private float _iOpenAngle = 90f;
    [SerializeField] private float _iOpenSpeed = 2f;

    private bool _isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private bool _isAnimating = false;

    public string InteractionVerb => _isOpen ? _iVerbWhenOpen : _iVerb;

    private void Awake() {
        _closedRotation = transform.rotation;
        _openRotation = _closedRotation * Quaternion.Euler(0, _iOpenAngle, 0);
    }

    public void OnInteract(GameObject interactor) {
        if (!_isAnimating) {
            _isOpen = !_isOpen;
            StartCoroutine(AnimateDoor());
        }
    }

    public void OnLookEnter(GameObject looker) {

    }

    public void OnLookExit(GameObject looker) {

    }

    public Transform GetTransform() {
        return transform;
    }

    private System.Collections.IEnumerator AnimateDoor() {
        _isAnimating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = _isOpen ? _openRotation : _closedRotation;
        float elapsed = 0f;

        while (elapsed < 1f) {
            elapsed += Time.deltaTime * _iOpenSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed);
            yield return null;
        }

        transform.rotation = targetRotation;
        _isAnimating = false;
    }
}