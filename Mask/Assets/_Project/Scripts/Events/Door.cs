using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;

    Tween doorTween;
    [SerializeField] private float doorDuration = 1.0f;
    [SerializeField] private BoxCollider betweenDoorCollider;

    [SerializeField] private bool opensOnStateChange = false;
    [SerializeField] private string stateName;

    private void Start()
    {
        if(opensOnStateChange)
        {
            StoryStateSO.Instance.RegisterCallback(stateName, SetDoorOnState);
        }
    }

    private void SetDoorOnState(bool i_value)
    {
        if(i_value)
        {
            OnDoorOpen(true);
        }
        else
        {
            OnDoorClose();
        }
    }

    public void OnDoorOpen(bool isFront)
    {
        if (!isOpen)
        {
            doorTween.Kill();

            if (isFront)
            {
                if (betweenDoorCollider != null)
                {
                    betweenDoorCollider.center = new Vector3(0.0f, 0.0f, 2.5f);
                }
                doorTween = transform.DORotate(new Vector3(0.0f, 90.0f, 0.0f), doorDuration)
                .OnComplete(() =>
                {
                    isOpen = true;
                });
            }
            else
            {
                if (betweenDoorCollider != null)
                {
                    betweenDoorCollider.center = new Vector3(0.0f, 0.0f, -2.5f);
                }
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
        doorTween.Kill();

        doorTween = transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), doorDuration)
            .OnComplete(() =>
            {
                isOpen = false;
            });
    }
}
