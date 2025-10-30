Shader "Unlit/FixedCircle"
{
    Properties
    {
        _MainTex ("Circle Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _ScreenSize ("Size in Pixels", Float) = 64
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _ScreenSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenUV : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // ширина объекта в пикселях = _ScreenSize
                float2 screenSize = _ScreenParams.xy;
                float2 pixelSize = _ScreenSize / screenSize;

                // центр объекта
                float4 worldCenter = mul(unity_ObjectToWorld, float4(0,0,0,1));
                float4 clipCenter = UnityWorldToClipPos(worldCenter);

                // размер quad'а в clip space, чтобы был фиксированного размера на экране
                float2 clipPixelSize = pixelSize * 2.0; // clip space от -1 до 1
                float2 offset = (v.vertex.xy - float2(0, 0)) * clipPixelSize;

                o.vertex.xy = clipCenter.xy + offset;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
