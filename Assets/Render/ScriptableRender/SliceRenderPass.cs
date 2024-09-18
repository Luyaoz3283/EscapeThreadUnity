using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPass : ScriptableRenderPass
{
    private Material material;
    private RenderTargetIdentifier renderTextureIdentifier;
    private RenderTexture renderTexture;

    public CustomRenderPass(Material material, RenderTexture renderTexture)
    {
        this.material = material;
        this.renderTexture = renderTexture;
        renderTextureIdentifier = new RenderTargetIdentifier(renderTexture);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Custom Render Pass");

        // Set the RenderTarget to the custom RenderTexture
        cmd.SetRenderTarget(renderTextureIdentifier);

        // Clear the RenderTexture
        cmd.ClearRenderTarget(true, true, Color.clear);

        // Draw a full-screen quad with the custom material
        cmd.Blit(null, renderTextureIdentifier, material);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    // Optionally override Configure, FrameCleanup, or other methods as needed.
}
