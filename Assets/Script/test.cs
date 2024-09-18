using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class test : MonoBehaviour
{
    public ScriptableRendererFeature MykeepFrameFeature; // Reference to the MyKeepFrameFeature instance
    public RawImage rawImage; // Reference to the RawImage where the RenderTexture will be displayed

    void Start()
    {
        // Find the MyKeepFrameFeature in the Renderer Features list of the active Render Pipeline Asset
        MyKeepFrameFeature feature = MykeepFrameFeature as MyKeepFrameFeature;

        if (feature != null && rawImage != null)
        {
            // Assign the RenderTexture from MyKeepFrameFeature to the RawImage
            rawImage.texture = feature.OldFrameRenderTexture;
        }
        else
        {
            Debug.LogError("MyKeepFrameFeature or RawImage reference is missing.");
        }
    }

    void Update()
    {
        // Optionally update the RawImage texture every frame if needed
        MyKeepFrameFeature feature = MykeepFrameFeature as MyKeepFrameFeature;
        if (feature != null && rawImage != null)
        {
            rawImage.texture = feature.OldFrameRenderTexture;
        }
    }
}
