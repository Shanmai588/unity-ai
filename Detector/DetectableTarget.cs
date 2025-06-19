using UnityEngine;

public class DetectableTarget : MonoBehaviour, IDetectable
{
    [SerializeField] protected DetectableProfile profile;
    protected bool isHidden;

    public DetectableProfile Profile
    {
        get => profile;
        set => profile = value;
    }

    protected virtual void OnEnable()
    {
        if (profile != null) gameObject.layer = profile.PhysicsLayer;
    }

    public bool IsDetectable => !isHidden && gameObject.activeInHierarchy;
    public GameObject GameObject => gameObject;

    public virtual void Hide()
    {
        isHidden = true;
    }

    public virtual void Show()
    {
        isHidden = false;
    }
}