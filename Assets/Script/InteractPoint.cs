using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractPoint : MonoBehaviour
{
    private  PlayerMoveController playerMoveController;

    [SerializeField]private UnityEvent actionList;
    [SerializeField]private bool shouldDisplayText;
    [SerializeField]private string textToDisplay;

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
        actionList.Invoke();
        if (shouldDisplayText){
            Debug.Log("requsting displaying text");
            CaptionManager.DisplayCaption(textToDisplay);
        }
    }

    private void OnReachedDestination()
    {
        this.gameObject.SetActive(false);
        playerMoveController.EventReachedDestination -= OnReachedDestination;
        Debug.Log("Event Received!");
        // Add custom logic here
    }

    public void RequestMoving()
    {
        bool requestAccepted = playerMoveController.RequestMoveToNewGoal(this.transform.position, this.transform.forward);
        if (requestAccepted){
            playerMoveController.EventReachedDestination += OnReachedDestination;
        }
    }

    public void RequestGrowVine(){
        VineGrowController.startGrow();
    }
}
