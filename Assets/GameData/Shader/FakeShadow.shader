Shader "Custom/FakeShadow"
{
    Properties
    {
        _MainTex ("Shadow Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)
        _EmissionIntensity ("Emission Intensity", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Lighting Off
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _EmissionColor;
            float _EmissionIntensity;

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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed3 emission = _EmissionColor.rgb * _EmissionIntensity * col.a;
                return fixed4(emission, col.a);
            }
            ENDCG
        }
    }
}
