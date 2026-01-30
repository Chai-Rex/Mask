using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public struct StateVariable
{
    [SerializeField]
    private string name;
    public string Name { get { return name; } }

    [SerializeField]
    private bool _value;

    public StateVariable(string i_name, bool i_value, bool shouldRegister = true)
    {
        name = i_name;
        _value = i_value;

        if(shouldRegister)
        {
            StoryStateSO.RegisterInitialState(this);
        }
        
    }

    public bool Value 
    {  
        get { return _value; } 
        set { _value = value; }
    }

    public void SetValueAndUpdateBlackboard(bool i_value)
    {
        _value = i_value;
        StoryStateSO.Instance.SetValue(this);
    }
}

[CreateAssetMenu(fileName = "StoryStateSO", menuName = "Scriptable Objects/StoryStateSO")]
public class StoryStateSO : ScriptableObject
{
    private static StoryStateSO _instance;
    public static StoryStateSO Instance { get { return _instance; } }

    private static Dictionary<string, bool> RegisteredInitialStates = new Dictionary<string, bool>();

    public delegate void StateCallback(bool i_newValue);
    public static void RegisterInitialState(StateVariable i_variable)
    {
        if (RegisteredInitialStates.ContainsKey(i_variable.Name))
        {
            RegisteredInitialStates[i_variable.Name] = i_variable.Value;
        }
        else
        {
            RegisteredInitialStates.Add(i_variable.Name, i_variable.Value);
        }
    }

    [SerializeField]
    private List<StateVariable> _initialStateVariables;

    public Dictionary<string, bool> pairs = new Dictionary<string, bool>();
    private Dictionary<string, List<StateCallback>> functionCallbacks = new Dictionary<string, List<StateCallback>>();

    private void OnEnable()
    {
        _instance = this;

    }

    // Call this on game start to register all our state variables into the blackboard
    // Also helps see what variables are available
    public void LoadRegisteredStates()
    {
        _initialStateVariables.Clear();
        foreach (string state in RegisteredInitialStates.Keys)
        {
            // Sync the initial state variable list with our registered states at game start
            _initialStateVariables.Add(new StateVariable(state, RegisteredInitialStates[state], false));
            SetValue(state, RegisteredInitialStates[state]);
        }
    }

    public void RegisterCallback(string i_state, StateCallback i_callbackFunction)
    {
        if (!functionCallbacks.ContainsKey(i_state))
        {
            functionCallbacks.Add(i_state, new List<StateCallback>());
        }

        functionCallbacks[i_state].Add(i_callbackFunction);
    }

    public void ResetState()
    {
        pairs.Clear();

        foreach(StateVariable variable in _initialStateVariables)
        {
            SetValue(variable.Name, variable.Value);
        }
    }
    public void SetValue(StateVariable i_variable)
    {
        SetValue(i_variable.Name, i_variable.Value);
    }

    public void SetValue(string name, bool value)
    {
        if (pairs.ContainsKey(name))
        {
            pairs[name] = value;
        }
        else
        {
            pairs.Add(name, value);
        }

        if (functionCallbacks.ContainsKey(name))
        {
            foreach (StateCallback callback in functionCallbacks[name])
            {
                callback(value);
            }
        }

        //Debug.Log("Setting " + name + " to " +  value);
    }

    public bool GetValue(string name)
    {
        if(pairs.ContainsKey(name))
        {
            return pairs[name];
        }

        return false;
    }
}

