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

#include "UnityCG.cginc"

// Position buffer
sampler2D _PositionBuffer;
float4 _PositionBuffer_TexelSize;

sampler2D _PreviousPositionBuffer;
float4 _PreviousPositionBuffer_TexelSize;

float3 SamplePosition(float2 uv)
{
    return tex2Dlod(_PositionBuffer, float4(uv, 0, 0)).xyz;
}

float3 SamplePosition(float2 uv, float delta)
{
    float du = _PositionBuffer_TexelSize.x * delta;
    return tex2Dlod(_PositionBuffer, float4(uv.x + du, uv.y, 0, 0)).xyz;
}

float3 SamplePreviousPosition(float2 uv)
{
    return tex2Dlod(_PreviousPositionBuffer, float4(uv, 0, 0)).xyz;
}

// Velocity buffer
sampler2D _VelocityBuffer;
float4 _VelocityBuffer_TexelSize;

float3 SampleVelocity(float2 uv)
{
    return tex2Dlod(_VelocityBuffer, float4(uv, 0, 0)).xyz;
}

// Orthonormal basis vectors buffer
// xy: normal, zw: binormal
// Vectors are encoded with the stereographic projection.
sampler2D _BasisBuffer;
float4 _BasisBuffer_TexelSize;

sampler2D _PreviousBasisBuffer;
float4 _PreviousBasisBuffer_TexelSize;

float4 SampleBasis(float2 uv)
{
    return tex2Dlod(_BasisBuffer, float4(uv, 0, 0));
}

float4 SampleBasis(float2 uv, float delta)
{
    float du = _BasisBuffer_TexelSize.x * delta;
    return tex2Dlod(_BasisBuffer, float4(uv.x + du, uv.y, 0, 0));
}

float4 SamplePreviousBasis(float2 uv)
{
    return tex2Dlod(_PreviousBasisBuffer, float4(uv, 0, 0));
}

// Seed for PRNG
float _RandomSeed;

// PRNG function
float UVRandom(float2 uv, float salt)
{
    uv += float2(salt, _RandomSeed);
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// Stereographic projection and inverse projection
float2 StereoProjection(float3 n)
{
    return n.xy / (1 - n.z);
}

float3 StereoInverseProjection(float2 p)
{
    float d = 2 / (dot(p.xy, p.xy) + 1);
    return float3(p.xy * d, 1 - d);
}

// Line width and throttling
float2 _LineWidth; // min, max
float _Throttle;

float LineWidth(float2 uv)
{
    // Line width
    float w = lerp(_LineWidth.x, _LineWidth.y, UVRandom(uv.y, 10));
    return w * saturate((_Throttle - uv.y) / _PositionBuffer_TexelSize.y);
}
