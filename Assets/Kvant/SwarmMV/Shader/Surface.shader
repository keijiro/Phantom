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

Shader "Kvant/SwarmMV/Surface"
{
    Properties
    {
        [HideInInspector] _PositionBuffer("", 2D) = "black"{}
        [HideInInspector] _PreviousPositionBuffer("", 2D) = "black"{}

        [HideInInspector] _BinormalBuffer("", 2D) = "red"{}
        [HideInInspector] _PreviousBinormalBuffer("", 2D) = "red"{}

        [Enum(Smooth, 0, Random, 1)]
        _ColorMode("Color Mode", Float) = 0
        _Color1("Color 1", Color) = (1, 1, 1)
        _Color2("Color 2", Color) = (1, 1, 1)

        _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode" = "MotionVectors" }
            ZWrite Off Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "Motion.cginc"
            ENDCG
        }

        Cull Back
        CGPROGRAM
        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0
        #include "Surface.cginc"
        ENDCG

        Cull Front
        CGPROGRAM
        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0
        #define NORMAL_FLIP
        #include "Surface.cginc"
        ENDCG
    }
    CustomEditor "Kvant.SwarmMVMaterialEditor"
}
