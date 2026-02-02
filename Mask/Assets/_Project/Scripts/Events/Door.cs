using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    [SerializeField] private bool isDoorLocked = false;

    Tween doorTween;
    [SerializeField] private float doorDuration = 0.75f;
    [SerializeField] private BoxCollider betweenDoorCollider;

    [SerializeField] private bool opensOnStateChange = false;
    [SerializeField] private string stateName;

    private Quaternion closedRotation;
    [SerializeField] private float openAngle = 90.0f;

    private void Awake()
    {
        closedRotation = transform.localRotation;
    }

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
        if (isDoorLocked || isOpen) { return; }

        doorTween?.Kill();

        float swingDirection = isFront ? -1.0f : 1.0f;

        if (isFront)
        {
            if (betweenDoorCollider != null)
            {
                betweenDoorCollider.center = new Vector3(-0.56f, 1.0f, 1.55f);
            }
        }
        else
        {
            if (betweenDoorCollider != null)
            {
                betweenDoorCollider.center = new Vector3(-0.56f, 1.0f, -1.55f);
            }
        }
        
        Quaternion target = closedRotation * Quaternion.Euler(0.0f, swingDirection * openAngle, 0.0f);

        doorTween = transform.DOLocalRotateQuaternion(target, doorDuration)
            .OnComplete(() =>
            {
                isOpen = true;
            });
    }
    public void OnDoorClose()
    {
        doorTween.Kill();

        doorTween = transform.DOLocalRotateQuaternion(closedRotation, doorDuration)
            .OnComplete(() =>
            {
                isOpen = false;
            });
    }

    public void SetIsDoorLocked(bool _isDoorLocked)
    {
        isDoorLocked = _isDoorLocked;
    }
}
