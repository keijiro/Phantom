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
#include "SimplexNoiseGrad3D.cginc"

float3 _Acceleration;   // min, max, drag
float4 _Attractor;      // x, y, z, spread
float3 _Flow;
float3 _NoiseParams;    // amplitude, frequency, spread
float3 _NoiseOffset;
float2 _SwirlParams;    // amplitude, frequency
float _Twist;
float  _DeltaTime;

// Divergence-free noise vector field
float3 DFNoise(float3 p, float k)
{
    p += float3(0.9, 1.0, 1.1) * k * _NoiseParams.z;
    float3 n1 = snoise_grad(p);
    float3 n2 = snoise_grad(p + float3(15.3, 13.1, 17.4));
    return cross(n1, n2);
}

// Attractor position with spread parameter
float3 AttractPoint(float2 uv)
{
    float3 r = float3(UVRandom(uv, 0), UVRandom(uv, 1), UVRandom(uv, 2));
    return _Attractor.xyz + (r - 0.5) * _Attractor.w;
}

// Pass 0: initial position
float4 frag_InitPosition(v2f_img i) : SV_Target
{
    return float4(AttractPoint(i.uv.y + 2), 0);
}

// Pass 1: initial velocity
float4 frag_InitVelocity(v2f_img i) : SV_Target
{
    return 0;
}

// Pass 2: initial orthonormal basis
float4 frag_InitBasis(v2f_img i) : SV_Target
{
    return 0;
}

// Pass 3: position update
float4 frag_UpdatePosition(v2f_img i) : SV_Target
{
    // u=0: p <- head position
    // u>0: p <- one newer entry in the history array
    float3 p = SamplePosition(i.uv, -1);

    // Velocity vector for the head point (u=0)
    float3 v0 = SampleVelocity(i.uv);

    // Velocity vector for the tail points (u>0)
    float3 np = (p + _NoiseOffset) * _SwirlParams.y;
    float3 v1 = _Flow + DFNoise(np, i.uv.y) * _SwirlParams.x;

    // Apply the velocity vector.
    float u_0 = i.uv.x < _PositionBuffer_TexelSize.x;
    p += lerp(v1, v0, u_0) * _DeltaTime;

    return float4(p, 0);
}

// pass 4: velocity update
float4 frag_UpdateVelocity(v2f_img i) : SV_Target
{
    // Only needs the leftmost pixel.
    float2 uv = i.uv * float2(0, 1);

    // Head point position/velocity
    float3 p = SamplePosition(uv);
    float3 v = SampleVelocity(uv);

    // Force from the attactor
    float accel = lerp(_Acceleration.x, _Acceleration.y, UVRandom(uv, 7));
    float3 fa = (AttractPoint(uv) - p) * accel;

    // Force from the noise vector field
    float3 np = (p + _NoiseOffset) * _NoiseParams.y;
    float3 fn = DFNoise(np, uv.y) * _NoiseParams.x;

    // Apply drag and acceleration force.
    v = v * _Acceleration.z + (fa + fn) * _DeltaTime;

    return float4(v, 0);
}

// pass 5: orthonormal basis update
float4 frag_UpdateBasis(v2f_img i) : SV_Target
{
    // Use the parent normal vector from the previous frame.
    float3 ax = StereoInverseProjection(SampleBasis(i.uv, -1).zw);

    // Tangent vector
    float3 p0 = SamplePosition(i.uv, -2);
    float3 p1 = SamplePosition(i.uv, 2);
    float3 az = normalize(p1 - p0);

    // Reconstruct the orthonormal basis.
    float3 ay = normalize(cross(az, ax));
    ax = normalize(cross(ay, az));

    // Twisting
    ax = normalize(ax + _Twist * ay * (1 - i.uv.x));

    return float4(StereoProjection(ay), StereoProjection(ax));
}
