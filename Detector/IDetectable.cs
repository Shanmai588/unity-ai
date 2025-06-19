using UnityEngine;

public interface IDetectable
{
    bool IsDetectable { get; }

    GameObject GameObject { get; }
}