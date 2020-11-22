using UnityEditor;
using UnityEngine;

public class RadialDetectorEditor : MonoBehaviour
{
    [Range(0, 3)] public float radius = 1f;
    [SerializeField] private Transform secondPoint;

    private void OnDrawGizmos()
    {
        DrawRadiusRange(secondPoint);
    }

    private void DrawRadiusRange(Transform point)
    {
        Handles.color = GetRadiusColor(point);
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
    }

    private Color GetRadiusColor(Transform point)
    {
        return GetDistanceBetween(transform.position, point.position) <= radius * radius ? Color.green : Color.red;
    }

    private static double GetDistanceBetween(Vector2 transformPosition, Vector2 point)
    {
        return (point - transformPosition).sqrMagnitude;
    }
}
