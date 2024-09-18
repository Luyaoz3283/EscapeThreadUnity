using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceManager : MonoBehaviour
{
    [SerializeField]private GameObject sheet;
    [SerializeField]private RenderTexture previousTexture;
    private Material sheetMaterial;
    // Start is called before the first frame update
    void Start()
    {
        //initial setup
        sheetMaterial = sheet.GetComponent<Renderer>().material;

        // Initialize render texture
        sheetMaterial.SetFloat("_HasInitialized", 0);
        sheetMaterial.SetTexture("_PreviousFrame", previousTexture);
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isClothPressed = false;
        sheetMaterial.SetInt("_MousePressed", 1);
        if (Input.GetMouseButton(0))
        {
             // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Variable to store the hit information
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("cloth"))
            {
                // Get the world position where the ray hits
                isClothPressed = true;
                Vector3 worldPosition = hit.point;
                float [] array = new float[] { worldPosition.x, worldPosition.y, worldPosition.z, 0.0f};
                Vector4 slicePlane = new Vector4(worldPosition.x, worldPosition.y, worldPosition.z, 0); 
                sheetMaterial.SetInt("_MousePressed", 1);
                sheetMaterial.SetVector("_SlicePosition", slicePlane);
                sheetMaterial.SetFloat("_HasInitialized", 1);
                Debug.Log("Mouse points at world position: ");
                //Debug.Log("Hit target: " + hit.collider.gameObject.name);
            }
            else
            {
                //Debug.Log("Raycast did not hit any object.");
            }
        }
        
        // if (!isClothPressed)
        // {
        //     sheetMaterial.SetFloat("_MousePressed", 0.0f); 
        // }
        sheetMaterial.SetInt("_MousePressed", 1);
    }
}
