using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractPoint : MonoBehaviour
{
    private  PlayerMoveController playerMoveController;
    // Start is called before the first frame update
    void Start()
    {
        playerMoveController = GameObject.FindAnyObjectByType<PlayerMoveController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("Object clicked!");
        bool requestAccepted = playerMoveController.RequestMoveToNewGoal(this.transform.position, this.transform.forward);
        if (requestAccepted){
            playerMoveController.EventReachedDestination += OnReachedDestination;
        }
        
    }

    private void OnReachedDestination()
    {
        this.gameObject.SetActive(false);
        playerMoveController.EventReachedDestination -= OnReachedDestination;
        Debug.Log("Event Received!");
        // Add custom logic here
    }
}
