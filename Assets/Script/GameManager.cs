using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sheet;
    public ParticleSystem firstParticle;  // Assign the first particle system in the inspector
    public ParticleSystem secondParticle; // Assign the second particle system in the inspector
    public float delay = 2f;
    public float speed = 1.0f;

    private Material sheetMaterial;
    private float startStrength = 1.0f;
    private float currentStrength;
    private bool isStarted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //press t
        //reduce sheet Strength
        //start particle 1
        //start particle 2
        sheetMaterial = sheet.GetComponent<Renderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        { 
            isStarted = true;
            currentStrength = startStrength;
            // Start the first particle system
            firstParticle.Play();

            // Start the coroutine to delay the second particle system
            StartCoroutine(StartSecondParticleAfterDelay());
            
        }
        if (isStarted){
            sheetMaterial.SetFloat("_Strength", currentStrength);
            currentStrength -= Time.deltaTime * speed;
            currentStrength = Mathf.Clamp(currentStrength, -1, 1);
        }
        
    }

    IEnumerator StartSecondParticleAfterDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Start the second particle system
        secondParticle.Play();
    }
}
