using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretPlacerEditor : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.green;
    [SerializeField] private float gunHeight = 1.3f;
    [SerializeField] private float distanceBetweenGuns = .3f;
    [SerializeField] private float gunBarrelLength = .8f;

    private static readonly Vector3[] WireframeCubeLocalCoordinates = {
        // bottom 4 positions:
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(1, 0, -1),

        // top 4 positions:
        new Vector3(1, 2, 1),
        new Vector3(-1, 2, 1),
        new Vector3(-1, 2, -1),
        new Vector3(1, 2, -1)
    };

    private const float BaseVectorSize = 1f;

    private void OnDrawGizmos()
    {
        Transform currentTransform = transform;
        Vector3 playerPosition = currentTransform.position;
        Vector3 playerDirection = currentTransform.forward;
        PlaceTurretAt(playerPosition, playerDirection);
    }

    private void PlaceTurretAt(Vector3 startPosition, Vector3 direction)
    {
        if (Physics.Raycast(startPosition, direction, out RaycastHit hit))
        {
            Debug.DrawRay(startPosition, direction * hit.distance, hitColor);

            Vector3 up = hit.normal;
            Vector3 right = Vector3.Cross(up, direction).normalized;
            Vector3 forward = Vector3.Cross(right, up);

            DrawBaseVectorsAt(hit.point, right, up, forward);

            Matrix4x4 localToWorldTransformation = BuildTransformationMatrixFrom(hit.point, up, forward);
            DrawWireFrameCubeAt(WireframeCubeLocalCoordinates, localToWorldTransformation);
            DrawGunsWith(gunHeight, distanceBetweenGuns, gunBarrelLength, localToWorldTransformation);
        }
    }

    private static void DrawGunsWith(float gunHeight, float distanceBetweenGuns, float gunBarrelLength, Matrix4x4 localToWorldTransformation)
    {
        Vector3 localGunPosition = Vector3.up * gunHeight;
        Vector3 localGunEndPosition = Vector3.forward * gunBarrelLength;
        Vector3 localBarrelOffset = Vector3.right * (distanceBetweenGuns / 2);

        Vector3 worldGunPosition1 = TransformLocalToWorld(localGunPosition + localBarrelOffset, localToWorldTransformation);
        Vector3 worldGunEndPosition1 = TransformLocalToWorld(localGunPosition + localGunEndPosition + localBarrelOffset, localToWorldTransformation);
        Vector3 worldGunPosition2 = TransformLocalToWorld(localGunPosition - localBarrelOffset, localToWorldTransformation);
        Vector3 worldGunEndPosition2 = TransformLocalToWorld(localGunPosition + localGunEndPosition - localBarrelOffset, localToWorldTransformation);

        Debug.DrawLine(worldGunPosition1, worldGunEndPosition1, Color.yellow);
        Debug.DrawLine(worldGunPosition2, worldGunEndPosition2, Color.yellow);
        Debug.DrawLine(worldGunPosition1, worldGunPosition2, Color.yellow);
    }

    private static void DrawWireFrameCubeAt(IEnumerable<Vector3> cubeCoordinates, Matrix4x4 localToWorldTransformation)
    {
        List<Vector3> worldCoordinates = cubeCoordinates.Select(coordinate => TransformLocalToWorld(coordinate, localToWorldTransformation)).ToList();

        // Bottom sides
        Debug.DrawLine(worldCoordinates[0], worldCoordinates[1], Color.yellow);
        Debug.DrawLine(worldCoordinates[1], worldCoordinates[2], Color.yellow);
        Debug.DrawLine(worldCoordinates[2], worldCoordinates[3], Color.yellow);
        Debug.DrawLine(worldCoordinates[3], worldCoordinates[0], Color.yellow);

        // Top sides
        Debug.DrawLine(worldCoordinates[4], worldCoordinates[5], Color.yellow);
        Debug.DrawLine(worldCoordinates[5], worldCoordinates[6], Color.yellow);
        Debug.DrawLine(worldCoordinates[6], worldCoordinates[7], Color.yellow);
        Debug.DrawLine(worldCoordinates[7], worldCoordinates[4], Color.yellow);

        // Vertical sides
        Debug.DrawLine(worldCoordinates[0], worldCoordinates[4], Color.yellow);
        Debug.DrawLine(worldCoordinates[1], worldCoordinates[5], Color.yellow);
        Debug.DrawLine(worldCoordinates[2], worldCoordinates[6], Color.yellow);
        Debug.DrawLine(worldCoordinates[3], worldCoordinates[7], Color.yellow);
    }

    private static Vector3 TransformLocalToWorld(Vector3 localPosition, Matrix4x4 localToWorldTransformation)
    {
        return localToWorldTransformation.MultiplyPoint3x4(localPosition);
    }

    private static Matrix4x4 BuildTransformationMatrixFrom(Vector3 position, Vector3 normal, Vector3 forward)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        rotation.SetLookRotation(forward, normal);
        return Matrix4x4.TRS(position, rotation, Vector3.one);
    }

    private static void DrawBaseVectorsAt(Vector3 position, Vector3 right, Vector3 up, Vector3 front)
    {
        Debug.DrawRay(position, up * BaseVectorSize, Color.green);
        Debug.DrawRay(position, right * BaseVectorSize, Color.red);
        Debug.DrawRay(position,  front * BaseVectorSize, Color.cyan);
    }
}
