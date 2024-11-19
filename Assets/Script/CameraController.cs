using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool isMoving = false;
    public float speed = 20f;           // Forward movement speed
    public float bobSpeed = 14f;      // Speed of head bobbing
    public float bobAmount = 0.05f;   // Amplitude of head bobbing
    public float tiltAmount = 2f;     // Camera tilt on simulated turns
    public float tiltSpeed = 5f;      // Tilt smoothness

    private Vector3 initialPosition;  // Initial camera position for head bobbing
    private float bobTimer = 0f;      // Timer for bobbing effect

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (isMoving){
            SimulateHeadBobbing();
            SimulateCameraTilt();
            SimulateForwardMovement();
        }
    }

    private void SimulateForwardMovement()
    {
        // Move the character forward
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void SimulateHeadBobbing()
    {
        // Apply head bobbing if moving
        bobTimer += bobSpeed * Time.deltaTime;
        float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
        transform.localPosition = transform.localPosition + new Vector3(0, bobOffset, 0);
    }

    private void SimulateCameraTilt()
    {
        // Simulate camera tilt to add realism
        float tilt = Mathf.Sin(bobTimer * 0.5f) * tiltAmount; // Simulate slight sway
        Quaternion targetRotation = Quaternion.Euler(0, 0, -tilt);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
    }

    public void StartMoving(){
        isMoving = true;
    }
}
