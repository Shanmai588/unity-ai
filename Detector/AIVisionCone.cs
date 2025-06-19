using System.Collections.Generic;
using UnityEngine;

public class AIVisionCone : MonoBehaviour
{
#if UNITY_EDITOR
    private static Material sharedVisionConeMaterial;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private ConeDetector coneDetector;

    [SerializeField] private Color detectionColor = new(1f, 0f, 0f, 0.3f);
    [SerializeField] private int segments = 30;

    private bool needsRebuild = true;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        coneDetector = GetComponent<ConeDetector>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Use shared material to prevent memory leaks
        if (sharedVisionConeMaterial == null) sharedVisionConeMaterial = new Material(Shader.Find("Sprites/Default"));

        meshRenderer.sharedMaterial = sharedVisionConeMaterial;
        SetColor(detectionColor);

        SetDirty();
    }

    public void SetColor(Color color)
    {
        detectionColor = color;
        if (meshRenderer != null && meshRenderer.sharedMaterial != null)
        {
            // Create material instance only if we need a unique color
            if (meshRenderer.sharedMaterial == sharedVisionConeMaterial)
                meshRenderer.material = new Material(sharedVisionConeMaterial);
            meshRenderer.material.color = color;
        }
    }

    public void SetDirty()
    {
        needsRebuild = true;
    }

    public void Rebuild()
    {
        if (mesh == null || coneDetector == null) return;

        var angle = coneDetector.ConeAngle;
        var range = coneDetector.Range;
        var facing = coneDetector.FacingDirection;

        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        // Center vertex
        vertices.Add(Vector3.zero);

        // Generate cone vertices
        var angleStep = angle / segments;
        var startAngle = -angle / 2f;

        for (var i = 0; i <= segments; i++)
        {
            var currentAngle = startAngle + angleStep * i;
            Vector2 dir = Quaternion.Euler(0, 0, currentAngle) * facing;
            vertices.Add(dir * range);
        }

        // Generate triangles
        for (var i = 0; i < segments; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        needsRebuild = false;
    }

    private void LateUpdate()
    {
        if (needsRebuild) Rebuild();
    }
#endif
}