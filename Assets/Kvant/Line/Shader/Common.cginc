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

#include "UnityCG.cginc"

// Position buffer
// .xyz = position
// .w   = scale
sampler2D _PositionBuffer;
float4 _PositionBuffer_TexelSize;

sampler2D _PreviousPositionBuffer;
float4 _PreviousPositionBuffer_TexelSize;

float4 SamplePosition(float2 uv)
{
    return tex2Dlod(_PositionBuffer, float4(uv, 0, 0));
}

float4 SamplePreviousPosition(float2 uv)
{
    return tex2Dlod(_PreviousPositionBuffer, float4(uv, 0, 0));
}

// Rotation buffer
// .xyzw = rotation (represented as quaternion)
sampler2D _RotationBuffer;
float4 _RotationBuffer_TexelSize;

sampler2D _PreviousRotationBuffer;
float4 _PreviousRotationBuffer_TexelSize;

float4 SampleRotation(float2 uv)
{
    return tex2Dlod(_RotationBuffer, float4(uv, 0, 0));
}

float4 SamplePreviousRotation(float2 uv)
{
    return tex2Dlod(_PreviousRotationBuffer, float4(uv, 0, 0));
}

// Pseudo random number generator
float _RandomSeed;

float UVRandom(float2 uv, float salt)
{
    uv += float2(salt, _RandomSeed);
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
float4 QMult(float4 q1, float4 q2)
{
    float3 ijk = q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz);
    return float4(ijk, q1.w * q2.w - dot(q1.xyz, q2.xyz));
}

// Vector rotation with a quaternion
// http://mathworld.wolfram.com/Quaternion.html
float3 RotateVector(float3 v, float4 r)
{
    float4 r_c = r * float4(-1, -1, -1, 1);
    return QMult(r, QMult(float4(v, 0), r_c)).xyz;
}
