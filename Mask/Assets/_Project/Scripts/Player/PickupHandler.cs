using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PickupHandler : MonoBehaviour {
    [Header("References")] 
    [SerializeField] private Transform _iPickupTransform; // Where items end up
    [SerializeField] private Animator _iAnimator;

    [Header("Pickup Settings")]
    [SerializeField] private float _iPickupDuration = 0.5f;
    [SerializeField] private float _iArcHeight = 1f; // How high the arc goes
    [SerializeField] private AnimationCurve _iPickupCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Drop Settings")]
    [SerializeField] private float _iDropForwardForce = 2f;
    [SerializeField] private float _iDropUpwardForce = 1f;
    [SerializeField] private float _iPlaceDistance = 3f; // Max distance to place item
    [SerializeField] private float _iPlaceOffset = 0.1f; // How far from surface to place item
    [SerializeField] private LayerMask _iPlacementLayerMask = ~0; // What surfaces can we place on
    [SerializeField] private LayerMask _iInHandLayer;
    [SerializeField] private LayerMask _iInteractableLayer;

    [Header("Debug")]
    [SerializeField] private bool _iShowDebugRay = true;

    private Transform _cameraTransform;
    private GameObject _currentHeldItem;
    private CancellationTokenSource _pickupCancellation;

    public bool IsHoldingItem => _currentHeldItem != null;
    public GameObject HeldItem => _currentHeldItem;

    private void Awake() {
        _cameraTransform = Camera.main.transform;
    }

    private void Start() {
        // Subscribe to drop input
        if (InputManager.Instance != null) {
            InputManager.Instance._PlayerDropAction.started += _PlayerDropAction_started;
        }
    }

    private void OnDestroy() {
        if (InputManager.Instance != null) {
            InputManager.Instance._PlayerDropAction.started -= _PlayerDropAction_started;
        }

        _pickupCancellation?.Cancel();
        _pickupCancellation?.Dispose();
    }

    private void _PlayerDropAction_started(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        if (!IsHoldingItem) {
            return;
        }

        // Try to place item, if unsuccessful, drop it
        if (!TryPlaceItem()) {
            DropItem();
        }

        _iAnimator.Play("Drop");
    }

    /// <summary>
    /// Attempt to place the item on a surface by raycasting
    /// </summary>
    private bool TryPlaceItem() {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);

        if (_iShowDebugRay) {
            Debug.DrawRay(ray.origin, ray.direction * _iPlaceDistance, Color.blue, 1f);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, _iPlaceDistance, _iPlacementLayerMask)) {
            // Calculate placement position with offset from surface
            Vector3 placePosition = hit.point + hit.normal * _iPlaceOffset;

            // Calculate rotation to align with surface normal
            Quaternion placeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Place the item
            PlaceItem(placePosition, placeRotation);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Pick up an item and arc it to the player's hand
    /// </summary>
    public async Task<bool> PickupItem(GameObject i_item) {
        if (_currentHeldItem != null) {
            Debug.LogWarning("Already holding an item!");
            return false;
        }

        if (i_item == null || _iPickupTransform == null) {
            Debug.LogWarning("Item or hand transform is null!");
            return false;
        }

        // Cancel any existing pickup
        _pickupCancellation?.Cancel();
        _pickupCancellation?.Dispose();
        _pickupCancellation = new CancellationTokenSource();

        // Disable physics on the item during pickup
        Rigidbody rb = i_item.GetComponent<Rigidbody>();
        Collider itemCollider = i_item.GetComponent<Collider>();

        if (rb != null) {
            rb.isKinematic = true;
        }

        if (itemCollider != null) {
            itemCollider.enabled = false;
        }

        try {
            _iAnimator.Play("PickUp");
            // Perform the arc animation
            await ArcToHand(i_item.transform, _pickupCancellation.Token);

            // Parent to hand
            i_item.transform.SetParent(_iPickupTransform);
            i_item.transform.localPosition = Vector3.zero;
            i_item.transform.localRotation = Quaternion.identity;

            _currentHeldItem = i_item;
            i_item.GetComponent<PickupableItem>().SetItemPickedUp(true);

            if (itemCollider != null) {
                i_item.layer = (int)Mathf.Log(_iInHandLayer.value, 2);
                itemCollider.enabled = true;
            }

            return true;
        } catch (OperationCanceledException) {
            // Pickup was cancelled, re-enable physics
            if (rb != null && i_item != null) {
                rb.isKinematic = false;
            }
            if (itemCollider != null && i_item != null) {
                itemCollider.enabled = true;
            }
            return false;
        }
    }

    /// <summary>
    /// Drop the currently held item with physics
    /// </summary>
    public void DropItem() {
        if (_currentHeldItem == null) {
            return;
        }

        // Cancel any ongoing pickup
        _pickupCancellation?.Cancel();

        // Unparent from hand
        _currentHeldItem.transform.SetParent(null);

        // Re-enable physics
        Rigidbody rb = _currentHeldItem.GetComponent<Rigidbody>();
        Collider itemCollider = _currentHeldItem.GetComponent<Collider>();

        if (itemCollider != null) {
            itemCollider.enabled = true;
            _currentHeldItem.layer = (int)Mathf.Log(_iInteractableLayer.value, 2);
        }

        if (rb != null) {
            rb.isKinematic = false;

            // Add some force to throw it forward
            Vector3 dropDirection = _cameraTransform.forward * _iDropForwardForce + Vector3.up * _iDropUpwardForce;
            rb.linearVelocity = dropDirection;

            // Inherit player's velocity if moving
            CharacterController playerController = GetComponent<CharacterController>();
            if (playerController != null) {
                rb.linearVelocity += playerController.velocity;
            }
        }

        _currentHeldItem.GetComponent<PickupableItem>().SetItemPickedUp(false);
        _currentHeldItem = null;
        
    }

    /// <summary>
    /// Place item at a specific position without physics
    /// </summary>
    public void PlaceItem(Vector3 i_position, Quaternion i_rotation) {
        if (_currentHeldItem == null) {
            return;
        }

        _pickupCancellation?.Cancel();

        _currentHeldItem.transform.SetParent(null);
        _currentHeldItem.transform.position = i_position;
        _currentHeldItem.transform.rotation = i_rotation;

        Rigidbody rb = _currentHeldItem.GetComponent<Rigidbody>();
        Collider itemCollider = _currentHeldItem.GetComponent<Collider>();

        if (itemCollider != null) {
            itemCollider.enabled = true;
            _currentHeldItem.layer = (int)Mathf.Log(_iInteractableLayer.value, 2);
        }

        if (rb != null) {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        _currentHeldItem.GetComponent<PickupableItem>().SetItemPickedUp(false);
        _currentHeldItem = null;
    }

    private async Task ArcToHand(Transform i_itemTransform, CancellationToken i_cancellationToken) {
        Vector3 startPosition = i_itemTransform.position;
        Quaternion startRotation = i_itemTransform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < _iPickupDuration) {
            i_cancellationToken.ThrowIfCancellationRequested();

            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / _iPickupDuration;
            float curveValue = _iPickupCurve.Evaluate(normalizedTime);

            // Calculate target position (hand moves, so we need to update it each frame)
            Vector3 targetPosition = _iPickupTransform.position;

            // Linear interpolation between start and target
            Vector3 linearPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);

            // Add arc (parabolic curve)
            float arcProgress = 1f - Mathf.Pow(2f * normalizedTime - 1f, 2f); // Parabola that peaks at 0.5
            Vector3 arcOffset = Vector3.up * (_iArcHeight * arcProgress);

            // Final position with arc
            i_itemTransform.position = linearPosition + arcOffset;

            // Rotate towards hand
            i_itemTransform.rotation = Quaternion.Slerp(startRotation, _iPickupTransform.rotation, curveValue);

            await Task.Yield();
        }

        // Ensure we end exactly at the hand position
        i_itemTransform.position = _iPickupTransform.position;
        i_itemTransform.rotation = _iPickupTransform.rotation;
    }

    /// <summary>
    /// Force drop without physics (e.g., for emergency situations)
    /// </summary>
    public void ForceDropItem() {
        if (_currentHeldItem == null) {
            return;
        }

        _pickupCancellation?.Cancel();

        _currentHeldItem.transform.SetParent(null);

        Rigidbody rb = _currentHeldItem.GetComponent<Rigidbody>();
        Collider itemCollider = _currentHeldItem.GetComponent<Collider>();

        if (itemCollider != null) {
            itemCollider.enabled = true;
            _currentHeldItem.layer = (int)Mathf.Log(_iInteractableLayer.value, 2);
        }

        if (rb != null) {
            rb.isKinematic = false;
        }

        _currentHeldItem.GetComponent<PickupableItem>().SetItemPickedUp(false);
        _currentHeldItem = null;
    }
}