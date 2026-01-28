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

}
