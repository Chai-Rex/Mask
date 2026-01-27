using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = .2f;
    [SerializeField] private float gamepadSensitivity = 100f;
    [SerializeField] private float lookXLimit = 80f;
    [SerializeField] private bool invertY = false;

    [Header("Crouch Settings")]
    [SerializeField] private bool enableCrouch = true;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 10f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float groundStickForce = 5f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private float rotationX = 0f;
    private float currentHeight;
    private bool isCrouching = false;
    private bool canStandUp = true;

    // Input values
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool sprintInput;

    private string currentControlScheme = "Keyboard&Mouse";

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        currentHeight = standingHeight;

        if (cameraTransform == null) {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Start() {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial height
        characterController.height = standingHeight;
        characterController.center = new Vector3(0, standingHeight / 2f, 0);

        // Subscribe to input events
        SubscribeToInput();

        // Subscribe to control scheme changes
        InputManager.Instance._OnControlsChanged += OnControlsChanged;
    }

    private void OnDestroy() {
        // Unsubscribe from input events
        UnsubscribeFromInput();

        if (InputManager.Instance != null) {
            InputManager.Instance._OnControlsChanged -= OnControlsChanged;
        }
    }

    private void SubscribeToInput() {
        if (InputManager.Instance == null) return;

        InputManager.Instance._CrouchAction.started += OnCrouchToggled;
        InputManager.Instance._CancelAction.started += OnCancelStarted;
    }

    private void UnsubscribeFromInput() {
        if (InputManager.Instance == null) return;

        InputManager.Instance._CrouchAction.started -= OnCrouchToggled;
        InputManager.Instance._CancelAction.started -= OnCancelStarted;
    }

    private void OnControlsChanged(string controlScheme) {
        currentControlScheme = controlScheme;
    }

    private void OnCrouchToggled(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        if (!enableCrouch) return;

        if (isCrouching) {
            // Try to stand up
            TryStandUp();
        } else {
            // Crouch down
            isCrouching = true;
        }
    }

    private void TryStandUp() {
        // Check if we can stand up (no ceiling above)
        float checkDistance = standingHeight - crouchHeight + 0.1f;
        Vector3 checkOrigin = transform.position + Vector3.up * crouchHeight;

        if (!Physics.SphereCast(checkOrigin, characterController.radius, Vector3.up,
            out RaycastHit hit, checkDistance, ~0, QueryTriggerInteraction.Ignore)) {
            isCrouching = false;
        }
    }

    private void OnCancelStarted(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        // Toggle cursor lock
        if (Cursor.lockState == CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update() {
        if (InputManager.Instance == null) return;

        HandleInput();
        HandleMouseLook();
        HandleCrouch();
        HandleMovement();
    }

    private void HandleInput() {
        // Movement input
        moveInput = InputManager.Instance._MoveAction.ReadValue<Vector2>();

        // Look input
        lookInput = InputManager.Instance._LookAction.ReadValue<Vector2>();

        // Sprint input
        sprintInput = InputManager.Instance._SprintAction.IsPressed();
    }

    private void HandleMouseLook() {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        // Determine sensitivity based on control scheme
        float currentSensitivity = mouseSensitivity;
        float lookMultiplier = 1f;

        if (currentControlScheme == "Gamepad") {
            currentSensitivity = gamepadSensitivity;
            lookMultiplier = Time.deltaTime; // Gamepad needs frame-rate independence
        }

        // Horizontal rotation (Y-axis) - rotate the player body
        transform.Rotate(Vector3.up * lookInput.x * currentSensitivity * lookMultiplier);

        // Vertical rotation (X-axis) - rotate the camera
        rotationX += lookInput.y * currentSensitivity * lookMultiplier * (invertY ? 1f : -1f);
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void HandleCrouch() {
        if (!enableCrouch)
            return;

        // Smoothly interpolate height
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        characterController.height = currentHeight;
        characterController.center = new Vector3(0, currentHeight / 2f, 0);

        // Adjust camera position
        Vector3 cameraLocalPos = cameraTransform.localPosition;
        cameraLocalPos.y = currentHeight - 0.2f;
        cameraTransform.localPosition = cameraLocalPos;
    }

    private void HandleMovement() {
        // Determine current speed based on state
        float targetSpeed = walkSpeed;

        if (isCrouching) {
            targetSpeed = crouchSpeed;
        } else if (sprintInput && moveInput.y > 0) // Only sprint when moving forward
          {
            targetSpeed = sprintSpeed;
        }

        // Calculate movement direction relative to where player is looking
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Project onto horizontal plane to prevent flying up slopes
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 targetVelocity = (forward * moveInput.y + right * moveInput.x) * targetSpeed;

        // Smoothly accelerate/decelerate
        float currentSpeed = new Vector3(currentVelocity.x, 0, currentVelocity.z).magnitude;
        float targetSpeedMagnitude = targetVelocity.magnitude;

        if (targetSpeedMagnitude > currentSpeed) {
            // Accelerating
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetVelocity.x, acceleration * Time.deltaTime);
            currentVelocity.z = Mathf.Lerp(currentVelocity.z, targetVelocity.z, acceleration * Time.deltaTime);
        } else {
            // Decelerating
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetVelocity.x, deceleration * Time.deltaTime);
            currentVelocity.z = Mathf.Lerp(currentVelocity.z, targetVelocity.z, deceleration * Time.deltaTime);
        }

        // Apply gravity
        if (characterController.isGrounded) {
            // Apply ground stick force to help stay on slopes/ramps
            currentVelocity.y = -groundStickForce;
        } else {
            currentVelocity.y -= gravity * Time.deltaTime;
        }

        // Move the character
        characterController.Move(currentVelocity * Time.deltaTime);
    }

    // Public methods for external control
    public void SetMouseSensitivity(float sensitivity) {
        mouseSensitivity = sensitivity;
    }

    public void SetGamepadSensitivity(float sensitivity) {
        gamepadSensitivity = sensitivity;
    }

    public void SetWalkSpeed(float speed) {
        walkSpeed = speed;
    }

    public void EnableMovement(bool enable) {
        enabled = enable;
    }

    public void LockCursor(bool lockCursor) {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    public bool IsGrounded => characterController.isGrounded;
    public bool IsCrouching => isCrouching;
    public float CurrentSpeed => new Vector3(currentVelocity.x, 0, currentVelocity.z).magnitude;
    public Vector3 Velocity => currentVelocity;
}