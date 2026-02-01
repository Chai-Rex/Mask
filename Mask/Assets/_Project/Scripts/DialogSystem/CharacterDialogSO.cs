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
    public List<StateVariable> conditionalStates;
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
            foreach(StateVariable var in conditional.conditionalStates)
            {
                StoryStateSO.RegisterInitialState(new StateVariable(var.Name, false, false));
            }
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
            bool stateMatches = true;
            foreach (StateVariable variable in conditional.conditionalStates)
            {
                if (StoryStateSO.Instance.GetValue(variable.Name) != variable.Value)
                {
                    stateMatches = false;
                    break;
                }
            }

            if (stateMatches)
            {
                activeConditionals.Add(conditional.decision);
            }
        }

        return activeConditionals;
    }

}
