using UnityEngine;

public class SurfaceAreaCalculatorEditor : MonoBehaviour
{
    [SerializeField] private Mesh mesh;

    private void OnDrawGizmos()
    {
        float meshArea = AreaCalculator(mesh);
        float meshVolume = VolumeCalculator(mesh);
        //Debug.Log(meshVolume);
    }

    private static float VolumeCalculator(Mesh mesh)
    {
        if (mesh == null)
        {
            return 0;
        }

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        float totalMeshVolume = 0f;

        for (int i = 0; i < TriangleAmountIn(mesh); i++)
        {
            int baseIndex = i * 3;
            Vector3 first = vertices[triangles[baseIndex]];
            Vector3 second = vertices[triangles[baseIndex + 1]];
            Vector3 third = vertices[triangles[baseIndex + 2]];

            Debug.DrawLine(first, second);
            Debug.DrawLine(second, third);
            Debug.DrawLine(third, first);

            totalMeshVolume += PyramidVolume(first, second, third);
        }

        return totalMeshVolume;
    }

    private static float PyramidVolume(Vector3 first, Vector3 second, Vector3 third)
    {
        //Debug.DrawLine(Vector3.zero, first);
        //Debug.DrawLine(Vector3.zero, second);
        //Debug.DrawLine(Vector3.zero, third);

        Vector3 triangleNormal = Vector3.Cross(second - first, third - second);

        float parallelepipedVolume = Vector3.Dot(first, triangleNormal);
        //Debug.DrawLine(Vector3.zero, triangleNormal * parallelepipedVolume, Color.green);

        return Mathf.Abs(parallelepipedVolume) / 6f;
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
            int baseIndex = i * 3;
            Vector3 first = vertices[triangles[baseIndex]];
            Vector3 second = vertices[triangles[baseIndex + 1]];
            Vector3 third = vertices[triangles[baseIndex + 2]];

            totalMeshArea += TriangleArea(first, second, third);
        }

        return totalMeshArea;
    }

    private static float TriangleArea(Vector3 first, Vector3 second, Vector3 third)
    {
        Vector3 firstSide = second - first;
        Vector3 secondSide = third - second;

        return Vector3.Cross(firstSide, secondSide).magnitude / 2;
    }

    private static int TriangleAmountIn(Mesh mesh)
    {
        return mesh.triangles.Length / 3;
    }
}
