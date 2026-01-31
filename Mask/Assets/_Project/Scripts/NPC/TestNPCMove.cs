using UnityEngine;

public class TestNPCMove : NPCMove {

    [SerializeField] private float[] times;

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        foreach (float t in times) {
            TimeManager.Instance.ScheduleAt(t, ActivateTimeEvent);
        }
    }

    protected override void ActivateTimeEvent() {
        base.ActivateTimeEvent();
    }

}
