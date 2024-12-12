using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInManager : MonoBehaviour
{
    private SceneTransition sceneManager;
    private CaptionManager captionManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("new scene starting");
        sceneManager = GameObject.FindAnyObjectByType<SceneTransition>();
        sceneManager.TriggerSceneFadeIn();
        captionManager = GameObject.FindAnyObjectByType<CaptionManager>();
        Invoke("DisplayCaption", 3f);
        
        //Camera cam = GameObject.FindAnyObjectByType<Camera>();
        //cam.GetComponent<Animator>().SetBool("StartMoving", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayCaption(){
        //captionManager.DisplayCaption(0);
    }
}
