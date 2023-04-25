using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceGenerator : MonoBehaviour
{
    public GameObject[] models; // Array of low-poly natural models to be generated
    public int numModels = 10; // Number of models to be generated
    public float modelScale = 1f; // Scale of the generated models
    public float modelPlacementRadius = 5f; // Radius around each generated point where models can be placed

    private Mesh surfaceMesh; // The mesh of the surface
    private Vector3[] meshVertices; // Array of vertices of the surface mesh
    private Vector3[] meshNormals; // Array of normals of the surface mesh

    void Start()
    {
        surfaceMesh = GetComponent<MeshFilter>().mesh;
        meshVertices = surfaceMesh.vertices;
        meshNormals = surfaceMesh.normals;

        for (int i = 0; i < numModels; i++)
        {
            Vector3 randomPoint = GetRandomPointOnSurface();
            Vector3 modelRotation = GetSurfaceNormalAtPoint(randomPoint);

            int modelIndex = Random.Range(0, models.Length);
            GameObject newModel = Instantiate(models[modelIndex], randomPoint, Quaternion.identity);
            newModel.transform.localScale = new Vector3(modelScale, modelScale, modelScale);
            newModel.transform.rotation = Quaternion.FromToRotation(Vector3.up, modelRotation);
        }
    }

    // Returns a random point on the surface within a specified radius
    Vector3 GetRandomPointOnSurface()
    {
        Vector3 randomPoint = meshVertices[Random.Range(0, meshVertices.Length)];
        randomPoint += meshNormals[Random.Range(0, meshNormals.Length)] * Random.Range(0f, modelPlacementRadius);

        return transform.TransformPoint(randomPoint);
    }

    // Returns the surface normal at a specified point
    Vector3 GetSurfaceNormalAtPoint(Vector3 point)
    {
        Vector3 localPos = transform.InverseTransformPoint(point);
        int[] triangles = surfaceMesh.triangles;
        Vector3[] normals = surfaceMesh.normals;
        Vector3 normal = Vector3.zero;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v1 = transform.TransformPoint(meshVertices[triangles[i]]);
            Vector3 v2 = transform.TransformPoint(meshVertices[triangles[i + 1]]);
            Vector3 v3 = transform.TransformPoint(meshVertices[triangles[i + 2]]);

            if (IsPointInTriangle(localPos, v1, v2, v3))
            {
                normal = transform.TransformDirection(normals[surfaceMesh.triangles[i]]);
                break;
            }
        }

        return normal;
    }

    // Returns true if a point is inside a triangle in 3D space
    bool IsPointInTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 e1 = p2 - p1;
        Vector3 e2 = p3 - p1;
        Vector3 n = Vector3.Cross(e1, e2);

        float dot = Vector3.Dot(n, p - p1);

        if (dot < 0)
        {
            return false;
        }

        Vector3 q = Vector3.Cross(e1, p - p1);

        dot = Vector3.Dot(n, q);

        if (dot < 0)
        {
            return false;
        }

        return true;
    }
}
