using UnityEngine;

public readonly struct DetectionEvent
{
    public readonly IDetector Detector;
    public readonly IDetectable Target;
    public readonly DetectionEventType Type;
    public readonly float Time;
    public readonly Vector2 Position;

    public DetectionEvent(IDetector detector, IDetectable target, DetectionEventType type, float time, Vector2 position)
    {
        Detector = detector;
        Target = target;
        Type = type;
        Time = time;
        Position = position;
    }
}