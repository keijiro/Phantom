Shader "Hidden/Kino/Scanner"
{
    Properties
    {
        _MainTex("", 2D) = ""{}
        _Color1("", Color) = (1, 1, 1)
        _Color2("", Color) = (1, 1, 1)
        _Color3("", Color) = (1, 1, 1)
    }
    Subshader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #include "Scanner.cginc"
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }
    }
}
