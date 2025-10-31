using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class URPGridOptimizedDrawer : MonoBehaviour
{
    public GridManager gridManager;
    public Material lineMaterial; // Unlit/Color ou Sprites/Default
    public float squareSize = 0.1f;
    public Color lineColor = Color.white;

    private Mesh mesh;

    void Start()
    {
        DrawAtoms();
    }

    // Appelle ça quand tu veux rafraîchir
    public void DrawAtoms()
    {
        if (gridManager == null || gridManager.cells == null) return;

        if (mesh != null)
            mesh.Clear();
        else
            mesh = new Mesh();

        var vertices = new List<Vector3>();
        var indices = new List<int>();
        var colors = new List<Color>();

        int index = 0;
        float half = squareSize / 2f;

        // On calcule en espace monde puis on convertit en espace local du GameObject
        foreach (var cell in gridManager.cells)
        {
            if (!cell.haveAtome) continue;

            Vector2 pos2 = cell.GetPosition();
            // coordonnées monde de chaque coin du carré (z = 0)
            Vector3 topLeftWorld     = new Vector3(pos2.x - half, pos2.y + half, 0f);
            Vector3 topRightWorld    = new Vector3(pos2.x + half, pos2.y + half, 0f);
            Vector3 bottomRightWorld = new Vector3(pos2.x + half, pos2.y - half, 0f);
            Vector3 bottomLeftWorld  = new Vector3(pos2.x - half, pos2.y - half, 0f);

            // Convertir chaque point monde en espace local du GameObject qui contient le mesh
            Vector3 topLeftLoc     = transform.InverseTransformPoint(topLeftWorld);
            Vector3 topRightLoc    = transform.InverseTransformPoint(topRightWorld);
            Vector3 bottomRightLoc = transform.InverseTransformPoint(bottomRightWorld);
            Vector3 bottomLeftLoc  = transform.InverseTransformPoint(bottomLeftWorld);

            // Ajouter 4 segments (chaque segment = 2 vertices), on utilise MeshTopology.Lines
            vertices.Add(topLeftLoc);     indices.Add(index++);
            vertices.Add(topRightLoc);    indices.Add(index++);

            vertices.Add(topRightLoc);    indices.Add(index++);
            vertices.Add(bottomRightLoc); indices.Add(index++);

            vertices.Add(bottomRightLoc); indices.Add(index++);
            vertices.Add(bottomLeftLoc);  indices.Add(index++);

            vertices.Add(bottomLeftLoc);  indices.Add(index++);
            vertices.Add(topLeftLoc);     indices.Add(index++);

            // couleur pour chaque vertex
            for (int i = 0; i < 8; i++) colors.Add(lineColor);
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        mesh.SetColors(colors);

        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();

        mf.mesh = mesh;
        mr.material = lineMaterial;
    }
}
