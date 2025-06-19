using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    private static DetectionSystem instance;
    private readonly HashSet<IDetector> detectors = new();

    public static DetectionSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DetectionSystem>();
                if (instance == null)
                {
                    var go = new GameObject("DetectionSystem");
                    instance = go.AddComponent<DetectionSystem>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        FixedUpdateJobs();
        SyncTransforms();
    }

    public void Register(IDetector detector)
    {
        detectors.Add(detector);
    }

    public void Unregister(IDetector detector)
    {
        detectors.Remove(detector);
    }

    public void FixedUpdateJobs()
    {
        // implement Unity Job System for parallel processing
        // For now, detectors handle their own updates
    }

    public void SyncTransforms()
    {
        Physics2D.SyncTransforms();
    }
}