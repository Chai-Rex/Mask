using JetBrains.Annotations;
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

[System.Serializable]
public struct ConditionalDecision
{
    public string conditionalState;
    public PlayerDecision decision;
}

[CreateAssetMenu(fileName = "CharacterDialogSODialogTreeSO", menuName = "Scriptable Objects/CharacterDialogSO")]
public class CharacterDialogSO : ScriptableObject
{
    
    public List<string> DialogText = new List<string>();

    public List<PlayerDecision> decisionOptions = new List<PlayerDecision>();

    [SerializeField]
    private List<ConditionalDecision> conditionalDecisions = new List<ConditionalDecision>();

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

        foreach(ConditionalDecision conditional in conditionalDecisions)
        {
            StoryStateSO.RegisterInitialState(new StateVariable(conditional.conditionalState, false, false));
            if (conditional.decision.affectsState)
            {
                StoryStateSO.RegisterInitialState(new StateVariable(conditional.decision.stateVariable, false, false));
            }
        }

    }

    public List<PlayerDecision> GetActiveConditionalDecisions()
    {
        List<PlayerDecision> activeConditionals = new List<PlayerDecision>();

        foreach (ConditionalDecision conditional in conditionalDecisions)
        {
            Debug.Log(conditional.conditionalState + " is " + StoryStateSO.Instance.GetValue(conditional.conditionalState));
            if (StoryStateSO.Instance.GetValue(conditional.conditionalState))
            {
                activeConditionals.Add(conditional.decision);
            }
        }

        return activeConditionals;
    }

}
