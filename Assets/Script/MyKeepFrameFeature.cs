using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// This renderer feature replicates a "don't clear" behavior by injecting two passes into the pipeline:
// 1. A pass that copies the color buffer at the end of a frame.
// 2. A pass that draws the content of the copied texture at the beginning of the next frame.
public class MyKeepFrameFeature : ScriptableRendererFeature
{
    // This pass is responsible for copying the current frame's color to a specified destination texture.
    class CopyFramePass : ScriptableRenderPass
    {
        private RTHandle source { get; set; }  // The source RTHandle from which the color will be copied.
        private RTHandle destination { get; set; } // The destination RTHandle where the color will be copied to.

        // Sets up the source and destination handles for the copy operation.
        public void Setup(RTHandle source, RTHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        // Executes the copy operation using a command buffer.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Only proceed if the camera is the main game camera.
            if (renderingData.cameraData.camera.cameraType != CameraType.Game)
                return;

            // Create a command buffer to handle the blit (copy) operation.
            CommandBuffer cmd = CommandBufferPool.Get("CopyFramePass");
            // Copies the content from the source to the destination using a Blit operation.
            Blit(cmd, source, destination);

            // Executes the command buffer and then releases it.
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    // This pass is responsible for drawing the previously copied frame data to a full-screen quad.
    class DrawOldFramePass : ScriptableRenderPass
    {
        private Material m_DrawOldFrameMaterial; // Material used for drawing the old frame.
        private RTHandle m_Handle; // The handle to the previously copied frame.
        private string m_TextureName; // The name of the texture used in the shader.

        // Sets up the material, handle, and texture name for this pass.
        public void Setup(Material drawOldFrameMaterial, RTHandle handle, string textureName)
        {
            m_DrawOldFrameMaterial = drawOldFrameMaterial;
            m_TextureName = textureName;
            m_Handle = handle;
        }

        // Executes the draw operation using a command buffer.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Only proceed if the material is not null.
            if (m_DrawOldFrameMaterial != null)
            {
                // Create a command buffer for drawing the old frame.
                CommandBuffer cmd = CommandBufferPool.Get("DrawOldFramePass");
                // Set the global texture used by the material to the previously copied frame.
                cmd.SetGlobalTexture(m_TextureName, m_Handle);

                // Get the camera's current color target handle.
                var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

                // Determine the viewport scale based on whether scaling is used on the source.
                Vector2 viewportScale = source.useScaling ? 
                    new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) 
                    : Vector2.one;

                // Blit the old frame texture onto the source target using the material, rendering a full-screen quad.
                Blitter.BlitTexture(cmd, source, viewportScale, m_DrawOldFrameMaterial, 0);

                // Executes the command buffer and then releases it.
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }
    }

    [Serializable]
    public class Settings
    {
        [Tooltip("The material that is used when the old frame is redrawn at the start of the new frame (before opaques).")]
        public Material displayMaterial; // The material used to display the previous frame's data.
        
        [Tooltip("The name of the texture used for referencing the copied frame. (Defaults to _FrameCopyTex if empty)")]
        public string textureName; // The name of the texture for the copied frame; defaults to "_FrameCopyTex" if not set.
    }

    private CopyFramePass m_CopyFrame; // Instance of the copy frame pass.
    private DrawOldFramePass m_DrawOldFrame; // Instance of the draw old frame pass.

    private RTHandle m_OldFrameHandle; // Handle for the render texture that stores the old frame.

    public Settings settings = new Settings(); // Settings instance for configuring this feature.

    // Property to expose the RenderTexture for use outside the script
    public RenderTexture OldFrameRenderTexture { get; private set; }

    // In this function, the passes are created and their point of injection in the rendering pipeline is set.
    public override void Create()
    {
        m_CopyFrame = new CopyFramePass();
        // Set the render pass event to after all transparents are rendered, to capture the final color buffer.
        m_CopyFrame.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        m_DrawOldFrame = new DrawOldFramePass();
        // Set the render pass event to before opaques are rendered, to draw the old frame early in the new frame.
        m_DrawOldFrame.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    }

    // Enqueues the render passes to the renderer.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_CopyFrame); // Adds the copy frame pass.
        renderer.EnqueuePass(m_DrawOldFrame); // Adds the draw old frame pass.
    }

    // Configures the render passes with the necessary render textures and material.
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        // Create the render texture descriptor based on the camera's target settings.
        var descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.msaaSamples = 1; // Set multi-sample anti-aliasing (MSAA) samples to 1.
        descriptor.depthBufferBits = 0; // Disable depth buffer for this render texture.

        // Set the texture name, defaulting to "_FrameCopyTex" if none is specified in the settings.
        var textureName = String.IsNullOrEmpty(settings.textureName) ? "_FrameCopyTex" : settings.textureName;

        // Allocate the render texture if needed, using the descriptor settings.
        RenderingUtils.ReAllocateIfNeeded(ref m_OldFrameHandle, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: textureName);

        // Create a RenderTexture from the RTHandle for external use (e.g., in a RawImage).
        OldFrameRenderTexture = m_OldFrameHandle.rt; // Access the underlying RenderTexture from the RTHandle.

        // Configure the draw old frame pass to not clear the buffer.
        m_DrawOldFrame.ConfigureClear(ClearFlag.None, Color.red); 

        // Setup the copy frame pass with the camera's current color target and the old frame handle.
        m_CopyFrame.Setup(renderer.cameraColorTargetHandle, m_OldFrameHandle);
        // Setup the draw old frame pass with the material, old frame handle, and texture name.
        m_DrawOldFrame.Setup(settings.displayMaterial, m_OldFrameHandle, textureName);
    }

    // Cleans up the resources used by this feature when it is disposed of.
    protected override void Dispose(bool disposing)
    {
        m_OldFrameHandle?.Release(); // Release the render texture handle if it exists.
        OldFrameRenderTexture = null; // Nullify the exposed render texture to avoid accessing released resources.
    }
}
