using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastGeneratorOld : MonoBehaviour
{
    public GameObject[] groundModels; // an array of model prefabs to be generated
    public GameObject[] newModels; // an array of model prefabs to be generated
    public float radius = 0f; // the minimum distance between generated models
    public float scaleModifier = 0.5f; // the scale modifier of the generated models
    public GameObject surfaceObject;
    private Vector3[] meshVertices;
    private Vector3[] meshNormals;
    private int[] meshTriangles;
    private Mesh surfaceMesh;

    void Start()
    {
        // get the surface mesh from the attached object
        MeshFilter meshFilter = surfaceObject.GetComponent<MeshFilter>();
        

        surfaceMesh = meshFilter.mesh;
        meshVertices = surfaceMesh.vertices;
        meshTriangles = surfaceMesh.triangles;
        meshNormals = surfaceMesh.normals;
    }

    void Update()
    {
        Vector3 point = Random.insideUnitSphere * 5 + transform.position;
        Vector3 direction = Random.onUnitSphere;
        tryCreateObject(point, direction);
    }






    bool CheckValidLocation(Vector3 point)
    {
        // check if the point is within the radius of another model
        return true;
        foreach (GameObject newModel in newModels)
        {
            if (Vector3.Distance(point, newModel.transform.position) < radius)
            {
                return false;
            }
        }
    }



    Vector3 GetPointOnMesh(Vector3 point)
    {
        // get a random point on the mesh
        Vector3 randomPoint = meshVertices[Random.Range(0, meshVertices.Length)];
        // Debug.Log(randomPoint);

        return surfaceObject.transform.TransformPoint(randomPoint);
    }


    GameObject tryCreateObject(Vector3 point, Vector3 direction)
    {
        // cast a ray from the camera to the point on the mesh
        Ray ray = new Ray(point, direction);
        RaycastHit hit;

        // check if the ray hits the mesh
        if (Physics.Raycast(ray, out hit))
        {
            // check if the intersection point is a valid location for a new model
          
            if (hit.collider.gameObject.CompareTag("wall"))
            {
                GameObject selectedModel = null;

                bool isGroundModel = Vector3.Angle(hit.normal, Vector3.up) < 10f;
                // randomly select a model from the array and generate it at the intersection point
                if(isGroundModel){
                int randomModelIndex = Random.Range(0, groundModels.Length);
                selectedModel = groundModels[randomModelIndex];
                }

                if(selectedModel!=null){
                    
                Quaternion surfaceNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);
                GameObject newModel = Instantiate(selectedModel, hit.point, surfaceNormal);
                
                return newModel;
                }else{
                    Debug.Log("selected model is null");
                }
            }
        }
        return null;
    }
    // Vector3 GetSurfaceNormalAtPoint(Vector3 point)
    // {
    //     Vector3 localPos = surfaceObject.transform.InverseTransformPoint(point);
    //     int[] triangles = surfaceMesh.triangles;
    //     Vector3[] normals = surfaceMesh.normals;
    //     Vector3 normal = Vector3.zero;

    //     for (int i = 0; i < triangles.Length; i += 3)
    //     {
    //         Vector3 v1 = surfaceObject.transform.TransformPoint(meshVertices[triangles[i]]);
    //         Vector3 v2 = surfaceObject.transform.TransformPoint(meshVertices[triangles[i + 1]]);
    //         Vector3 v3 = surfaceObject.transform.TransformPoint(meshVertices[triangles[i + 2]]);
    //         Debug.Log(triangles.Length);
    //         if (IsPointInTriangle(localPos, v1, v2, v3))
    //         {
    //             Debug.Log(triangles.Length);
    //             if(i >triangles.Length){
    //                 Debug.Log("i is greater than triangles length");
    //             }
    //             normal = surfaceObject.transform.TransformDirection(normals[surfaceMesh.triangles[i]]);
                
    //             break;
    //         }
    //     }

    //     return normal;
    // }


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

