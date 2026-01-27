using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StateVariable
{
    public string name;
    public int initialValue;
}

[CreateAssetMenu(fileName = "StoryStateSO", menuName = "Scriptable Objects/StoryStateSO")]
public class StoryStateSO : ScriptableObject
{

    public List<StateVariable> stateVariables;

    public Dictionary<string, int> pairs;
}
