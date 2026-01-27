using TMPro;
using UnityEngine;

public class HUDCanvas : MonoBehaviour {

    [Header("References")]
    [SerializeField] private InteractionHandler _iInteractionHandler;
    [SerializeField] private GameObject _iInteractionPrompt;
    [SerializeField] private TextMeshProUGUI _iVerbText;

    [Header("Settings")]
    [SerializeField] private string _iKeyPrompt = "[E] ";

    private void Start() {
        if (_iInteractionHandler == null) {
            _iInteractionHandler = FindFirstObjectByType<InteractionHandler>();
        }

        // Subscribe to interaction events
        _iInteractionHandler._OnInteractableFound.AddListener(ShowInteractionPrompt);
        _iInteractionHandler._OnInteractableLost.AddListener(HideInteractionPrompt);

        // Hide prompt initially
        HideInteractionPrompt();
    }

    private void OnDestroy() {
        if (_iInteractionHandler != null) {
            _iInteractionHandler._OnInteractableFound.RemoveListener(ShowInteractionPrompt);
            _iInteractionHandler._OnInteractableLost.RemoveListener(HideInteractionPrompt);
        }
    }

    private void ShowInteractionPrompt(string verb) {
        _iInteractionPrompt.SetActive(true);

        if (_iVerbText != null) {
            _iVerbText.text = _iKeyPrompt + verb;
        }
    }

    private void HideInteractionPrompt() {
        _iInteractionPrompt.SetActive(false);
    }


}
