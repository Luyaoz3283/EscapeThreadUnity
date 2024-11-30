using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField]float rotateSensitivity = 1f;
    [SerializeField]float moveSensitivity = 1f;
    
    [SerializeField]float stopDistance = 2f;
    [SerializeField]float stopAngle = 1f;
    Vector3 newPosition;
    Vector3 finalLookVector;
    Quaternion targetRotation;
    Camera cam;
    private bool allowFreeMovement = true;
    // Start is called before the first frame update
    void Start()
    {
        SetMoveGoal(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowFreeMovement) return;

        Move();
        Rotate();
        
        //detect click
        if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){

				GameObject victim = hit.collider.gameObject;
				Debug.Log(hit.collider.gameObject+ "tag:" + victim.tag);
				if(victim.tag == "viableLocation")
				{
                    Debug.Log("setting new goal-1");
                    SetMoveGoal(victim.transform.position);
				}
			}

		}
    }

    void SetMoveGoal(Vector3 inPos){
        Debug.Log("setting new goal");
        newPosition = inPos;
        Vector3 currentPosition = transform.position;
        finalLookVector = newPosition - currentPosition;
        targetRotation = Quaternion.FromToRotation(Vector3.forward, finalLookVector);
    }

    void Rotate()
    {
        //rotation
        Vector3 currentLookDir = transform.forward;    
        float angleNeedsToRotate = Vector3.Angle(currentLookDir, finalLookVector);
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
