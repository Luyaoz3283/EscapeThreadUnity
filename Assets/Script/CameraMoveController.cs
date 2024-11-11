using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [SerializeField]float rotateSensitivity = 1f;
    [SerializeField]float moveSensitivity = 1f;
    
    [SerializeField]GameObject newObj;
    [SerializeField]float stopDistance = 2f;
    [SerializeField]float stopAngle = 1f;
    Vector3 newPosition;
    Vector3 finalLookVector;
    Quaternion targetRotation;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        StartMoving(new Vector3(40f,40f,3.2f));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        
    }

    void StartMoving(Vector3 inPos){
        newPosition = newObj.transform.position;
        Vector3 currentPosition = transform.position;
        finalLookVector = newPosition - currentPosition;
        targetRotation = Quaternion.FromToRotation(Vector3.forward, finalLookVector);
    }

    void Rotate()
    {
        //rotation
        Vector3 currentLookDir = transform.forward;    
        float angleNeedsToRotate = Vector3.Angle(currentLookDir, finalLookVector);
        // Debug.Log("currentLookDir: " + currentLookDir);
        // Debug.Log("finalLookVector: " + finalLookVector);
        // Debug.Log("target rotation: " + targetRotation.eulerAngles);
        // Debug.Log("angle needs to rotate: " + angleNeedsToRotate);
        if (angleNeedsToRotate <= stopAngle){
            return;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
        targetRotation, rotateSensitivity * Time.deltaTime * angleNeedsToRotate/30f);
    }

    void Move(){
        //position
        Vector3 currentPosition = transform.position;
        Vector3 finalLookVector = newPosition - currentPosition;
        if (finalLookVector.magnitude < stopDistance){
            return;
        }
        Vector3 moveStep = finalLookVector * moveSensitivity * Time.deltaTime;
        transform.position = transform.position + moveStep;
    }
}
