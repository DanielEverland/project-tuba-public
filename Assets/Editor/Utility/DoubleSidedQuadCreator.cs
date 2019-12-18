using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DoubleSidedQuadCreator : MonoBehaviour
{
    [MenuItem(Utility.MenuItemRoot + "Double Sided Quad", priority = 10000)]
    private static void CreateDoubleSidedQuad()
    {
        string selectedPath = EditorUtility.SaveFilePanelInProject("Save Double Sided Quad", "Double Sided Quad", "asset", "");

        if (selectedPath == null)
            return;

        Mesh mesh = CreateMesh();

        AssetDatabase.CreateAsset(mesh, selectedPath);
        AssetDatabase.Refresh();

        Selection.activeObject = mesh;
    }
    private static Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Double Sided Quad";

        mesh.vertices = new Vector3[8]
        {
            new Vector3(-0.5f, 0.5f),
            new Vector3(0.5f, 0.5f),
            new Vector3(0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f),

            new Vector3(-0.5f, 0.5f),
            new Vector3(0.5f, 0.5f),
            new Vector3(0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f),
        };

        mesh.triangles = new int[12]
        {
            0, 1, 2,
            0, 2, 3,

            6, 5, 4,
            7, 6, 4,
        };

        mesh.uv = new Vector2[8]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),

            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
        };

        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }
}
