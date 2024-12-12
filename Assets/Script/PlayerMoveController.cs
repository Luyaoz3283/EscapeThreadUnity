using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;
using UnityEditor.Rendering;

public class PlayerMoveController : MonoBehaviour
{
    public event Action EventReachedDestination;
    [SerializeField]float rotateSensitivity = 1f;
    [SerializeField]float moveSensitivity = 1f;
    
    [SerializeField]float stopDistance = 2f;
    [SerializeField]float stopAngle = 1f;
    private CaptionManager captionManager;
    
    Camera cam;
    private bool allowFreeMovement = true;
    private bool isRotating = false;
    private bool isPositionaing = false;
    private Vector3 newPosition;
    private Vector3 newRotation;
    private Quaternion targetRotation;
    private float distance;
    private Vector3 entireDistanceVector;
    private float entireRotationVector;
    // Start is called before the first frame update
    void Start()
    {
        captionManager = GameObject.FindFirstObjectByType<CaptionManager>();
        //initialize location
        newPosition = transform.position;
        Vector3 currentPosition = transform.position;
        newRotation = newPosition - currentPosition;
        targetRotation = Quaternion.FromToRotation(Vector3.forward, newRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowFreeMovement) return;

        if (isPositionaing) Move();
        if (isRotating) Rotate();
        if (!isRotating && !isPositionaing) 
        {
            EventReachedDestination?.Invoke(); 
        }
    }

    public bool RequestMoveToNewGoal(Vector3 newPos, Vector3 newRot){
        if (isPositionaing || isRotating) {
            Debug.Log("is moving");
            return false;
        }
        isPositionaing = true;
        isRotating = true;
        newPosition = newPos;
        newRotation = newRot;
        entireDistanceVector = newPosition - transform.position;
        entireRotationVector = Vector3.Angle(transform.forward, newRotation);
        targetRotation = Quaternion.FromToRotation(Vector3.forward, newRotation);
        //captionManager.DisplayCaption(0);
        Debug.Log("trigger moving");
        return true;
    }

    void Rotate()
    {
        float secondUse = entireDistanceVector.magnitude / moveSensitivity;
        //rotation
        Vector3 currentLookDir = transform.forward;    
        float currentAngleLeft = Vector3.Angle(currentLookDir, newRotation);
        float speed = entireRotationVector / secondUse; 
        Debug.Log("angle needs to rotate:" + currentAngleLeft);
        if (currentAngleLeft <= stopAngle){
            isRotating = false;
            return;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
        targetRotation, rotateSensitivity * Time.deltaTime * speed);
    }

    void Move(){
        //position
        Debug.Log("distance:" + entireDistanceVector.magnitude);
        float currentDistance = (transform.position - newPosition).magnitude;
        if (currentDistance < stopDistance){
            isPositionaing = false;
            return;
        }
        Vector3 moveStep = entireDistanceVector.normalized * moveSensitivity * Time.deltaTime;
        transform.position = transform.position + moveStep;
    }
}
