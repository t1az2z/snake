Shader "t1az2z/2ColorUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Top Color", Color) = (1,1,1,1)
        _Color2 ("Bottom Color", Color) = (1,1,1,1)
        _StepValue ("Scale", Range(0, 1)) = .5
    }
    SubShader
    {
        LOD 100
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha
        
        Tags
        {
            "Queue"="Background"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            float4 _Color2;
            float _StepValue;

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float border = step(i.uv.y, _StepValue);
                col *= border * _Color2 + (1 - border) * _Color;

                return col;
            }
            ENDCG
        }
    }
}