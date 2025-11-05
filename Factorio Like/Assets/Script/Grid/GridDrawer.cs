using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridDrawer : MonoBehaviour
{
    public GridManager gridManager;
    public Material lineMaterial;
    public float squareSize = 0.1f;

    private Mesh mesh;

    void Start()
    {
        DrawAtoms();
    }

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

        foreach (var cell in gridManager.cells)
        {
            if (!cell.haveAtom) continue;
            
            Color currentColor = Color.gray;
            if (cell.atoms != null)
            {
                foreach (var atom in cell.atoms)
                {
                    if (atom.active)
                    {
                        currentColor = atom.color;
                        break;
                    }
                }
            }

            Vector2 pos2 = cell.GetPosition();
            Vector3 topLeftWorld     = new Vector3(pos2.x - half, pos2.y + half, 0f);
            Vector3 topRightWorld    = new Vector3(pos2.x + half, pos2.y + half, 0f);
            Vector3 bottomRightWorld = new Vector3(pos2.x + half, pos2.y - half, 0f);
            Vector3 bottomLeftWorld  = new Vector3(pos2.x - half, pos2.y - half, 0f);

            Vector3 topLeftLoc     = transform.InverseTransformPoint(topLeftWorld);
            Vector3 topRightLoc    = transform.InverseTransformPoint(topRightWorld);
            Vector3 bottomRightLoc = transform.InverseTransformPoint(bottomRightWorld);
            Vector3 bottomLeftLoc  = transform.InverseTransformPoint(bottomLeftWorld);

            vertices.Add(topLeftLoc);     indices.Add(index++);
            vertices.Add(topRightLoc);    indices.Add(index++);

            vertices.Add(topRightLoc);    indices.Add(index++);
            vertices.Add(bottomRightLoc); indices.Add(index++);

            vertices.Add(bottomRightLoc); indices.Add(index++);
            vertices.Add(bottomLeftLoc);  indices.Add(index++);

            vertices.Add(bottomLeftLoc);  indices.Add(index++);
            vertices.Add(topLeftLoc);     indices.Add(index++);

            for (int i = 0; i < 8; i++)
            {
                colors.Add(currentColor);
            }
               
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
