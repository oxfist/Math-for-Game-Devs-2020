using UnityEditor;
using UnityEngine;

public class ForwardDirectionVectorEditor : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Range(0, 1)] public float lookAtThreshold;

    private readonly Color _lookingAtColor = Color.green;
    private readonly Color _notLookingAtColor = Color.red;

    private void OnDrawGizmos()
    {
        DrawForwardDirectionVector(target.position);
    }

    private void DrawForwardDirectionVector(Vector2 targetPosition)
    {
        Transform currentTransform = transform;
        Vector2 currentPosition = currentTransform.position;
        Vector2 direction = currentTransform.right;
        Vector2 targetDirection = (targetPosition - currentPosition).normalized;

        Handles.color = ColorFromLookAtStatus(LookingAt(direction, targetDirection));

        Handles.DrawLine(currentPosition, currentPosition + direction);
    }

    private Color ColorFromLookAtStatus(bool lookingAt)
    {
        return lookingAt ? _lookingAtColor : _notLookingAtColor;
    }

    private bool LookingAt(Vector2 direction, Vector2 targetDirection)
    {
        return GetLookAtValue(direction, targetDirection) >= lookAtThreshold;
    }

    private static float GetLookAtValue(Vector2 direction, Vector2 targetDirection)
    {
        return Vector2.Dot(direction, targetDirection);
    }
}
