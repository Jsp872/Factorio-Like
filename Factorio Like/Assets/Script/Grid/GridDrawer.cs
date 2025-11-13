using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float updateInterval = 0.1f;
    public float movementThreshold = 0.2f;

    private Vector3 lastPlayerPos;
    private Mesh mesh;

    private readonly List<Vector3> vertices = new();
    private readonly List<int> indices = new();
    private readonly List<Color> colors = new();

    private void Start()
    {
        lastPlayerPos = player != null ? player.position : Vector3.zero;
        StartCoroutine(AutoUpdate());
    }

    private IEnumerator AutoUpdate()
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
        if (gridManager == null || gridManager.cells == null || player == null) return;

        if (mesh == null) mesh = new Mesh();
        else mesh.Clear();

        vertices.Clear();
        indices.Clear();
        colors.Clear();

        int index = 0;
        Vector3 playerPos = player.position;

        foreach (var cell in gridManager.cells)
        {
            if (Vector3.Distance(playerPos, cell.Position) > viewRadius) continue;

            float half = squareSize / 2f;

            // Atom
            if (cell.haveAtom)
            {
                Color atomColor = Color.gray;
                foreach (var atom in cell.atoms)
                {
                    if (atom.active) { atomColor = atom.color; break; }
                }
                DrawSquare(cell.Position, half, atomColor, ref index);
            }

            // Electricity
            DrawSquare(cell.Position, half + 0.1f, cell.haveElectricity ? Color.blue : Color.gray, ref index);

            // Purification
            DrawSquare(cell.Position, half + 0.3f, cell.isPurify ? Color.green : Color.red, ref index);
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        mesh.SetColors(colors);

        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();
        mf.mesh = mesh;
        mr.material = lineMaterial;
    }

    private void DrawSquare(Vector2 center, float halfSize, Color color, ref int index)
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

        for (int i = 0; i < 8; i++) colors.Add(color);
    }
}
