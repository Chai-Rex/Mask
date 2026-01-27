using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDecisionSO", menuName = "Scriptable Objects/PlayerDecisionSO")]
public class PlayerDecisionSO : ScriptableObject
{
    public List<string> options;
    public List<CharacterDialogSO> resultDialog;

}
