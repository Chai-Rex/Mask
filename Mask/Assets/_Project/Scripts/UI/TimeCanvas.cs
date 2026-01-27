using TMPro;
using UnityEngine;

public class TimeCanvas : MonoBehaviour {

    [SerializeField] private TMP_Text _iText;


    public void SetTime(float i_seconds) {
        // change how the time is displayed?
        _iText.text = Mathf.RoundToInt(i_seconds).ToString();
    }


}
