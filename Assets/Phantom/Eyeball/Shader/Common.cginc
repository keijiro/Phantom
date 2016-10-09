#include "UnityCG.cginc"
#include "SimplexNoise3D.cginc"

// Pseudo random number generator
float Random(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// Uniformaly distributed points on a unit sphere
// http://mathworld.wolfram.com/SpherePointPicking.html
// Note: Not strictly implemented. Results will be slightly biased.
float3 RandomVector(float2 uv)
{
    float u = Random(uv) * 2 - 1;
    float u2 = sqrt(1 - u * u);

    float theta = Random(uv + 0.1) * UNITY_PI * 2;

    float sn, cs;
    sincos(theta, sn, cs);

    return float3(cs * u2, sn * u2, u);
}

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
float4 QMul(float4 q1, float4 q2)
{
    return float4(
        q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz),
        q1.w * q2.w - dot(q1.xyz, q2.xyz)
    );
}

// Construct a quaternion representing a rotation around a given axis.
float4 RotationQuaternion(float3 axis, float radian)
{
    float sn, cs;
    sincos(radian * 0.5, sn, cs);
    return float4(axis * sn, cs);
}

// Rotate a vector with a given quaternion
// http://mathworld.wolfram.com/Quaternion.html
float3 RotateWithQuaternion(float3 v, float4 r)
{
    float4 r_c = r * float4(-1, -1, -1, 1);
    return QMul(r, QMul(float4(v, 0), r_c)).xyz;
}

// Displacement function
float3 Displace(float3 p, float time)
{
    float3 np = p * 2;
    np.z += time * 2;

    float d = snoise(np);
    d = max(1 + d * abs(d) * 4, 0.4);

    return p * d;
}

// Alpha (for cutoff) function
float Alpha(float3 p, float time)
{
    p.z += time * 3;
    return (snoise(p) + 1) * 0.5;
}
