Shader "Unlit/IceCrystal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (0.6, 0.9, 1.0, 0.7)
        _Brightness("Brightness", Float) = 1.5
        _Cube ("Reflection Cubemap", CUBE) = "" {}
        _ReflectStrength ("Reflection Strength", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back
        // LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Brightness;
            samplerCUBE _Cube;
            float _ReflectStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldRefl : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                o.worldRefl = reflect(-viewDir, worldNormal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = tex * _Color * _Brightness;

                fixed4 refl = texCUBE(_Cube, i.worldRefl);
                col.rgb = lerp(col.rgb, col.rgb + refl.rgb, _ReflectStrength);
                col.a *= _Color.a;

                return col;
            }
            ENDCG
        }
    }
}
