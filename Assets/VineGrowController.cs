using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class VineGrowController : MonoBehaviour
{
    private float growProgress = 0f;
    private bool isGrowing = false;
    private int currentGrowStage = 0;
    [SerializeField]private List<float> growStages;
    [SerializeField]private Material vineMat;
    [SerializeField]private float growFactor = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
        isGrowing = true;
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
        Debug.Log("progress:" + growProgress);
        if (growProgress >= growStages[currentGrowStage]){
            StopGrow();
        }
    }

    private void Reset(){
        vineMat.SetFloat("_Strength", 0f);
        currentGrowStage = 0;
    }

    private void startGrow(){
        isGrowing = true;
    }

    private void StopGrow(){
        isGrowing = false;
    }

    public void StartNextStage(){
        currentGrowStage++;
        isGrowing = true;
    }
}
