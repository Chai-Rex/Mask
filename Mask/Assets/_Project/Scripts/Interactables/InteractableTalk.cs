using Unity.VisualScripting;
using UnityEngine;

public class InteractableTalk : MonoBehaviour, IInteractable {


    [SerializeField] private string _iVerb = "Talk";
    [SerializeField] private string _iName = "Boberto";

    // replace with solver
    [SerializeField] private CharacterDialogSO _iTempDialogSO; 
    [SerializeField] private CharacterDialogSO _iCompleteDialogue;
    private bool _isDialogueComplete = false;

    [SerializeField] private AudioClip _iTypingSound;
    [SerializeField] private float _iTypingSpeed = 0.05f;

    public string InteractionVerb => _iVerb;

    public void OnInteract(GameObject i_interactor) {
        //Debug.Log($"My name is: {_iName}");

        DialogueHandler handler = i_interactor.GetComponent<DialogueHandler>();
        handler.SetCharacterSpeechSettings(_iTypingSound, _iTypingSpeed);
        if (_isDialogueComplete) {
            handler.StartDialogueTree(_iCompleteDialogue, _iName);
        } else {
            // get dialog tree from gabe's bool solver
            handler.StartDialogueTree(_iTempDialogSO, _iName);
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
