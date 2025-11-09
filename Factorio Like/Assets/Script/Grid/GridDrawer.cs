using UnityEngine;
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
    public float updateInterval = 0.1f;     // Temps min entre 2 mises à jour
    public float movementThreshold = 0.2f;  // Mouvement min du joueur pour recalculer

    private float lastUpdateTime = 0f;
    private Vector3 lastPlayerPos;
    private Mesh mesh;


    //---------- INIT ----------

    void Start()
    {
        lastPlayerPos = player != null ? player.position : Vector3.zero;
        DrawAtoms();
    }


    //---------- AUTO UPDATE OPTIMISÉ ----------

    void Update()
    {
        if (player == null) return;

        // Trop tôt pour mettre à jour
        if (Time.time - lastUpdateTime < updateInterval)
            return;

        // Pas assez de mouvement → inutile de recalculer
        if (Vector3.Distance(player.position, lastPlayerPos) < movementThreshold)
            return;

        lastPlayerPos = player.position;
        lastUpdateTime = Time.time;

        DrawAtoms();
    }


    //---------- DESSIN DES ATOMES AUTOUR DU JOUEUR ----------

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
        float half = squareSize / 2f;

        Vector3 playerPos = player.position;

        foreach (var cell in gridManager.cells)
        {
            // Affiche seulement les atomes proches du joueur
            if (Vector3.Distance(playerPos, cell.Position) > viewRadius)
                continue;

            if (!cell.haveAtom)
                continue;

            // Couleur du premier atome actif
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

            // Carré autour du centre de la cellule
            Vector3 topLeftWorld     = new Vector3(pos2.x - half, pos2.y + half, 0f);
            Vector3 topRightWorld    = new Vector3(pos2.x + half, pos2.y + half, 0f);
            Vector3 bottomRightWorld = new Vector3(pos2.x + half, pos2.y - half, 0f);
            Vector3 bottomLeftWorld  = new Vector3(pos2.x - half, pos2.y - half, 0f);

            // Conversion locale pour mesh
            Vector3 topLeftLoc     = transform.InverseTransformPoint(topLeftWorld);
            Vector3 topRightLoc    = transform.InverseTransformPoint(topRightWorld);
            Vector3 bottomRightLoc = transform.InverseTransformPoint(bottomRightWorld);
            Vector3 bottomLeftLoc  = transform.InverseTransformPoint(bottomLeftWorld);

            // 4 lignes = 8 vertices
            vertices.Add(topLeftLoc);     indices.Add(index++);
            vertices.Add(topRightLoc);    indices.Add(index++);

            vertices.Add(topRightLoc);    indices.Add(index++);
            vertices.Add(bottomRightLoc); indices.Add(index++);

            vertices.Add(bottomRightLoc); indices.Add(index++);
            vertices.Add(bottomLeftLoc);  indices.Add(index++);

            vertices.Add(bottomLeftLoc);  indices.Add(index++);
            vertices.Add(topLeftLoc);     indices.Add(index++);

            for (int i = 0; i < 8; i++)
                colors.Add(currentColor);
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
