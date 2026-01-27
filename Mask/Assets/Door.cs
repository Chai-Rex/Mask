using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;

    Tween doorTween;
    [SerializeField] private float doorDuration = 1.0f;

    public void OnDoorOpen(bool isFront)
    {
        if (!isOpen)
        {
            doorTween.Kill();

            if (isFront)
            {
                doorTween = transform.DORotate(new Vector3(0.0f, 90.0f, 0.0f), doorDuration)
                .OnComplete(() =>
                {
                    isOpen = true;
                });
            }
            else
            {
                doorTween = transform.DORotate(new Vector3(0.0f, -90.0f, 0.0f), doorDuration)
                .OnComplete(() =>
                {
                    isOpen = true;
                });
            }      
        }
    }

    public void OnDoorClose()
    {
        if (isOpen)
        {
            doorTween.Kill();

            doorTween = transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), doorDuration)
                .OnComplete(() =>
                {
                    isOpen = false;
                });
        }
    }
}
