using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class CreditsCanvas : MonoBehaviour {
    [Header("Level Loading")]
    [SerializeField] private LevelManager.Levels levelToLoad;

    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;

    [SerializeField] private float scrollSpeed = 50f; // pixels per second

    [SerializeField] private TMP_Text _iDeathsText;

    private float maxScrollY;
    private bool isScrolling;

    private CancellationTokenSource _cts;

    private void Awake() {
        _cts = new CancellationTokenSource();
    }

    private void Start() {
        StoryStateSO.Instance.RegisterCallback("guessedCorrect", TriggerGameEnd);
    }

    private void TriggerGameEnd(bool value)
    {
        if(value)
        {
            StartCredits();
        }
    }

    private void OnDestroy() {
        // Cancel any running async work
        _cts.Cancel();
        _cts.Dispose();

        if (InputManager.Instance)
            InputManager.Instance._DialogueContinueAction.started -=
                _DialogueContinueAction_started;
    }

    public async void StartCredits() {
        CancellationToken token = _cts.Token;

        try {
            InputManager.Instance.DisablePlayerActions();

            _iDeathsText.text = $"Deaths {StoryStateSO.Instance._numDeaths}";

            await Awaitable.WaitForSecondsAsync(2f, token);

            InputManager.Instance.EnableDialogueActions();
            InputManager.Instance._DialogueContinueAction.started +=
                _DialogueContinueAction_started;

            // content height minus viewport height
            maxScrollY = Mathf.Max(
                0f,
                content.rect.height - viewport.rect.height
            );

            isScrolling = true;

            while (isScrolling && !token.IsCancellationRequested) {
                Vector2 pos = content.anchoredPosition;
                pos.y += scrollSpeed * Time.deltaTime;

                if (pos.y >= maxScrollY) {
                    pos.y = maxScrollY;
                    isScrolling = false;
                }

                content.anchoredPosition = pos;

                await Awaitable.NextFrameAsync(token);
            }
        } catch (OperationCanceledException) {
            // Expected on destroy — swallow safely
        }
    }

    private void _DialogueContinueAction_started(
        UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        LevelManager.Instance.LoadScene(levelToLoad);
    }
}
