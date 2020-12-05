using System.Collections.Generic;
using UnityEngine;

public class TurretPlacerEditor : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.green;

    private readonly Vector3[] _wireframeCubeLocalCoordinates = {
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
            DrawWireFrameCubeAt(_wireframeCubeLocalCoordinates, localToWorldTransformation);
        }
    }

    private static void DrawWireFrameCubeAt(IReadOnlyList<Vector3> cubeCoordinates, Matrix4x4 localToWorldTransformation)
    {
        // Bottom sides
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[0]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[1]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[1]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[2]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[2]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[3]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[3]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[0]), Color.yellow);

        // Top sides
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[4]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[5]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[5]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[6]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[6]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[7]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[7]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[4]), Color.yellow);

        // Vertical sides
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[0]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[4]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[1]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[5]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[2]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[6]), Color.yellow);
        Debug.DrawLine(localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[3]), localToWorldTransformation.MultiplyPoint3x4(cubeCoordinates[7]), Color.yellow);
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
