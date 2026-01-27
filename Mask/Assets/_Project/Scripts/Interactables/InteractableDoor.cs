using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable {
    [SerializeField] private string verb = "Open";
    [SerializeField] private string verbWhenOpen = "Close";
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isAnimating = false;

    public string InteractionVerb => isOpen ? verbWhenOpen : verb;

    private void Awake() {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void OnInteract(GameObject interactor) {
        if (!isAnimating) {
            isOpen = !isOpen;
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
        isAnimating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        float elapsed = 0f;

        while (elapsed < 1f) {
            elapsed += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed);
            yield return null;
        }

        transform.rotation = targetRotation;
        isAnimating = false;
    }
}