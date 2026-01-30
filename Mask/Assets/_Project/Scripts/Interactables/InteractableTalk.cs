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

    private CharacterDialogSO _dialogueTree;

    public string InteractionVerb => _iVerb;

    public void OnInteract(GameObject i_interactor) {

        DialogueHandler handler = i_interactor.GetComponent<DialogueHandler>();

        _dialogueTree = _dialogTreeSelector.GetDialogTree();
        handler.StartDialogueTree(_dialogueTree, _iName, _iSoundData, _iDialogueAnimationHandler);
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


}
