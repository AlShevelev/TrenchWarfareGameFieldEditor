// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BWLinePattern"
{
    Properties{
 
        _densityLines ("Density Lines",Float) = 0
        _blurryFrontier("Blurry Frontier", Range(0.01,1.0)) = 0
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
       
            #include "UnityCG.cginc"
 
            float _densityLines;
            float _blurryFrontier;
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };
       
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.posWorld = mul(unity_ObjectToWorld,v.vertex);
                return o;
            }
       
            fixed4 frag (v2f i) : SV_Target
            {
 
                float f = smoothstep(-_blurryFrontier,_blurryFrontier,sin(_densityLines*i.posWorld.y));
                float4 col1 = float4(1.0,1.0,1.0,1.0);
                float4 col2 = float4(0.0,0.0,0.0,1.0);
                return lerp(col1,col2,f);
            }
            ENDCG
        }
    }
}