using UnityEngine;

[CreateAssetMenu(menuName = "Tracing/Shape")]
public class TraceShapeData : ScriptableObject
{
    public Vector2[] points;
    public float allowedError = 20f;
    public Color traceColor = Color.white;
}
