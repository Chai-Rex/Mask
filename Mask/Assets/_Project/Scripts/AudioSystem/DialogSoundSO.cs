using AudioSystem;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogSoundSO", menuName = "Scriptable Objects/DialogSoundSO")]
public class DialogSoundSO : ScriptableObject {

    public SoundData _iTypingSoundData;

    public float _iTypingSpeed = 0.05f;
    public float _iMinPitchModulation = 1;
    public float _iMaxPitchModulation = 1;
    public float _iPunctuationPauseLength = 0.5f;

}
