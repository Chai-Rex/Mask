using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogTesterScript : MonoBehaviour
{
    public StoryStateSO exampleState;
    public CharacterDialogSO currentDialog;

    private string dialogText = string.Empty;
    private string displayedText = string.Empty;
    private bool finishedDisplayingText = true;
    public float textSpeed = 2f;

    private float timeBetweenChars = 0;
    private float timer = 0;

    private bool displayingPlayerOptions = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance._PlayerInteractAction.performed += InteractPressed;

        timeBetweenChars = 1f / textSpeed;
        currentDialog.ResetDialog();
    }

    // Update is called once per frame
    void Update()
    {
        // Displays the text 1 character at a time based on text speed
        if(!finishedDisplayingText)
        {
            timer += Time.deltaTime;

            if(timer >= timeBetweenChars)
            {
                timer = 0;
                displayedText += dialogText[displayedText.Length];
                Debug.Log(displayedText);

                if (displayedText.Length == dialogText.Length)
                {
                    finishedDisplayingText = true;
                }
            }
            
        }
    }

    private void InteractPressed(InputAction.CallbackContext context)
    {
        if (finishedDisplayingText)
        {
            if(!currentDialog.IsDialogFinished())
            {
                DisplayNextDialog();
            }
            else
            {
                if(!displayingPlayerOptions)
                {
                    DisplayPlayerChoice();
                }
                else
                {
                    ChooseDialogOption(0);
                }
            }
        }
        else
        {
            displayedText = dialogText;
            finishedDisplayingText = true;
            Debug.Log(displayedText);
        }
    }

    private void DisplayNextDialog()
    {
        displayedText = string.Empty;
        dialogText = currentDialog.GetDialogText();

        if (dialogText != string.Empty)
        {
            finishedDisplayingText = false;
        }
    }

    private void DisplayPlayerChoice()
    {
        displayingPlayerOptions = true;
        dialogText = string.Empty;
        displayedText = string.Empty;
        foreach (PlayerDecision decision in currentDialog.decisionOptions)
        {
            displayedText += decision.text + '\n';
        }

        Debug.Log(displayedText);
    }

    private void ChooseDialogOption(int choice)
    {
        if (choice >= 0 && choice < currentDialog.decisionOptions.Count)
        {
            PlayerDecision chosenDecision = currentDialog.decisionOptions[choice];
            currentDialog = chosenDecision.nextDialog;
            currentDialog.ResetDialog(); // Make sure we start from the beginning of the dialog

            if (chosenDecision.affectsState)
            {
                StoryStateSO playerState = exampleState;
                playerState.SetValue(chosenDecision.stateVariable, chosenDecision.stateValue);
            }
            
        }

        displayingPlayerOptions = false;

        DisplayNextDialog();
    }
}
