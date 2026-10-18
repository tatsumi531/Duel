Shader "Duel/SpriteSlice"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _CutPoint ("Cut Point (Local Space)", Vector) = (0, 0, 0, 0)
        _CutNormal ("Cut Normal (Local Space)", Vector) = (0, 1, 0, 0)
        _KeepSide ("Keep Side (+1 / -1)", Float) = 1
        _EdgeWidth ("Edge Width", Range(0, 0.5)) = 0.05
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float2 localPos : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float2 _CutPoint;
                float2 _CutNormal;
                float _KeepSide;
                float _EdgeWidth;
                float4 _EdgeColor;
            CBUFFER_END

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                // SpriteRenderer.color reaches the shader either baked into the vertex
                // color stream (dynamic sprite batching) or through unity_SpriteColor
                // (non-batched / SRP Batcher path). Only one of them carries it, so
                // multiplying both is what URP's own sprite shaders do.
                OUT.color = IN.color * unity_SpriteColor;
                OUT.localPos = IN.positionOS.xy;
                return OUT;
            }

            float4 Frag(Varyings IN) : SV_Target
            {
                // d > 0 means "inside the half that should remain visible".
                float2 n = normalize(_CutNormal);
                float d = dot(IN.localPos - _CutPoint, n) * _KeepSide;
                clip(d);

                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float4 col = texColor * IN.color;

                // Bright seam right at the cut line, fading out over _EdgeWidth.
                float edge = saturate((_EdgeWidth - d) / max(_EdgeWidth, 1e-5));
                col.rgb = lerp(col.rgb, _EdgeColor.rgb, edge * _EdgeColor.a * texColor.a);

                return col;
            }
            ENDHLSL
        }
    }
}
