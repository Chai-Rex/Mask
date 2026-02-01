using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour {

    [SerializeField] private ResponseButton _iResponseButton;
    [SerializeField] private GameObject _iResponseOptions;
    [SerializeField] private int _iMaxResponseOptions = 10;

    [SerializeField] private TMP_Text _iDialogueText; 
    [SerializeField] private TMP_Text _iNameText;


    private ResponseButton[] _responseButtons;

    private void Awake() {
        _responseButtons = new ResponseButton[_iMaxResponseOptions];
        for (int i = 0; i < _iMaxResponseOptions; i++) {
            _responseButtons[i] = Instantiate(_iResponseButton, _iResponseOptions.transform);
            _responseButtons[i].gameObject.SetActive(false);
        }
    }

    public void SetDialogue(string i_dialogue) {
        _iDialogueText.text = i_dialogue;
    }

    public void AddDialogueCharacter(char i_dialogue) {
        _iDialogueText.text += i_dialogue;
    }

    public void SetName(string i_name) {
        _iNameText.text = i_name;
    }

    public void AddResponse(string i_response, int i_id) {
        _responseButtons[i_id].gameObject.SetActive(true);
        _responseButtons[i_id].SetText(i_response, i_id);
    }

    public void ClearResponses() {
        foreach (var response in _responseButtons) {
            response.gameObject.SetActive(false);
        }
    }

}
