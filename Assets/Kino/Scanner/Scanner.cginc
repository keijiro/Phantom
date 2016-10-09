#include "UnityCG.cginc"

sampler2D _MainTex;
sampler2D_float _CameraDepthTexture;
float4x4 _InverseView;

float3 _Axis;
float _Exponent;
float _FallOff;

half3 _Color1;
half3 _Color2;
half3 _Color3;

float _Scale1;
float _Scale2;
float _Scale3;

float _Speed1;
float _Speed2;
float _Speed3;

float3 GetWorldPos(float2 uv, float depth)
{
    float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
    float3 vpos = float3((uv * 2 - 1) / p11_22, -1) * depth;
    return mul(_InverseView, float4(vpos, 1)).xyz;
}

fixed4 frag (v2f_img i) : SV_Target
{
    float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));

    float p = dot(GetWorldPos(i.uv, depth), _Axis);
    float fo = saturate(1 - depth * _FallOff);

    float ch1 = fo * pow(1 - frac(p * _Scale1 + _Time.y * _Speed1), _Exponent);
    float ch2 = fo * pow(1 - frac(p * _Scale2 + _Time.y * _Speed2), _Exponent);
    float ch3 = fo * pow(1 - frac(p * _Scale3 + _Time.y * _Speed3), _Exponent);

    half4 source = tex2D(_MainTex, i.uv);

    source.rgb += _Color1 * ch1;
    source.rgb += _Color2 * ch2;
    source.rgb += _Color3 * ch3;

    return source;
}
