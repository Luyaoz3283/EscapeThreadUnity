Shader "Unlit/UnlitSlice"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    { 
        _MainTex("Texture", 2D) = "white" {}                   // Main texture
        _PreviousFrame("Previous Frame", 2D) = "white" {} // Render texture from the previous frame
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)          // Base color of the shader
        _SliceColor("Slice Color", Color) = (1, 1, 1, 1)          // Base color of the shader
        _SliceThickness ("Slice Thickness", Float) = 1.0 // Float property
    }

    // The SubShader block containing the Shader code.
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader.
            #pragma vertex vert
            // This line defines the name of the fragment shader.
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;
                // The uv variable contains the UV coordinate on the texture for the
                // given vertex.
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
                // The uv variable contains the UV coordinate on the texture for the
                // given vertex.
                float2 uv           : TEXCOORD0;
                float3 worldPos : TEXCOORD1;                // World position passed to the fragment shader
            };

            TEXTURE2D(_MainTex);                             // Main texture
            SAMPLER(sampler_MainTex);                        // Texture sampler
            TEXTURE2D(_PreviousFrame);                             // Main texture
            SAMPLER(sampler_PreviousFrame);                        // Texture sampler

            float4 _BaseColor;                               // Base color property
            float4 _SliceColor;                              // Color of the slice effect
            float _SliceThickness;                           // Thickness of the slice visual effect
            float4 _SlicePosition;                           // Position of the slice plane in world space
            int _MousePressed;                           // Thickness of the slice visual effect
            int _HasInitialized = 0;
            // The vertex shader definition with properties defined in the Varyings
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous clip space.
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;                                          // Pass UVs to the fragment shader
                OUT.worldPos = TransformObjectToWorld(IN.positionOS).xyz; // Transform to world space position
                return OUT;
            }

            // Fragment shader: defines the pixel color
            
            

            // The fragment shader definition.
            half4 frag(Varyings IN) : SV_Target
            {
                // Calculate a time-based modifier using a sine wave
                float timeModifier = sin(_Time.y); // Oscillates between -1 and 1
                
                // Scale to oscillate between 0 and 1
                timeModifier = (timeModifier + 1) / 2.0;

                //return half4(0.0, 1.0, 0.0, 1.0); // Output solid red to test
                // Sample the texture and multiply by the base color
                half4 mainTextureColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half4 previousColor = SAMPLE_TEXTURE2D(_PreviousFrame, sampler_PreviousFrame, IN.uv);
                //initially assign the previous color the same as the texture color
                if (_HasInitialized == 0){
                    previousColor = mainTextureColor;
                }
                
                //calculate final color
                float distanceToPressedMouse = length(IN.worldPos - _SlicePosition.xyz);
                half4 finalColor = mainTextureColor;
                //color the pressed pixels
                // if (_MousePressed == 1 && abs(distanceToPressedMouse) < _SliceThickness)
                // {
                //     finalColor = _SliceColor;
                // }
                
                
                
                //color the previous pixels
                // if (mainTextureColor.a > previousColor.a)
                // {
                //     // Use the texture with the lower alpha
                //     finalColor = previousColor;
                // }
                return finalColor;
            }
            ENDHLSL
        }
    }
}