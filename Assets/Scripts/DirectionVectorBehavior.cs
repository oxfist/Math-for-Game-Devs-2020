using UnityEditor;
using UnityEngine;

public class DirectionVectorBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void OnDrawGizmos()
    {
        DrawDirectionVectorToward(target.position);
    }

    private void DrawDirectionVectorToward(Vector2 targetPosition)
    {
        Handles.color = Color.white;

        Vector2 startingPosition = transform.position;
        Vector2 directionVector = (targetPosition - startingPosition).normalized;
        Vector2 endingPosition = directionVector + startingPosition;

        Gizmos.DrawLine(startingPosition, endingPosition);
    }
}
