using Unity.VisualScripting;
using UnityEngine;

public class InteractableTalk : MonoBehaviour, IInteractable {


    [SerializeField] private string _iVerb = "Talk";
    [SerializeField] private string _iName = "Boberto";

    [SerializeField] private DialogTreeSelector _dialogTreeSelector;

    [Header("Typing Settings")]
    [SerializeField] private AudioClip _iTypingSound;
    [SerializeField] private float _iTypingSpeed = 0.05f;
    [SerializeField] private float _iMinPitchModulation = 1;
    [SerializeField] private float _iMaxPitchModulation = 1;

    public string InteractionVerb => _iVerb;

    public void OnInteract(GameObject i_interactor) {
        //Debug.Log($"My name is: {_iName}");

        DialogueHandler handler = i_interactor.GetComponent<DialogueHandler>();
        handler.SetCharacterSpeechSettings(_iTypingSound, _iTypingSpeed, _iMinPitchModulation, _iMaxPitchModulation);

        CharacterDialogSO dialog = _dialogTreeSelector.GetDialogTree();
        handler.StartDialogueTree(dialog, _iName);
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
