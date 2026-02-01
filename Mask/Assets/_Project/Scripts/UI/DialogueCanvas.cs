using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvas : MonoBehaviour {

    [SerializeField] private ResponseButton _iResponseButton;
    [SerializeField] private GameObject _iResponseOptions;
    [SerializeField] private int _iMaxResponseOptions = 10;

    [SerializeField] private TMP_Text _iDialogueText; 
    [SerializeField] private TMP_Text _iNameText;

    [SerializeField] private RectTransform _iRebuildTransform;

    private ResponseButton[] _responseButtons;

    private bool _refresh;

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
        WaitFrameBeforeCanvasRefresh();
    }

    public void ClearResponses() {
        foreach (var response in _responseButtons) {
            response.gameObject.SetActive(false);
        }
    }

    private async void WaitFrameBeforeCanvasRefresh() {
        await Awaitable.NextFrameAsync();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_iRebuildTransform);
    }

}
