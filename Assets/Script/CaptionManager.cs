using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptionManager : MonoBehaviour
{
    public static CaptionManager Instance { get; private set; }
    [SerializeField]private List<string> CaptionList;
    [SerializeField]private TextMeshProUGUI captionUI;
    private int currentCaptionIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCaption(int newIndex){
        captionUI.gameObject.SetActive(true);
        captionUI.text = CaptionList[newIndex];
    }

    public static void DisplayCaption(string newTextString){
        Debug.Log("displaying text");
        if (Instance != null && Instance.captionUI != null)
        {
            Debug.Log("displaying text - 1");
            Instance.captionUI.gameObject.SetActive(true);
            Instance.captionUI.text = newTextString;
        }  
    }

    public void HideCaption(){
        captionUI.gameObject.SetActive(false);
    }

    private void Initialize(){
        HideCaption();
    }
}
