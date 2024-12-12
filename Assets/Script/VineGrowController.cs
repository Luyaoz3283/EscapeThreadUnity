using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class VineGrowController : MonoBehaviour
{
    public static VineGrowController Instance { get; private set; }
    private float growProgress = 0f;
    private bool isGrowing = false;
    private int currentGrowStage = 0;
    [SerializeField]private List<float> growStages;
    [SerializeField]private Material vineMat;
    [SerializeField]private float growFactor = 1f;
    
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
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Grow();
    }

    private void Grow(){
        if (!isGrowing) return;
        growProgress += Time.deltaTime*growFactor;
        vineMat.SetFloat("_Grow", growProgress);
        //Debug.Log("progress:" + growProgress);
        if (growProgress >= growStages[currentGrowStage]){
            StopGrow();
        }
    }

    private void Reset(){
        vineMat.SetFloat("_Strength", 0f);
        vineMat.SetFloat("_Grow", 0f);
        currentGrowStage = 0;
    }

    

    private void StopGrow(){
        isGrowing = false;
    }

    public static void startGrow(){
        Instance.isGrowing = true;
    }

    public static void StartNextStage(){
        Instance.currentGrowStage++;
        Instance.isGrowing = true;
    }
}
