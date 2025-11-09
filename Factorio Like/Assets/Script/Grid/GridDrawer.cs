using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridDrawer : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public Material lineMaterial;
    public Transform player;

    [Header("Display Settings")]
    public float squareSize = 0.1f;
    public float viewRadius = 5f;

    [Header("Performance Settings")]
    public float updateInterval = 0.1f;     // Délai entre deux updates
    public float movementThreshold = 0.2f;  // Distance min pour recalculer

    private Vector3 lastPlayerPos;
    private Mesh mesh;

    void Start()
    {
        lastPlayerPos = player != null ? player.position : Vector3.zero;
        StartCoroutine(AutoUpdate());
    }

    IEnumerator AutoUpdate()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(player.position, lastPlayerPos) >= movementThreshold)
            {
                lastPlayerPos = player.position;
                DrawAtoms();
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    public void DrawAtoms()
    {
        if (gridManager == null || gridManager.cells == null || player == null)
            return;

        if (mesh != null)
            mesh.Clear();
        else
            mesh = new Mesh();

        var vertices = new List<Vector3>();
        var indices = new List<int>();
        var colors = new List<Color>();

        int index = 0;
        Vector3 playerPos = player.position;

        foreach (var cell in gridManager.cells)
        {
            if (Vector3.Distance(playerPos, cell.Position) > viewRadius)
                continue;

            float half = squareSize / 2f;

            // ---- ATOME ----
            if (cell.haveAtom)
            {
                Color atomColor = Color.gray;
                if (cell.atoms != null)
                {
                    foreach (var atom in cell.atoms)
                    {
                        if (atom.active)
                        {
                            atomColor = atom.color;
                            break;
                        }
                    }
                }
                DrawSquare(cell.Position, half, atomColor, ref index, vertices, indices, colors);
            }

            // ---- ÉLECTRICITÉ ----
            {
                float electricityHalf = half + 0.1f;
                Color electricityColor = cell.haveElectricity ? Color.blue : Color.gray;
                DrawSquare(cell.Position, electricityHalf, electricityColor, ref index, vertices, indices, colors);
            }

            // ---- PURIFICATION ----
            {
                float purifyHalf = half + 0.3f;
                Color purifyColor = cell.isPurify ? Color.green : Color.red;
                DrawSquare(cell.Position, purifyHalf, purifyColor, ref index, vertices, indices, colors);
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

    private void DrawSquare(Vector2 center, float halfSize, Color color, ref int index,
                            List<Vector3> vertices, List<int> indices, List<Color> colors)
    {
        Vector3 tl = transform.InverseTransformPoint(new Vector3(center.x - halfSize, center.y + halfSize, 0f));
        Vector3 tr = transform.InverseTransformPoint(new Vector3(center.x + halfSize, center.y + halfSize, 0f));
        Vector3 br = transform.InverseTransformPoint(new Vector3(center.x + halfSize, center.y - halfSize, 0f));
        Vector3 bl = transform.InverseTransformPoint(new Vector3(center.x - halfSize, center.y - halfSize, 0f));

        vertices.Add(tl); indices.Add(index++);
        vertices.Add(tr); indices.Add(index++);

        vertices.Add(tr); indices.Add(index++);
        vertices.Add(br); indices.Add(index++);

        vertices.Add(br); indices.Add(index++);
        vertices.Add(bl); indices.Add(index++);

        vertices.Add(bl); indices.Add(index++);
        vertices.Add(tl); indices.Add(index++);

        for (int i = 0; i < 8; i++)
            colors.Add(color);
    }
}
