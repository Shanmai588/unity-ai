using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDetector : MonoBehaviour, IDetector
{
    [SerializeField] [Min(0f)] protected float range = 10f;
    [SerializeField] protected LayerMask targetLayers;
    [SerializeField] [Min(16)] protected int bufferSize = 64;
    protected HashSet<IDetectable> currentTargets = new();

    protected Collider2D[] hitsBuffer;
    protected bool isActive;
    protected HashSet<IDetectable> previousTargets = new();

    public float Range
    {
        get => range;
        set
        {
            var newRange = Mathf.Max(0f, value);
            if (!Mathf.Approximately(range, newRange))
            {
                range = newRange;
                OnRangeChanged();
            }
        }
    }

    public LayerMask TargetLayers
    {
        get => targetLayers;
        set => targetLayers = value;
    }

    protected virtual void Awake()
    {
        hitsBuffer = new Collider2D[bufferSize];
    }

    protected virtual void FixedUpdate()
    {
        if (!isActive) return;
        CollectHits();
    }

    protected virtual void OnEnable()
    {
        DetectionSystem.Instance?.Register(this);
    }

    protected virtual void OnDisable()
    {
        DetectionSystem.Instance?.Unregister(this);
        StopDetection();
    }

    protected virtual void OnDestroy()
    {
        // Clear event listeners to prevent memory leaks
        TargetDetected = null;
        TargetLost = null;
    }

    public bool IsActive => isActive;
    public IEnumerable<IDetectable> Targets => currentTargets;

    public event Action<IDetectable> TargetDetected;
    public event Action<IDetectable> TargetLost;

    public virtual void StartDetection()
    {
        isActive = true;
        currentTargets.Clear();
        previousTargets.Clear();
    }

    public virtual void StopDetection()
    {
        isActive = false;
        // Don't clear targets here to avoid duplicate LOST events
    }

    protected virtual void OnRangeChanged()
    {
    }

    protected abstract void CollectHits();

    protected void ProcessHits(int hitCount)
    {
        // Check for buffer overflow
        if (hitCount >= hitsBuffer.Length)
            Debug.LogWarning(
                $"[{name}] Detection buffer full! Consider increasing buffer size from {bufferSize}. Detected: {hitCount}");

        // Swap current and previous targets
        var temp = previousTargets;
        previousTargets = currentTargets;
        currentTargets = temp;
        currentTargets.Clear();

        // Process current hits
        for (var i = 0; i < Mathf.Min(hitCount, hitsBuffer.Length); i++)
        {
            var hit = hitsBuffer[i];
            if (hit == null) continue;

            var detectable = hit.GetComponent<IDetectable>();
            if (detectable != null && detectable.IsDetectable)
            {
                currentTargets.Add(detectable);

                if (!previousTargets.Contains(detectable)) RaiseDetected(detectable, hit.transform.position);
            }
        }

        // Check for lost targets
        foreach (var target in previousTargets)
            if (!currentTargets.Contains(target))
                RaiseLost(target);
    }

    protected void RaiseDetected(IDetectable target, Vector2 position)
    {
        TargetDetected?.Invoke(target);
        var evt = new DetectionEvent(this, target, DetectionEventType.DETECTED, Time.time, position);
        // You can send this event through an event system if needed
    }

    protected void RaiseLost(IDetectable target)
    {
        TargetLost?.Invoke(target);
        // Get position from Component
        var component = target as Component;
        var position = component != null ? (Vector2)component.transform.position : Vector2.zero;
        var evt = new DetectionEvent(this, target, DetectionEventType.LOST, Time.time, position);
    }
}