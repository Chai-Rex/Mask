using System.Threading.Tasks;
using UnityEngine;

public class InteractableTalk : MonoBehaviour, IInteractable {

    [Header("Interaction Settings")]
    [SerializeField] private string _iVerb = "Talk";

    [Header("Dialogue Settings")]
    [SerializeField] private string _iName = "Boberto";
    [SerializeField] private DialogTreeSelector _dialogTreeSelector;
    [SerializeField] private DialogueAnimationHandler _iDialogueAnimationHandler;

    [Header("Sound Settings")]
    [SerializeField] private DialogSoundSO _iSoundData;

    [Header("Rotation Curves")]
    [SerializeField] private float turnDurationSeconds = 0.5f;
    [SerializeField] private AnimationCurve turnCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve returnCurve = AnimationCurve.EaseInOut(1, 1, 0, 0);

    private CharacterDialogSO _dialogueTree;
    private float _startingYaw;


    public string InteractionVerb => _iVerb;

    public void OnInteract(GameObject i_interactor) {

        DialogueHandler handler = i_interactor.GetComponent<DialogueHandler>();

        _dialogueTree = _dialogTreeSelector.GetDialogTree();
        handler.StartDialogueTree(this, _dialogueTree, _iName, _iSoundData, _iDialogueAnimationHandler);
    }

    public void OnLookEnter(GameObject i_looker) {
        // turn towards player?
    }

    public void OnLookExit(GameObject i_looker) {
        // turn back 
    }

    public Transform GetTransform() {
        return transform;
    }

    /// <summary>
    /// Smoothly rotates this GameObject to face the target (Y axis only).
    /// Awaitable.
    /// </summary>
    public async Task RotateTowardsAsync( Transform target) {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        float startYaw = transform.eulerAngles.y;
        float targetYaw = Quaternion.LookRotation(direction).eulerAngles.y;

        float elapsed = 0f;

        while (elapsed < turnDurationSeconds) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / turnDurationSeconds);
            float curveT = turnCurve.Evaluate(t);

            float yaw = Mathf.LerpAngle(startYaw, targetYaw, curveT);
            transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);

            await Task.Yield();
        }

        transform.rotation = Quaternion.AngleAxis(targetYaw, Vector3.up);
    }

    /// <summary>
    /// Smoothly rotates this GameObject back to its starting yaw (Y axis only).
    /// Awaitable.
    /// </summary>
    public async void RotateBackToStartAsync() {
        float startYaw = transform.eulerAngles.y;
        float targetYaw = _startingYaw;

        float elapsed = 0f;

        while (elapsed < turnDurationSeconds) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / turnDurationSeconds);
            float curveT = returnCurve.Evaluate(t);

            float yaw = Mathf.LerpAngle(startYaw, targetYaw, curveT);
            transform.rotation = Quaternion.AngleAxis(yaw, Vector3.up);

            await Task.Yield();
        }

        transform.rotation = Quaternion.AngleAxis(targetYaw, Vector3.up);
    }

}
