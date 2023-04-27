using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class champiAnimation : MonoBehaviour
{
    public float growSpeed = 0.01f; // speed of scale animation
    public Vector3 scaleAmount = new Vector3(1.1f, 1.1f, 1.1f); // amount to scale object when animating
    public float rotationSpeed = 1f; // speed of rotation animation
    public float amplitudeX = 0.1f;
    public float amplitudeY = 0.1f;
    public float amplitudeZ = 0.1f;
    public float scaleAmpX = 0.1f;
    public float scaleAmpY = 0.1f;
    public float scaleAmpZ = 0.1f;


    private float offsetX = 0f;
    private float offsetY = 0f;
    private float offsetZ = 0f;
    

    private bool isColliding = false;

    private void start()
    {
        
    
        isColliding = false;
        // StartCoroutine(ScaleUp(scaleUpDuration)); // scale up on start
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    void OnTriggerEnter (Collider col) 
 {
          if (col.gameObject.CompareTag("rightHandCollider"))
          {
            isColliding = true;
            

          }
 
 }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("rightHandCollider")) // check if collision is with specific collider
        {
            isColliding = false;
        }
    }



    private void Update()
    {
            Quaternion randomRotation = GenerateRandomRotation(amplitudeX, amplitudeY, amplitudeZ, rotationSpeed);
            transform.rotation *= randomRotation;
            Vector3 randomScale = GenerateRandomScale(scaleAmpX, scaleAmpY, scaleAmpZ, rotationSpeed);
            Vector3 currentScale = transform.localScale;
            Vector3 scaleModif = currentScale + randomScale;
            Debug.Log(randomScale);
            // transform.localScale = scaleModif;
            


            // transform.localScale = currentScale + randomScale;

        if (transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(growSpeed, 0, growSpeed);
        }
        if (transform.localScale.y < 1f)
        {
            transform.localScale += new Vector3(0, growSpeed*1.2f, 0);
        }
    
     
        if (isColliding)
        {
            // rotate object
            // float rotationAmount = rotationSpeed * Time.deltaTime;
            // if (reverseRotation) rotationAmount *= -1;
            // scale object
            // Vector3 newScale = transform.localScale;
            // newScale += scaleAmount * scaleSpeed * Time.deltaTime;
            // transform.localScale = newScale;
        }
    }

public static Quaternion GenerateRandomRotation(float amplitudeX, float amplitudeY, float amplitudeZ, float speed)
{
    // if (Time.time == 0)
    // {
    // float offsetX = Random.Range(-amplitudeX, amplitudeX);
    // float offsetY = Random.Range(-amplitudeY, amplitudeY);
    // float offsetZ = Random.Range(-amplitudeZ, amplitudeZ);

    // }

    float time = (Time.time * speed) * 0.01f ;
    float sinX = Mathf.Sin(time) * amplitudeX;
    float sinY = Mathf.Sin(time) * amplitudeY;
    float sinZ = Mathf.Sin(time) * amplitudeY;


    Quaternion rotation = Quaternion.Euler(sinX, sinY, sinZ);

    return rotation;
}

public static Vector3 GenerateRandomScale(float scaleAmpX, float scaleAmpY, float scaleAmpZ, float speed)
{
    // float offsetX = Random.Range(-amplitudeX, amplitudeX);
    // float offsetY = Random.Range(-amplitudeY, amplitudeY);
    // float offsetZ = Random.Range(-amplitudeZ, amplitudeZ);

    float time = (Time.time * speed) * 0.01f ;
    float sinX = Mathf.Sin(time) * scaleAmpX;
    float sinY = Mathf.Sin(time) * scaleAmpY;
    float sinZ = Mathf.Sin(time) * scaleAmpZ;
    float remappedSinX = (sinX + 1f)/10f;
    float remappedSinY = (sinY + 1f)/10f;
    float remappedSinZ = (sinZ + 1f)/10f;

   Vector3 scaleModif = new Vector3(remappedSinX, remappedSinY, remappedSinZ);

    return scaleModif;
}

}

