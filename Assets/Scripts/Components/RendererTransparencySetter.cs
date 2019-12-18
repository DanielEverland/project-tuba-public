using System.Collections.Generic;
using UnityEngine;

public class RendererTransparencySetter : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> renderers = null;
    
    public void SetTransparency(float value)
    {
        foreach (Renderer renderer in renderers)
        {
            Color color = renderer.material.color;

            color.a = value;

            renderer.material.color = color;
        }
    }
}