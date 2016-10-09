Shader "Eyeball/Deformer"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "gray" {}
        _Color("Color", Color) = (1, 1, 1)
        _BackColor("Backface Color", Color) = (1, 1, 1)

        [Space]
        [Normal] _NormalMap("Normal", 2D) = "bump"{}
        _NormalMapScale("Scale", Range(0, 1)) = 1

        [Space]
        [Gamma] _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0

        [Space]
        _Deform("Deform", Range(0, 1)) = 0.5
        _Cutoff("Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode" = "MotionVectors" }
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "Motion.cginc"
            ENDCG
        }

        Cull Back

        CGPROGRAM
        #pragma surface surf Standard nolightmap vertex:vert
        #pragma target 3.0
        #include "Deformer.cginc"
        ENDCG

        Cull Front

        CGPROGRAM
        #pragma surface surf Standard nolightmap vertex:vert
        #pragma target 3.0
        #define BACKFACE 1
        #include "Deformer.cginc"
        ENDCG
    }
    FallBack "Diffuse"
}
