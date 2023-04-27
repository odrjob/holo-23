using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastGenerator : MonoBehaviour
{
    public GameObject[] groundModels; // an array of model prefabs to be generated
    public GameObject[] wallModels; // an array of model prefabs to be generated
    public GameObject[] ceilingModels; // an array of model prefabs to be generated

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 point = Random.insideUnitSphere* 5 + transform.position;
        Vector3 direction = Random.onUnitSphere;
        tryCreateObject(point, direction);
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
            if (hit.collider.gameObject.CompareTag("notWall"))
            {
            Debug.Log("ce ray cast a touché un objet qui n'est pas un mur");
            }
            

            if (!hit.collider.gameObject.CompareTag("notWall"))
            {
                GameObject selectedModel = null;

                bool isGroundModel = Vector3.Angle(hit.normal, Vector3.up) < 10f;
                bool isCeilingModel = Vector3.Angle(hit.normal, Vector3.down) < 10f;
                bool isWall1Model = Vector3.Angle(hit.normal, Vector3.left) < 10f;
                bool isWall2Model = Vector3.Angle(hit.normal, Vector3.right) < 10f;
                // randomly select a model from the array and generate it at the intersection point
                if(isGroundModel){
                int randomModelIndex = Random.Range(0, groundModels.Length);
                selectedModel = groundModels[randomModelIndex];
                }
                if(isCeilingModel){
                int randomModelIndex = Random.Range(0, ceilingModels.Length);
                selectedModel = ceilingModels[randomModelIndex];
                }
                if(isWall1Model || isWall2Model){
                int randomModelIndex = Random.Range(0, wallModels.Length);
                selectedModel = wallModels[randomModelIndex];
                }
            

                if(selectedModel!=null){
                    
                Quaternion surfaceNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);
                GameObject newModel = Instantiate(selectedModel, hit.point, surfaceNormal);
                
                return newModel;
                }else{
                }
            }
        }
        return null;
    }

}

