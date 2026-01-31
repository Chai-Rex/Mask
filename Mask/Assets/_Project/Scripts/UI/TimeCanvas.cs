using TMPro;
using UnityEngine;

public class TimeCanvas : MonoBehaviour {
    [SerializeField] private TMP_Text _iText;

    [Tooltip("How many real-life seconds equal ONE in-game minute")]
    [SerializeField] private float secondsPerGameMinute = 1f;

    /// <summary>
    /// Sets the displayed in-game time based on real seconds elapsed.
    /// </summary>
    public void SetTime(float realSecondsElapsed) {
        // Convert real seconds to total in-game minutes
        float totalGameMinutes = realSecondsElapsed / secondsPerGameMinute;

        int hours = Mathf.FloorToInt(totalGameMinutes / 60f);
        int minutes = Mathf.FloorToInt(totalGameMinutes % 60f);

        _iText.text = $"{hours:00}:{minutes:00}";
    }
}
