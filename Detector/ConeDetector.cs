using UnityEngine;

public class ConeDetector : BaseDetector
{
    [Header("Cone Configuration")] [SerializeField]
    private float coneAngle = 45f;

    [SerializeField] private Vector2 facingDirection = Vector2.right;

    [Header("Line of Sight")] [SerializeField]
    private LayerMask obstacleLayers = -1;

    [SerializeField] private bool requireLineOfSight = true;

    // Track changes for vision cone updates
    private float lastConeAngle;
    private Vector2 lastFacingDirection;
    private float lastRange;

    private AIVisionCone visionCone;

    public float ConeAngle => coneAngle;
    public Vector2 FacingDirection => facingDirection;

    protected override void Awake()
    {
        base.Awake();
        visionCone = GetComponent<AIVisionCone>();
        CacheValues();
    }

    public void ConfigureCone(float angle, Vector2 facing)
    {
        coneAngle = Mathf.Clamp(angle, 0f, 360f);
        facingDirection = facing.normalized;
        CheckForChanges();
    }

    protected override void OnRangeChanged()
    {
        CheckForChanges();
    }

    protected override void CollectHits()
    {
        var hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            range,
            hitsBuffer,
            targetLayers
        );

        // Filter by cone angle
        var validHits = 0;
        for (var i = 0; i < hitCount; i++)
        {
            var hit = hitsBuffer[i];
            if (hit == null) continue;

            Vector2 dirToTarget = (hit.transform.position - transform.position).normalized;
            var angle = Vector2.Angle(facingDirection, dirToTarget);

            if (angle <= coneAngle / 2f)
            {
                var canSeeTarget = true;

                // Check line of sight if required
                if (requireLineOfSight)
                {
                    var distance = Vector2.Distance(transform.position, hit.transform.position);
                    var lineOfSight = Physics2D.Raycast(
                        transform.position,
                        dirToTarget,
                        distance,
                        obstacleLayers
                    );

                    canSeeTarget = lineOfSight.collider == null || lineOfSight.collider == hit;
                }

                if (canSeeTarget)
                {
                    hitsBuffer[validHits] = hit;
                    validHits++;
                }
            }
        }

        ProcessHits(validHits);
        CheckForChanges();
    }

    private void CacheValues()
    {
        lastConeAngle = coneAngle;
        lastRange = range;
        lastFacingDirection = facingDirection;
    }

    private void CheckForChanges()
    {
        if (visionCone != null &&
            (lastConeAngle != coneAngle ||
             lastRange != Range ||
             lastFacingDirection != facingDirection))
        {
            visionCone.SetDirty();
            CacheValues();
        }
    }
}