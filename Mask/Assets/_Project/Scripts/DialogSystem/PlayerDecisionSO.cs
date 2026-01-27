using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDecisionSO", menuName = "Scriptable Objects/PlayerDecisionSO")]
public class PlayerDecisionSO : ScriptableObject
{
    [System.Serializable]
    public struct PlayerDecision
    {
        public string text;
        public CharacterDialogSO resultDialog;
        public bool affectsState;
        public string stateVariable;
        public int stateValue;
    }

    public List<PlayerDecision> decisionOptions;

}
