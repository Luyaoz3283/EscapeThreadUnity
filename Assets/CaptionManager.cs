using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptionManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private List<string> CaptionList;
    [SerializeField]private TextMeshProUGUI captionUI;
    private int currentCaptionIndex = 0;
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

    public void HideCaption(){
        captionUI.gameObject.SetActive(false);
    }

    private void Initialize(){
        HideCaption();
    }
}
