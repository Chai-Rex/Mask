using System.Collections.Generic;
using UnityEngine;

public class DialogTreeSelector : MonoBehaviour
{
    [System.Serializable]
    public struct DialogTreeSelection
    {
        public List<StateVariable> requiredStates;
        public CharacterDialogSO dialogTree;
    }

    [SerializeField] private StoryStateSO _StoryState;

    [SerializeField] private List<DialogTreeSelection> _treeOptions;

    [SerializeField] private CharacterDialogSO _defaultDialogTree;
    [SerializeField] private CharacterDialogSO _iCompleteDialogue;

    private List<CharacterDialogSO> visitedDialogs = new List<CharacterDialogSO>();

    public CharacterDialogSO GetDialogTree()
    {
        foreach (DialogTreeSelection treeSelection in _treeOptions)
        {
            bool stateMatches = true;
            foreach (StateVariable variable in treeSelection.requiredStates)
            {
                if(_StoryState.GetValue(variable.Name) != variable.Value)
                {
                    stateMatches = false;
                    break;
                }
            }

            if(stateMatches)
            {
                if (visitedDialogs.Contains(treeSelection.dialogTree))
                {
                    return _defaultDialogTree;
                }
                else
                {
                    SetDialogVisited(treeSelection.dialogTree);
                    return treeSelection.dialogTree;
                }
            }
        }

        return _defaultDialogTree;
    }

    public void SetDialogVisited(CharacterDialogSO dialog)
    {
        visitedDialogs.Add(dialog);
    }
}
