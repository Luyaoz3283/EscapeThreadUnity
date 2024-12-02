using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInManager : MonoBehaviour
{
    SceneTransition sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("new scene starting");
        sceneManager = GameObject.FindAnyObjectByType<SceneTransition>();
        sceneManager.TriggerSceneFadeIn();
        //Camera cam = GameObject.FindAnyObjectByType<Camera>();
        //cam.GetComponent<Animator>().SetBool("StartMoving", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
