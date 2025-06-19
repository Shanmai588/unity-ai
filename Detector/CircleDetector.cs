using UnityEngine;

public class CircleDetector : BaseDetector
{
    private CircleCollider2D trigger;

    protected override void Awake()
    {
        base.Awake();
        trigger = GetComponent<CircleCollider2D>();
        trigger.isTrigger = true;
        UpdateTriggerRadius();
    }

    private void OnValidate()
    {
        UpdateTriggerRadius();
    }

    protected override void CollectHits()
    {
        var hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            range,
            hitsBuffer,
            targetLayers
        );
        ProcessHits(hitCount);
    }

    protected override void OnRangeChanged()
    {
        UpdateTriggerRadius();
    }

    private void UpdateTriggerRadius()
    {
        if (trigger != null)
            trigger.radius = range;
    }
}