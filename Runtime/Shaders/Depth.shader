Shader "Render Depth Monochrome" {
    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f {
                float4 pos : SV_POSITION;
                float depth : TEXCOORD0;
            };

            v2f vert (appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.depth = ComputeScreenPos(o.pos).z;
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                float depthNormalized = i.depth / _ZBufferParams.z;
                return half4(depthNormalized, depthNormalized, depthNormalized, 1.0);
            }
            ENDCG
        }
    }
}