using System.Collections;
using UnityEngine;

public class BathHeat : BaseTimeEvent
{
    private bool isPlayerIn = false;
    private bool isActive = false;
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

        // Player Death

    }

    public void SetBathHeatIsActive(bool _isActive)
    {
        isActive = _isActive;

        if (isActive)
        {
            if (isPlayerIn)
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
