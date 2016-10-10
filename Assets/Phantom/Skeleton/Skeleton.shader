Shader "Phantom/Skeleton"
{
    Properties
    {
        _AlbedoMap("Albedo", 2D) = "grey"{}
        [HDR] _AlbedoColor("Color", Color) = (0.5, 0.5, 0.5)

        [Space]
        [Gamma] _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0

        [Space]
        _NormalMap("Normals", 2D) = "bump"{}
        _NormalMapScale("Scale", Range(0, 2)) = 1

        [Space]
        _DetailNormalMap("Detail Normals", 2D) = "bump"{}
        _DetailNormalMapScale("Scale", Range(0, 2)) = 1

        [Space]
        _OcclusionMap("Occlusion", 2D) = "white"{}
        _OcclusionStrength("Strength", Range(0, 1)) = 1

        [Header(Curvature Effect)]
        _CurvatureMap("Curvature", 2D) = "black"{}
        [HDR] _CurvatureEmission("Emission", Color) = (1, 1, 1)

        [Header(Back Face Attributes)]
        _BackColor("Color", Color) = (1, 1, 1)
        [Gamma] _BackMetallic("Metallic", Range(0, 1)) = 0
        _BackSmoothness("Smoothness", Range(0, 1)) = 0

        [Header(Helix Parameters)]
        _NoiseAmp("Noise Amplitude", Float) = 0.6
        _NoiseSpeed("Noise Speed", Float) = 1.2
        _NoiseFreq("Noise Frequency", Float) = 1
        _NoiseRough("Noise Roughness", Float) = 0
        [Space]
        _SpikeProb("Spike Probability", Float) = 0.005
        _SpikeAmp("Spike Amplitude", Float) = 1
        [Space]
        _HelixFreq("Helix Frequency", Float) = 30
        _HelixSlope("Helix Slope", Float) = 3
        _HelixSpeed("Helix Speed", Float) = 3
        [Space]
        _Cutoff("Cutoff", Range(0, 1)) = 0.5
        _WaveFreq("Wave Frequency", Float) = 2
        _WaveAmp("Wave Amplitude", Range(0, 1)) = 0.35
        _WaveSpeed("Wave Speed", Float) = 8
        [Space]
        _RandomSeed("Random Seed", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque"}

        Cull back

        CGPROGRAM
        #pragma surface Surface Standard vertex:ModifyVertex addshadow
        #pragma target 3.0
        #include "Skeleton.cginc"
        ENDCG

        Cull front

        CGPROGRAM
        #pragma surface Surface Standard vertex:ModifyVertex addshadow
        #pragma target 3.0
        #define BACKFACE
        #include "Skeleton.cginc"
        ENDCG
    }
}
