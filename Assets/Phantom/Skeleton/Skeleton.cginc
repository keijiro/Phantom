#include "UnityCG.cginc"
#include "ClassicNoise2D.cginc"

sampler2D _AlbedoMap;
half3 _AlbedoColor;

half _Metallic;
half _Smoothness;

sampler2D _NormalMap;
half _NormalMapScale;

sampler2D _DetailNormalMap;
half _DetailNormalMapScale;

sampler2D _OcclusionMap;
half _OcclusionStrength;

sampler2D _CurvatureMap;
half3 _CurvatureEmission;

half3 _BackColor;
half _BackMetallic;
half _BackSmoothness;

float _NoiseAmp;
float _NoiseSpeed;
float _NoiseFreq;
float _NoiseRough;

float _SpikeProb;
float _SpikeAmp;

float _HelixFreq;
float _HelixSlope;
float _HelixSpeed;

float _Cutoff;
float _WaveFreq;
float _WaveAmp;
float _WaveSpeed;

float _RandomSeed;

struct Input
{
    float2 uv_AlbedoMap;
    float3 rawPosition;
};

float Random01(float3 v, float t)
{
    float2 uv = v.xy + float2(v.z * -2.1, t);
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

float3 ApplyModifier(float3 v)
{
    float time = _Time.y;
    float phi = atan2(v.z, v.x) / UNITY_PI;

    float2 np = float2(phi, v.y * 2) * _NoiseFreq;
    np.y += _NoiseSpeed * time + _RandomSeed;

    float2 nrep = float2(_NoiseFreq, 1000);

    float n = pnoise(np, nrep);
    n += pnoise(np * 5, nrep * 5) * _NoiseRough;

    float sp = Random01(v, _RandomSeed);
    sp = (sp < _SpikeProb) * _SpikeAmp * 4;

    v.xz *= 1 + n * _NoiseAmp + abs(n) * sp;

    return v;
}

void Cutout(float3 v)
{
    float time = _Time.y;
    float phi = atan2(v.x, v.z) / UNITY_PI;

    float w = sin((v.y * 2 + phi) * _WaveFreq * UNITY_PI - time * _WaveSpeed);
    float p = v.y * _HelixFreq + phi * _HelixSlope + time * _HelixSpeed;

    clip(frac(p) + w * _WaveAmp - _Cutoff);
}

void ModifyVertex(inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUTPUT(Input, o);

    o.rawPosition = v.vertex.xyz;

    float3 v1 = ApplyModifier(v.vertex.xyz);
    float3 v2 = ApplyModifier(v.texcoord1.xyz);
    float3 v3 = ApplyModifier(v.texcoord2.xyz);
    float3 n = normalize(cross(v2 - v1, v3 - v1));

    v.vertex.xyz = v1;
    v.normal = normalize((v.normal + n) * 0.5);

#ifdef BACKFACE
    v.normal *= -1;
#endif
}

void Surface(Input IN, inout SurfaceOutputStandard o)
{
    Cutout(IN.rawPosition);

#ifndef BACKFACE

    float2 uv = IN.uv_AlbedoMap;
    half4 a_map = tex2D(_AlbedoMap, uv);
    o.Albedo = saturate(_AlbedoColor.rgb * a_map);

    o.Metallic = _Metallic;
    o.Smoothness = _Smoothness;

    half4 n1_map = tex2D(_NormalMap, uv);
    half4 n2_map = tex2D(_DetailNormalMap, uv);
    half3 n1 = UnpackScaleNormal(n1_map, _NormalMapScale);
    half3 n2 = UnpackScaleNormal(n2_map, _DetailNormalMapScale);
    o.Normal = BlendNormals(n1, n2);

    half o_map = tex2D(_OcclusionMap, uv).g;
    o.Occlusion = LerpOneTo(o_map, _OcclusionStrength);

    half cv = dot(tex2D(_CurvatureMap, uv).rb, 2);
    cv *= pow(frac(IN.rawPosition.y - _Time.y), 3);
    o.Emission = _CurvatureEmission * saturate(cv * 8);

#else

    o.Albedo = _BackColor;
    o.Metallic = _BackMetallic;
    o.Smoothness = _BackSmoothness;

#endif
}
