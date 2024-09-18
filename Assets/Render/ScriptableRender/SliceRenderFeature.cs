using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomSettings
    {
        public Material material; // Assign the material with the shader in the inspector
        public RenderTexture renderTexture; // Assign the RenderTexture in the inspector
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques; // Choose when this pass executes
    }

    public CustomSettings settings = new CustomSettings();
    private CustomRenderPass customRenderPass;

    public override void Create()
    {
        // Initialize the custom render pass with the material and RenderTexture
        if (settings.material != null && settings.renderTexture != null)
        {
            customRenderPass = new CustomRenderPass(settings.material, settings.renderTexture)
            {
                renderPassEvent = settings.renderPassEvent // Set when this pass will execute
            };
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (customRenderPass != null)
        {
            // Add the custom render pass to the renderer
            renderer.EnqueuePass(customRenderPass);
        }
    }
}
