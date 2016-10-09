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
#include "SimplexNoise2D.cginc"

float _Length;
float _Scroll; // Scroll amount in UV space
float _BaseScale;

// Noise field parameters
// (offset x, offset y, frequency, amplitude)
float4 _Displace;
float4 _Twist;
float4 _Tilt;
float4 _Scale;

float UVToPosition(float2 uv)
{
    return (uv.x - ceil(uv.x - _Scroll) - 0.5) * _Length;
}

float4 frag_Position(v2f_img i) : SV_Target
{
    float pz = UVToPosition(i.uv);

    float2 np1 = float2(pz * _Displace.z, 0) + _Displace.xy;
    float2 np2 = np1 + float2(81.57, 68.39);

    float px = snoise(np1) * _Displace.w;
    float py = snoise(np2) * _Displace.w;

    float2 np3 = float2(pz * _Scale.z, 0) + _Scale.xy;

    float s = _BaseScale * (1 + snoise(np3) * _Scale.w);

    pz = (frac(i.uv.x - _Scroll) - 0.5) * _Length;

    return float4(px, py, pz, s);
}

float4 frag_Rotation(v2f_img i) : SV_Target
{
    float pz = UVToPosition(i.uv);

    float2 np1 = float2(pz * _Twist.z, 0) + _Twist.xy;
    float2 np2 = float2(pz * _Tilt .z, 0) + _Tilt .xy;
    float2 np3 = np2 + float2(38.29, 43.28);

    float r1 = snoise(np1) * _Twist.w;
    float r2 = snoise(np2) * _Tilt .w;
    float r3 = snoise(np3) * _Tilt .w;

    float sn1, cs1;
    float sn2, cs2;

    sincos(r1, sn1, cs1);
    float4 q1 = float4(0, 0, sn1, cs1);

    sincos(r2, sn1, cs1);
    sincos(r3 * UNITY_PI, sn2, cs2);
    float4 q2 = float4(cs2 * sn1, sn2 * sn1, 0, cs1);

    return QMult(q2, q1);
}
