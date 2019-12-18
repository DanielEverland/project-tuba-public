using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all debug elements
/// </summary>
public abstract class DebugElementBase : ScriptableObject
{
    protected static Material LineMaterialNoZDepth
    {
        get
        {
            if(lineMaterialNoZDepth == null)
            {
                lineMaterialNoZDepth = CreateDefaultLineMaterial(false);
            }

            return lineMaterialNoZDepth;
        }
    }
    private static Material lineMaterialNoZDepth;

    protected static Material LineMaterial
    {
        get
        {
            if (lineMaterial == null)
            {
                lineMaterial = CreateDefaultLineMaterial(true);
            }

            return lineMaterial;
        }
    }
    private static Material lineMaterial;
    
    private static Material CreateDefaultLineMaterial(bool zDepth)
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        Material material = new Material(shader);

        if (!zDepth)
        {
            // Turn off depth writes
            material.SetInt("_ZWrite", 0);
            material.SetInt("_ZTest", 0);
        }        

        return material;
    }

    public const string MenuRoot = Utility.MenuItemDebug;
    public const int MenuOrder = 200;

    public Canvas Canvas => DebugInitializer.Canvas2D;
    public Transform Root => Canvas.transform;

    private static readonly Vector2[] HexagonVertices = new Vector2[12]
    {
        new Vector2(-0.5f, 0.25f),
        new Vector2(0, 0.5f),
        new Vector2(0.5f, 0.25f),

        new Vector2(-0.5f, -0.25f),
        new Vector2(0, -0.5f),
        new Vector2(0.5f, -0.25f),

        new Vector2(-0.5f, 0.25f),
        new Vector2(0.5f, 0.25f),
        new Vector2(-0.5f, -0.25f),

        new Vector2(0.5f, 0.25f),
        new Vector2(0.5f, -0.25f),
        new Vector2(-0.5f, -0.25f),
    };

    /// <summary>
    /// Used for GL rendering
    /// </summary>
    public virtual void OnDebugElementRenderObject() { }

    public abstract void Enable();

    public void DrawGLLine(Vector2 a, Vector2 b, Color color)
    {
        GL.Color(color);
        GL.Vertex(a);
        GL.Vertex(b);
    }
    public void DrawGLLine(Vector2 a, Vector2 b)
    {
        GL.Vertex(a);
        GL.Vertex(b);
    }
    public void DrawHexagon(Vector3 center)
    {
        for (int i = 0; i < HexagonVertices.Length; i += 3)
        {
            GL.Vertex(Utility.ScaleToHexagonalSize(HexagonVertices[i + 0]) + center);
            GL.Vertex(Utility.ScaleToHexagonalSize(HexagonVertices[i + 1]) + center);
            GL.Vertex(Utility.ScaleToHexagonalSize(HexagonVertices[i + 2]) + center);
        }
    }
}
