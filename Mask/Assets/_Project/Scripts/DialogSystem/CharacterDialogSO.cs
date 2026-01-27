using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerDecision
{
    public string text;
    public CharacterDialogSO nextDialog;
    public bool affectsState;
    public string stateVariable;
    public int stateValue;
}

[CreateAssetMenu(fileName = "CharacterDialogSODialogTreeSO", menuName = "Scriptable Objects/CharacterDialogSO")]
public class CharacterDialogSO : ScriptableObject
{
    
    public List<string> DialogText;

    public List<PlayerDecision> decisionOptions;

    private int dialogIndex = 0;


    public bool IsDialogFinished() { return dialogIndex >= DialogText.Count; }
    public string GetDialogText()
    {
        if(!IsDialogFinished())
        {
            return DialogText[dialogIndex++]; // Return string and post increment
        }

        return string.Empty;
    }

    public void ResetDialog()
    {
        dialogIndex = 0;
    }
}
