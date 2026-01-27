using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    public Dictionary<string, int> pairs = new Dictionary<string, int>();

    public void ResetState()
    {
        foreach(StateVariable variable in stateVariables)
        {
            SetValue(variable.name, variable.initialValue);
        }
    }

    public void SetValue(string name, int value)
    {
        if (pairs.ContainsKey(name))
        {
            pairs[name] = value;
        }
        else
        {
            pairs.Add(name, value);
        }

        Debug.Log("Setting " + name + " to " +  value);
    }

    public int GetValue(string name)
    {
        if(pairs.ContainsKey(name))
        {
            return pairs[name];
        }

        return 0;
    }
}

