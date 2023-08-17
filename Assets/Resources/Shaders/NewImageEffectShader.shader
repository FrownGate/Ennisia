Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1) 
        _OutlineThickness ("Outline Thickness", Range(0,0.1)) = 0.02
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //complete _OutlineColor _OutlineThickness declaration
            uniform fixed4 _OutlineColor;
            uniform float _OutlineThickness;
            

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
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                 bool onEdge = false;

                // Offsets to sample the surrounding pixels
                float2 offsets[8] = {
                    float2(-_OutlineThickness, 0),
                    float2(_OutlineThickness, 0),
                    float2(0, -_OutlineThickness),
                    float2(0, _OutlineThickness),
                    float2(-_OutlineThickness, -_OutlineThickness),
                    float2(_OutlineThickness, _OutlineThickness),
                    float2(-_OutlineThickness, _OutlineThickness),
                    float2(_OutlineThickness, -_OutlineThickness)
                };

                // Check the surrounding pixels
                for (int j = 0; j < 8; j++)
                {
                    fixed4 sampleCol = tex2D(_MainTex, i.uv + offsets[j]);
                    if(sampleCol.a <= 0.1) // Change 0.1 if you want to adjust edge detection sensitivity
                    {
                        onEdge = true;
                        break;
                    }
                }

                // If on the edge, blend with the outline color
                if(onEdge)
                {
                    col.rgb = lerp(col.rgb, _OutlineColor.rgb, _OutlineColor.a);
                }
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
