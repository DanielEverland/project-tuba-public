using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

/// <summary>
/// Effect that displays a <see cref="Shield"/> layer
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class ShieldEffectElement : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float pingEffectLerpSpeed = 5;

    private const int MinSegments = 30;
    private const float SegmentCurve = 20;
    private readonly string[] ColorKeys = new string[2]
    {
        "_Color",
        "_EmissionColor",
    };
    private Dictionary<string, Color> defaultColors = new Dictionary<string, Color>();

    private void Awake()
    {
        SetDefaultColors();
    }
    private void Update()
    {
        UpdatePingEffect();
    }
    public void Initialize(float radius)
    {
        int segments = GetSegmentCount(radius);

        lineRenderer.positionCount = segments;
        for (int i = 0; i < segments; i++)
        {
            float percentage = (float)i / (segments - 1);
            float radians = percentage * (2 * Mathf.PI);
            
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            Vector2 position = new Vector2(x, y) * radius;
            position = Utility.ScaleToOrthographicVector(position);
            lineRenderer.SetPosition(i, position);
        }
    }
    private void UpdatePingEffect()
    {
        for (int i = 0; i < ColorKeys.Length; i++)
        {
            string key = ColorKeys[i];

            lineRenderer.material.SetColor(key, Color.Lerp(lineRenderer.material.color, defaultColors[key], pingEffectLerpSpeed * Time.deltaTime));
        }        
    }
    public void PingDamageEffect(Color targetColor)
    {
        SetColor(targetColor);
    }
    private int GetSegmentCount(float radius)
    {
        return Mathf.RoundToInt(MinSegments + Mathf.Sqrt(radius) * SegmentCurve);
    }
    private void OnValidate()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
    }
    private void SetColor(Color color)
    {
        for (int i = 0; i < ColorKeys.Length; i++)
        {
            lineRenderer.material.SetColor(ColorKeys[i], color);
        }
    }
    private void SetDefaultColors()
    {
        defaultColors.Clear();
        for (int i = 0; i < ColorKeys.Length; i++)
        {
            defaultColors.Add(ColorKeys[i], lineRenderer.material.GetColor(ColorKeys[i]));
        }
    }
}
