using System.Collections;
using UnityEngine;

public class BathHeat : BaseTimeEvent
{
    private StateVariable isPlayerIn = new StateVariable("isPlayerInBath", false);
    private StateVariable isActive = new StateVariable("isBathHeatActive", false);
    [SerializeField] private float heatDuration = 5.0f;

    private Coroutine bathCoroutine;

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        SetBathHeatIsActive(true);
    }

    IEnumerator BathHeating()
    {
        float elapsedHeat = 0.0f;

        while (elapsedHeat < heatDuration)
        {
            elapsedHeat += Time.deltaTime;

            yield return null;
        }

        bathCoroutine = null;

        PlayTriggerSound();
        // Player Death

    }

    public void SetBathHeatIsActive(bool _isActive)
    {
        isActive.SetValueAndUpdateBlackboard(_isActive);

        if (isActive.Value)
        {
            if (isPlayerIn.Value)
            {
                gameObject.SetActive(true);
                bathCoroutine = StartCoroutine(BathHeating());
            }       
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
