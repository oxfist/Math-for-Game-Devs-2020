using System.Collections.Generic;
using UnityEngine;

public class SurfaceAreaCalculatorEditor : MonoBehaviour
{
    [SerializeField] private Mesh mesh;

    private void OnDrawGizmos()
    {
        float meshArea = AreaCalculator(mesh);
    }

    private static float AreaCalculator(Mesh mesh)
    {
        if (mesh == null)
        {
            return 0;
        }

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        float totalMeshArea = 0f;

        for (int i = 0; i < TriangleAmountIn(mesh); i++)
        {
            totalMeshArea += TriangleArea(vertices, triangles, i);
        }

        return totalMeshArea;
    }

    private static float TriangleArea(IReadOnlyList<Vector3> vertices, IReadOnlyList<int> triangles, int triangleNumber)
    {
        int baseIndex = triangleNumber * 3;
        Vector3 first = vertices[triangles[baseIndex]];
        Vector3 second = vertices[triangles[baseIndex + 1]];
        Vector3 third = vertices[triangles[baseIndex + 2]];

        Vector3 firstSide = second - first;
        Vector3 secondSide = third - second;

        return Vector3.Cross(firstSide, secondSide).magnitude / 2;
    }

    private static int TriangleAmountIn(Mesh mesh)
    {
        return mesh.triangles.Length / 3;
    }
}
