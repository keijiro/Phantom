Shader "Hidden/Phantom/ScreenBlit"
{
    Properties
    {
        _MainTex("", 2D) = "white" {}
        _Color("", Color) = (0, 0, 0, 0)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;

    fixed _Invert;
    fixed4 _Color;
    fixed _Intensity;

    fixed4 frag(v2f_img i) : SV_Target
    {
        fixed4 c = saturate(tex2D(_MainTex, i.uv));

        fixed3 rgb = LinearToGammaSpace(c);

        rgb = lerp(rgb, 1 - rgb, _Invert);
        rgb = lerp(rgb, _Color.rgb, _Color.a);
        rgb *= _Intensity;

        c.rgb = GammaToLinearSpace(rgb);

        return c;
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
