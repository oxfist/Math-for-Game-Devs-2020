using UnityEditor;
using UnityEngine;

public class SpaceTransformerEditor : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void OnDrawGizmos()
    {
        Vector2 targetLocalPosition = WorldToLocalPosition(target.position, transform);
        Debug.Log($"Local position: {targetLocalPosition}");

        Vector2 targetWorldPosition = LocalToWorldPosition(targetLocalPosition, transform);
        Debug.Log($"World position: {targetWorldPosition}");
        DrawLocalAxisFrom(targetWorldPosition);
    }

    private static Vector2 LocalToWorldPosition(Vector2 targetLocalPosition, Transform worldTransform)
    {
        Vector2 currentPosition = worldTransform.position;
        Vector2 horizontalSide = worldTransform.right * targetLocalPosition.x;
        Vector2 verticalSide = worldTransform.up * targetLocalPosition.y;

        Vector2 worldPosition = horizontalSide + verticalSide + currentPosition;
        return worldPosition;
    }

    private static void DrawLocalAxisFrom(Vector2 position)
    {
        Handles.color = Color.yellow;
        Handles.DrawLine(position, Vector2.right + position);
        Handles.DrawLine(position, Vector2.up + position);
        Handles.color = Color.white;
    }

    private static Vector2 WorldToLocalPosition(Vector2 targetPosition, Transform worldTransform)
    {
        Vector2 currentLocalPosition = worldTransform.position;

        Vector2 hypotenuse = CurrentPositionToTarget(currentLocalPosition, targetPosition);
        float hypotenuseSquared = hypotenuse.sqrMagnitude;

        float horizontalSide = Vector2.Dot(worldTransform.right, hypotenuse);
        float verticalSign = Mathf.Sign(Vector2.Dot(worldTransform.up, hypotenuse));
        float verticalSide = verticalSign * VerticalSide(hypotenuseSquared, horizontalSide);

        return new Vector2
        {
            x = horizontalSide,
            y = verticalSide
        };
    }

    private static float VerticalSide(float hypotenuseSquared, float horizontal)
    {
        float vertical = Mathf.Sqrt( hypotenuseSquared- horizontal * horizontal);
        return float.IsNaN(vertical) ? 0 : vertical;
    }

    private static Vector2 CurrentPositionToTarget(Vector2 startingPosition, Vector2 endingPosition)
    {
        return endingPosition - startingPosition;
    }
}
