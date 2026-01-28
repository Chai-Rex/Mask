using DG.Tweening;
using UnityEngine;

public class InteractableLooseBoard : BaseTimeEvent, IInteractable
{
    private bool isFalling = false;
    private bool hasFallen = false;

    private string verb = "";
    [SerializeField] private string verbWhenFallen = "Fix";

    private Vector3 startLocation;
    [SerializeField] private Vector3 endLocation;

    [SerializeField] private float fallDuration;
    [SerializeField] private float fallDelay = 3.0f;

    private MeshRenderer meshRenderer;

    private bool isPlayerDead = false;

    public string InteractionVerb => hasFallen && !isPlayerDead ? verbWhenFallen : verb;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        startLocation = transform.position;
        endLocation = transform.position + new Vector3(0.0f, -200.0f, 0.0f);
    }

    protected override void ActivateTimeEvent()
    {
        base.ActivateTimeEvent();

        OnLooseBoardFall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFallen) { return; }

        if (other.gameObject.tag == "Player")
        {
            // Death
            isPlayerDead = true;
        }
    }

    private void OnLooseBoardFall()
    {
        hasFallen = true;
        isFalling = true;

        transform.DOMove(endLocation, fallDuration)
            .OnComplete(() =>
            {
                isFalling = false;
                meshRenderer.enabled = false;
                transform.position = startLocation;
            });
    }

    private void OnLooseBoardFix()
    {
        if (!isFalling)
        {
            hasFallen = false;
            meshRenderer.enabled = true;
        }
    }

    public void ActivateLooseBoard()
    {
        TimeManager.Instance.ScheduleAfter(fallDelay, ActivateTimeEvent);
    }

    public void OnInteract(GameObject interactor)
    {
        if (hasFallen)
        {
            OnLooseBoardFix();
        }
    }
}
