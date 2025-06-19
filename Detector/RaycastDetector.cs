using System.Collections.Generic;
using UnityEngine;

public class RaycastDetector : BaseDetector
{
    private Vector2 direction = Vector2.right;
    private int rayCount = 1;
    private float spreadAngle;
    private readonly HashSet<Collider2D> uniqueHitsSet = new();

    public void ConfigureRays(Vector2 dir, int count, float spread)
    {
        direction = dir.normalized;
        rayCount = Mathf.Max(1, count);
        spreadAngle = Mathf.Clamp(spread, 0f, 180f);
    }

    protected override void CollectHits()
    {
        uniqueHitsSet.Clear();

        var angleStep = rayCount > 1 ? spreadAngle / (rayCount - 1) : 0f;
        var startAngle = -spreadAngle / 2f;

        for (var i = 0; i < rayCount; i++)
        {
            var angle = startAngle + angleStep * i;
            Vector2 rayDir = Quaternion.Euler(0, 0, angle) * direction;

            var hit = Physics2D.Raycast(
                transform.position,
                rayDir,
                range,
                targetLayers
            );

            if (hit.collider != null) uniqueHitsSet.Add(hit.collider);

            // Debug visualization
            Debug.DrawRay(transform.position, rayDir * range, IsActive ? Color.red : Color.gray);
        }

        // Convert unique hits to array
        var index = 0;
        foreach (var collider in uniqueHitsSet)
            if (index < hitsBuffer.Length)
            {
                hitsBuffer[index] = collider;
                index++;
            }

        ProcessHits(index);
    }
}