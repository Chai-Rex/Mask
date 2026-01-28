using Unity.VisualScripting;
using UnityEngine;

public class InteractableTalk : MonoBehaviour, IInteractable {


    [SerializeField] private string _iVerb = "Talk";
    [SerializeField] private string _iName = "Boberto";

    // replace with solver
    [SerializeField] private CharacterDialogSO _iTempDialogSO; 
    [SerializeField] private CharacterDialogSO _iCompleteDialogue;
    private bool _isDialogueComplete = false;

    public string InteractionVerb => _iVerb;

    public void OnInteract(GameObject i_interactor) {
        //Debug.Log($"My name is: {_iName}");

        if (_isDialogueComplete) {
            i_interactor.GetComponent<DialogueHandler>().StartDialogueTree(_iCompleteDialogue, _iName);
        } else {
            // get dialog tree from gabe's bool solver
            i_interactor.GetComponent<DialogueHandler>().StartDialogueTree(_iTempDialogSO, _iName);
            _isDialogueComplete = true;
        }

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
