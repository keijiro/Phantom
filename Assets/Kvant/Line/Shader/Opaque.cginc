//
// Kvant/Line - One dimensional object array renderer
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

sampler2D _MainTex;
half3 _Color;

half _Metallic;
half _Smoothness;

sampler2D _NormalMap;
half _NormalScale;

sampler2D _OcclusionMap;
half _OcclusionStrength;

struct Input
{
    float2 uv_MainTex;
};

void vert(inout appdata_full v)
{
    float4 p = SamplePosition(v.texcoord1.xy);
    float4 r = SampleRotation(v.texcoord1.xy);

    v.vertex.xyz = RotateVector(v.vertex.xyz, r) * p.w + p.xyz;
    v.normal = RotateVector(v.normal, r);
#if defined(_NORMALMAP)
    v.tangent.xyz = RotateVector(v.tangent.xyz, r);
#endif
}

void surf(Input IN, inout SurfaceOutputStandard o)
{
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = _Color * c.rgb;

#if defined(_NORMALMAP)
    fixed4 n = tex2D(_NormalMap, IN.uv_MainTex);
    o.Normal = UnpackScaleNormal(n, _NormalScale);
#endif

#if defined(_OCCLUSIONMAP)
    fixed occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
    o.Occlusion = LerpOneTo(occ, _OcclusionStrength);
#endif

    o.Metallic = _Metallic;
    o.Smoothness = _Smoothness;
}
