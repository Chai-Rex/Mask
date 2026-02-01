using AudioSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ResponseButton : MonoBehaviour {

    [SerializeField] private TMP_Text _iText;
    [SerializeField] private Button _iButton;

    private int _id;

    private void Awake() {
        _iButton.onClick.AddListener(PressButton);
    }

    public void SetText(string i_text, int i_id) {
        _iText.text = i_text;
        _id = i_id;
    }

    private void PressButton() {
        DialogueHandler.Instance.SelectResponse(_id);
    }
}
