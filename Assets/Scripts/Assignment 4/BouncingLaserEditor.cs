﻿using UnityEngine;

public class BouncingLaserEditor : MonoBehaviour
{
    [SerializeField] private bool bounceDebug;
    [Range(0, 10)] [SerializeField] private int maxBounces = 1;
    [Range(1, 15)] [SerializeField] private float maxLaserDistance = 1;
    [SerializeField] private Color hitColor = Color.green;
    [SerializeField] private Color outOfRangeColor = Color.yellow;

    private const float BaseVectorSize = .35f;

    private void OnDrawGizmos()
    {
        Transform currentTransform = transform;
        Vector3 playerPosition = currentTransform.position;
        Vector3 playerDirection = currentTransform.forward;
        DrawBouncingLaser(playerPosition, playerDirection, maxBounces);
    }

    private void DrawBouncingLaser(Vector3 startPosition, Vector3 direction, int bounces)
    {
        if (bounces < 0) return;

        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, maxLaserDistance))
        {
            Debug.DrawRay(startPosition, direction * hit.distance, hitColor);
            Vector3 bounceDirection = CalculateBounceDirection(hit, direction);

            DrawBouncingLaser(hit.point, bounceDirection, bounces - 1);
        }
        else
        {
            Debug.DrawRay(startPosition, direction * maxLaserDistance, outOfRangeColor);
        }
    }

    private Vector3 CalculateBounceDirection(RaycastHit hit, Vector3 direction)
    {
        Vector3 up = hit.normal;
        Vector3 right = Vector3.Cross(up, direction).normalized;
        Vector3 front = Vector3.Cross(right, up);

        if (bounceDebug)
        {
            DrawBaseVectorsAt(hit.point, right, up, front);
        }

        Vector3 bounceDirection = Vector3.Cross(direction, right);

        return bounceDirection;
    }

    private static void DrawBaseVectorsAt(Vector3 position, Vector3 right, Vector3 up, Vector3 front)
    {
        Debug.DrawRay(position, up * BaseVectorSize, Color.green);
        Debug.DrawRay(position, right * BaseVectorSize, Color.red);
        Debug.DrawRay(position,  front * BaseVectorSize, Color.cyan);
    }
}
