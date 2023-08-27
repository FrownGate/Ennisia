
Shader "Unlit/SOL"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OutlineWidth ("Outline Width", Range(0, 0.03)) = 0.01
        _AlphaThreshold ("Alpha Threshold", Range(0, 1)) = 0.1
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _OutlineWidth;
            float _AlphaThreshold;
            
            static const float2 offsets[8] = {
                float2(-1, -1), float2(-1, 1), float2(1, -1), float2(1, 1),
                float2(-1,  0), float2(1,  0), float2(0, -1), float2(0,  1)
            };
            fixed4 _OutlineColor;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv);

            if (col.a < _AlphaThreshold)
                return col;
            
            fixed4 outlineCol = _OutlineColor;
            
            float2 currentOffset = offsets[0] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[1] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[2] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[3] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[4] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[5] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[6] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            currentOffset = offsets[7] * _OutlineWidth;
            if(tex2D(_MainTex, i.uv + currentOffset).a < _AlphaThreshold) { return outlineCol; }

            return col;
        }

            ENDCG
        }
    }
}

