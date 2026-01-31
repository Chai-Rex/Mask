using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerDecision
{
    public string text;
    public CharacterDialogSO nextDialog;
    public bool affectsState;
    public string stateVariable;
    public bool stateValue;
}

[CreateAssetMenu(fileName = "CharacterDialogSODialogTreeSO", menuName = "Scriptable Objects/CharacterDialogSO")]
public class CharacterDialogSO : ScriptableObject
{
    
    public List<string> DialogText = new List<string>();

    public List<PlayerDecision> decisionOptions = new List<PlayerDecision>();

    private void OnEnable()
    {
        // Register potential decision states
        foreach (PlayerDecision decision in decisionOptions)
        { 
            if(decision.affectsState)
            {
                StoryStateSO.RegisterInitialState(new StateVariable(decision.stateVariable, false, false));
            }
        }

        
    }

}
