using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the data necessary to generate the environment for a level
/// </summary>
public class LevelEnvironmentData
{
    public List<Vector3> Vertices = new List<Vector3>();
    public Dictionary<int, List<int>> Triangles = new Dictionary<int, List<int>>();
    public List<Vector2> UVs = new List<Vector2>();
    public List<Material> Materials = new List<Material>();

    public void SetTriangles(Material material, int[] triangles)
    {
        if (!Materials.Contains(material))
        {
            Materials.Add(material);
        }

        int submeshIndex = GetSubmeshIndex(material);

        if (!Triangles.ContainsKey(submeshIndex))
            Triangles.Add(submeshIndex, new List<int>());

        Triangles[submeshIndex].AddRange(triangles);
    }
    public int GetSubmeshIndex(Material material)
    {
        if (Materials.Contains(material))
        {
            return Materials.IndexOf(material);
        }
        else
        {
            Materials.Add(material);
            return Materials.Count - 1;
        }
    }
    public Mesh BuildMesh(string name)
    {
        Mesh mesh = new Mesh();
        mesh.name = name;
        mesh.vertices = Vertices.ToArray();
        mesh.uv = UVs.ToArray();

        mesh.subMeshCount = Triangles.Count;
        for (int i = 0; i < Triangles.Count; i++)
        {
            mesh.SetTriangles(Triangles[i], i);
        }

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        return mesh;
    }
}
