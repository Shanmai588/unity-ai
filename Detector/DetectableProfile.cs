using UnityEngine;

public class DetectableProfile : ScriptableObject
{
    public DetectionPriority Priority = DetectionPriority.MEDIUM;
    public float BaseVisibility = 1f;

    [Tooltip("The physics layer this detectable should be on")]
    public int PhysicsLayer;
}