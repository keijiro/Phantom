//
// Kvant/SwarmMV - "Swarm" with motion vector support
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

#include "Common.cginc"

float4x4 _NonJitteredVP;
float4x4 _PreviousVP;
float4x4 _PreviousM;

struct appdata
{
    float4 vertex : POSITION;
    float2 texcoord : TEXCOORD;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float4 transfer0 : TEXCOORD0;
    float4 transfer1 : TEXCOORD1;
};

v2f vert(appdata v)
{
    float2 uv = v.texcoord.xy;

    // If this vertex is one of the first/last two vertices, do nothing
    // special. It may generate large motion vectors (thus it gets large
    // blur when motion blur is applied).
    // 
    // If the vertex is not a first/last ones, refer to the parent vertex in
    // the previous frame. This suppresses motion vectors and gives static look.
    float ppb_tsx = _PreviousPositionBuffer_TexelSize.x;
    float not_head = uv.x >      ppb_tsx * 3;
    float not_tail = uv.x < (1 - ppb_tsx * 3);
    float u_p0 = uv.x - ppb_tsx * min(not_head, not_tail);

    // Position
    float3 p0 = SamplePreviousPosition(float2(u_p0, uv.y));
    float3 p1 = SamplePosition(uv);

    // Binormal
    float3 b0 = StereoInverseProjection(SamplePreviousBasis(uv).zw);
    float3 b1 = StereoInverseProjection(SampleBasis(uv).zw);

    // Apply the local transformation.
    float lw = LineWidth(uv);
    float3 vp0 = p0 + b0 * lw * v.vertex.x;
    float3 vp1 = p1 + b1 * lw * v.vertex.x;

    // Transfer the data to the pixel shader.
    v2f o;
    o.vertex = UnityObjectToClipPos(float4(vp1, 1));
    o.transfer0 = mul(_PreviousVP, mul(_PreviousM,  float4(vp0, 1)));
    o.transfer1 = mul(_NonJitteredVP, mul(unity_ObjectToWorld, float4(vp1, 1)));
    return o;
}

half4 frag(v2f i) : SV_Target
{
    float3 hp0 = i.transfer0.xyz / i.transfer0.w;
    float3 hp1 = i.transfer1.xyz / i.transfer1.w;

    float2 vp0 = (hp0.xy + 1) / 2;
    float2 vp1 = (hp1.xy + 1) / 2;

#if UNITY_UV_STARTS_AT_TOP
    vp0.y = 1 - vp0.y;
    vp1.y = 1 - vp1.y;
#endif

    return half4(vp1 - vp0, 0, 1);
}
