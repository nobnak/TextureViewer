Shader "Unlit/PIPTexture" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}

        _SrcBlend("SrcBlend", Int) = 1 // One
        _DstBlend("DstBlend", Int) = 0 // Zero
    }
    SubShader {
        Blend[_SrcBlend][_DstBlend]
        Cull Off
        ZTest Always
        ZWrite Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4x4 _ChannelMixer;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float4 cmain = tex2D(_MainTex, i.uv);
                float4 c = cmain;

                c = mul(_ChannelMixer, c);

                return c;
            }
            ENDCG
        }
    }
}
