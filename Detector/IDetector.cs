using System;
using System.Collections.Generic;

public interface IDetector
{
    bool IsActive { get; }
    IEnumerable<IDetectable> Targets { get; }
    event Action<IDetectable> TargetDetected;
    event Action<IDetectable> TargetLost;
    void StartDetection();
    void StopDetection();
}