#include "Common.cginc"

sampler2D _MainTex;
half4 _Color;
half4 _BackColor;

sampler2D _NormalMap;
half _NormalMapScale;

half _Metallic;
half _Smoothness;

half _Deform;
half _Cutoff;

struct Input
{
    float2 uv_MainTex;
    float3 worldPos;
};

void vert(inout appdata_full v, out Input data)
{
    UNITY_INITIALIZE_OUTPUT(Input, data);

    float3 v0 = v.vertex.xyz;
    float3 v1 = v.texcoord1.xyz;
    float3 v2 = v.texcoord2.xyz;

    v0 = Displace(v0, _Time.y);
    v1 = Displace(v1, _Time.y);
    v2 = Displace(v2, _Time.y);
    float3 nrm = normalize(-cross(v2 - v0, v1 - v0));

    v.vertex.xyz = lerp(v.vertex.xyz, v0, _Deform);
    v.normal = lerp(v.normal, nrm, saturate(_Deform * 10));
}

void surf(Input IN, inout SurfaceOutputStandard o)
{
    clip(Alpha(IN.worldPos, _Time.y) - _Cutoff);

    float4 c_map = tex2D(_MainTex, IN.uv_MainTex);
    float4 n_map = tex2D(_NormalMap, IN.uv_MainTex);
    float3 nrm = UnpackScaleNormal(n_map, _NormalMapScale);

#ifdef BACKFACE
    o.Albedo = _BackColor.rgb;
    o.Metallic = 0;
    o.Smoothness = 0;
    o.Normal = -nrm;
#else
    o.Albedo = c_map.rgb * _Color.rgb;
    o.Metallic = _Metallic;
    o.Smoothness = _Smoothness;
    o.Normal = nrm;
#endif
}
