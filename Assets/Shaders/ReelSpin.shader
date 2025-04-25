Shader "Custom/ReelSpin"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Tex1 ("Texture 1", 2D) = "white" {}
        _Tex2 ("Texture 2", 2D) = "white" {}
        _Tex3 ("Texture 3", 2D) = "white" {}
        _Tex1Blurred ("Texture 1 Blurred", 2D) = "white" {}
        _Tex2Blurred ("Texture 2 Blurred", 2D) = "white" {}
        _Tex3Blurred ("Texture 3 Blurred", 2D) = "white" {}
        _Scroll ("Scroll", Range(-1,1)) = -0.5
        _Blur ("Blur", Range(0,1)) = 0.5
        _ClipMin ("Clip Min Y", Range(0,1)) = 0.0
        _ClipMax ("Clip Max Y", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="False"
        }

        LOD 100

        Cull Back
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _Tex1;
            sampler2D _Tex2;
            sampler2D _Tex3;
            sampler2D _Tex1Blurred;
            sampler2D _Tex2Blurred;
            sampler2D _Tex3Blurred;
            float _Scroll;
            float _Blur;
            float _ClipMin;
            float _ClipMax;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //Compute the scrolling UVs
                float scrollUV = frac(i.uv.y + _Scroll / 3.0);
                float section = 1.0 / 3.0;

                float weight1 = step(0.0, scrollUV) * step(scrollUV, section);
                float weight2 = step(section, scrollUV) * step(scrollUV, section * 2.0);
                float weight3 = step(section * 2.0, scrollUV);

                float2 texUV = float2(i.uv.x, frac(scrollUV * 3.0));

                //Get non-blurred and blurred texture colors
                fixed4 col1 = lerp(tex2D(_Tex1, texUV),tex2D(_Tex1Blurred, texUV), _Blur);
                fixed4 col2 = lerp(tex2D(_Tex2, texUV),tex2D(_Tex2Blurred, texUV), _Blur);
                fixed4 col3 = lerp(tex2D(_Tex3, texUV),tex2D(_Tex3Blurred, texUV), _Blur);
 
                //Interpolate between normal and blurred textures based on speed
                fixed4 finalColor = col1 * weight1 + col2 * weight2 + col3 * weight3;
 
                //Clipping mask (0.0–1.0 Y range, outside alpha = 0)
                float clip = step(_ClipMin, i.uv.y) * step(i.uv.y, _ClipMax);
                finalColor.a *= clip;

                return finalColor;
            }
            ENDCG
        }
    }
}
