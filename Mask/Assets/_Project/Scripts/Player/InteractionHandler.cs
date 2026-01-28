using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour {

    [Header("Raycast Settings")]
    [SerializeField] private Transform _iCameraTransform;
    [SerializeField] private float _iInteractionRange = 3f;
    [SerializeField] private LayerMask _iInteractableLayer;
    [SerializeField] private LayerMask _iInHandLayer;

    [Header("Debug")]
    [SerializeField] private bool _iShowDebugRay = true;

    // Events for UI to subscribe to
    public UnityEvent<string> _OnInteractableFound; // Passes the verb string
    public UnityEvent _OnInteractableLost;

    private IInteractable _currentInteractable;
    private GameObject _currentInteractableObject;

    private GameObject _currentInHandObject;

    public IInteractable CurrentInteractable => _currentInteractable;
    public bool HasInteractable => _currentInteractable != null;

    private void Awake() {
        if (_iCameraTransform == null) {
            _iCameraTransform = Camera.main.transform;
        }
    }

    private void Start() {
        // Subscribe to interact input
        if (InputManager.Instance != null) {
            InputManager.Instance._PlayerInteractAction.started += OnInteractInput;
        }
    }

    private void OnDestroy() {
        if (InputManager.Instance != null) {
            InputManager.Instance._PlayerInteractAction.started -= OnInteractInput;
        }
    }

    private void Update() {
        CheckForInteractable();
    }

    private void CheckForInteractable() {
        Ray ray = new Ray(_iCameraTransform.position, _iCameraTransform.forward);

        if (_iShowDebugRay) {
            Debug.DrawRay(ray.origin, ray.direction * _iInteractionRange,
                _currentInteractable != null ? Color.green : Color.red);
        }

        if (Physics.Raycast(ray, out RaycastHit hitInteractable, _iInteractionRange, _iInteractableLayer)) {
            IInteractable interactable = hitInteractable.collider.GetComponent<IInteractable>();

            if (interactable != null) {
                // New interactable found
                if (_currentInteractable != interactable) {
                    // Exit previous interactable
                    if (_currentInteractable != null) {
                        _currentInteractable.OnLookExit(gameObject);
                    }

                    // Enter new interactable
                    _currentInteractable = interactable;
                    _currentInteractableObject = hitInteractable.collider.gameObject;
                    _currentInteractable.OnLookEnter(gameObject);

                    // Notify UI
                    _OnInteractableFound?.Invoke("[e] " + _currentInteractable.InteractionVerb);
                }
                return;
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hitHand, _iInteractionRange, _iInHandLayer)) {
            if (_currentInHandObject != hitHand.collider.gameObject) {
                _currentInHandObject = hitHand.collider.gameObject;
                _OnInteractableFound?.Invoke("[q] drop item");
            }
            return;
        }

        // No interactable found or lost line of sight
        if (_currentInteractable != null) {
            _currentInteractable.OnLookExit(gameObject);
            _currentInteractable = null;
            _currentInteractableObject = null;

            // Notify UI
            _OnInteractableLost?.Invoke();
        }

        // No InHand item found or lost line of sight
        if (_currentInHandObject != null) {
            _currentInHandObject = null;

            // Notify UI
            _OnInteractableLost?.Invoke();
        }
    }

    private void OnInteractInput(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        if (_currentInteractable != null) {
            _currentInteractable.OnInteract(gameObject);
        }
    }

    // Public methods
    public void SetInteractionRange(float i_range) {
        _iInteractionRange = i_range;
    }

    public float GetInteractionRange() {
        return _iInteractionRange;
    }
}
