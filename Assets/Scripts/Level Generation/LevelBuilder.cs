using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Builds the level in the scene
/// </summary>
public static class LevelBuilder
{
    private static readonly Vector3 CeilingOffset = Vector3.forward * Utility.HexelCeilingHeight;

    public static Level Build(ILevelEntity room)
    {
        LevelBuildData buildData = new LevelBuildData();
        room.AddToLevel(buildData);        

        return Build(buildData);
    }
    public static Level Build(LevelBuildData data)
    {
        Level level = Level.CreateWithRoot();
        level.AddBuildData(data);

        CreateEnvironment(level, data);
        CreateColliders(level, data);
        CreateShadowCasters(level, data);

        SpawnGameObjects(level, data);

        return level;
    }
    private static void SpawnGameObjects(Level level, LevelBuildData buildData)
    {
        EnvironmentObject environment = CreateEnvironment("Game Objects");

        foreach (var keyValuePair in buildData.Prefabs)
        {
            GameObject instance = GameObject.Instantiate(keyValuePair.Value);
            instance.transform.position = Utility.AxialToWorldPosition(keyValuePair.Key);
            instance.transform.SetParent(environment.GameObject.transform);
        }

        level.AddEnvironment(environment.GameObject);
    }
    private static void CreateShadowCasters(Level level, LevelBuildData buildData)
    {
        EnvironmentObject environment = CreateEnvironment("Shadow Caster", EnvironmentObjectComponent.MeshFilter | EnvironmentObjectComponent.MeshRenderer);
        LevelEnvironmentData data = new LevelEnvironmentData();
        data.Triangles.Add(0, new List<int>());
        
        foreach (var pair in buildData.Tiles)
        {
            if (!pair.Value.CastsShadows)
                continue;
            
            Vector2 worldPosition = Utility.AxialToWorldPosition(pair.Key);
            byte bitmask = Utility.GetBitmask(pair.Key, level.TilePositions);
            List<Line> lines = pair.Value.Brush.GetCollisionCoordinates(bitmask);

            foreach (Line line in lines)
            {
                AddLineToMeshData(data, line, worldPosition);
            }
        }
        
        Mesh mesh = data.BuildMesh("Shadow Caster Mesh");
        environment.Filter.mesh = mesh;
        environment.Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        environment.Renderer.material = new Material(Shader.Find("Standard"));

        level.AddEnvironment(environment.GameObject);
    }
    private static void CreateColliders(Level level, LevelBuildData buildData)
    {
        //This contains the bounds of the level. Projectiles can pass these colliders, but entities cannot.
        EnvironmentObject edgeEnvironment = CreateEnvironment("Edges");
        edgeEnvironment.GameObject.layer = LayerMask.NameToLayer("EnvironmentNoBlock");
        foreach (List<Vector2> list in LevelBuilderUtility.CreateCollisionEdges(level, buildData.Tiles.Where(x => x.Value.IsWalkable)))
        {
            AddEdgeCollider(edgeEnvironment, list);
        }        
        level.AddEnvironment(edgeEnvironment.GameObject);


        //This contains tiles that have walls. No entities can pass through these
        EnvironmentObject wallEnvironments = CreateEnvironment("Walls");
        wallEnvironments.GameObject.layer = LayerMask.NameToLayer("Environment");
        foreach (List<Vector2> list in LevelBuilderUtility.CreateCollisionEdges(level, buildData.Tiles.Where(x => !x.Value.IsWalkable)))
        {
            AddEdgeCollider(wallEnvironments, list);
        }
        level.AddEnvironment(wallEnvironments.GameObject);

        void AddEdgeCollider(EnvironmentObject environment, List< Vector2> list)
        {
            EdgeCollider2D edgeCollider = environment.GameObject.AddComponent<EdgeCollider2D>();
            edgeCollider.points = list.ToArray();
        }
    }
    private static void AddLineToMeshData(LevelEnvironmentData data, Line line, Vector3 offset)
    {
        int verticeCount = data.Vertices.Count;

        data.Vertices.AddRange(new Vector3[4]
        {
            offset + Utility.ScaleToHexagonalSize((Vector3)line.A),
            offset + Utility.ScaleToHexagonalSize((Vector3)line.A + CeilingOffset),
            offset + Utility.ScaleToHexagonalSize((Vector3)line.B + CeilingOffset),
            offset + Utility.ScaleToHexagonalSize((Vector3)line.B),
        });

        data.Triangles[0].AddRange(new int[6]
        {
            verticeCount + 2, verticeCount + 1, verticeCount + 0,
            verticeCount + 0, verticeCount + 3, verticeCount + 2,
        });
    }
    private static void CreateEnvironment(Level level, LevelBuildData buildData)
    {
        EnvironmentObject environment = CreateEnvironment("Environment", EnvironmentObjectComponent.MeshFilter | EnvironmentObjectComponent.MeshRenderer);
        LevelEnvironmentData data = new LevelEnvironmentData();
        
        foreach (var pair in buildData.Tiles)
        {
            Vector2 worldPosition = Utility.AxialToWorldPosition(pair.Key);
            byte bitmask = Utility.GetBitmask(pair.Key, level.TilePositions);

            AddTileToLevel(data, pair.Value, worldPosition, bitmask);
        }

        Mesh mesh = data.BuildMesh("Environment Mesh");
        environment.Filter.mesh = mesh;
        environment.Renderer.materials = data.Materials.ToArray();

        level.AddEnvironment(environment.GameObject);
    }
    private static void AddTileToLevel(LevelEnvironmentData data, TileData tile, Vector3 worldPosition, byte bitmask)
    {
        int verticeCount = data.Vertices.Count;

        TileMeshData meshData = tile.Brush.GetMeshData(bitmask);

        // Vertices.
        foreach (Vector3 vertex in meshData.Vertices)
        {
            data.Vertices.Add(Utility.ScaleToHexagonalSize(vertex) + worldPosition);
        }

        // Triangles.
        int[][] rawTriangles = meshData.Triangles;
        for (int i = 0; i < rawTriangles.Length; i++)
        {
            int[] trianglesToSet = new int[rawTriangles[i].Length];

            // Offset the triangles
            for (int w = 0; w < rawTriangles[i].Length; w++)
                trianglesToSet[w] = rawTriangles[i][w] + verticeCount;

            data.SetTriangles(tile.BrushMaterials[i], trianglesToSet);
        }

        // UVs.
        foreach (Vector2 uv in meshData.UVs)
        {
            data.UVs.Add(uv);
        }

        // Edges.
        if (!AxialDirection.ContainsDirection(bitmask, AxialDirection.DownLeft) || !AxialDirection.ContainsDirection(bitmask, AxialDirection.DownRight))
        {
            EdgeBuilder.Build(data, tile, worldPosition, bitmask);
        }
    }
    private static EnvironmentObject CreateEnvironment(string name, EnvironmentObjectComponent components = EnvironmentObjectComponent.None)
    {
        EnvironmentObject environment = new EnvironmentObject();

        environment.GameObject = new GameObject(name);

        if(components.HasFlag(EnvironmentObjectComponent.MeshFilter))
            environment.Filter = environment.GameObject.AddComponent<MeshFilter>();

        if (components.HasFlag(EnvironmentObjectComponent.MeshRenderer))
            environment.Renderer = environment.GameObject.AddComponent<MeshRenderer>();

        return environment;
    }

    private class EnvironmentObject
    {
        public GameObject GameObject;
        public MeshFilter Filter;
        public MeshRenderer Renderer;
    }
    public enum EnvironmentObjectComponent
    {
        None            = 0b0000_0000,

        MeshFilter      = 0b0000_0001,
        MeshRenderer    = 0b0000_0010,
    }
}
