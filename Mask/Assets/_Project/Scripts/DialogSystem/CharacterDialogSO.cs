using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDialogSODialogTreeSO", menuName = "Scriptable Objects/CharacterDialogSO")]
public class CharacterDialogSO : ScriptableObject
{
    public List<string> DialogText;

    public CharacterDialogSO nextDialog;
}
