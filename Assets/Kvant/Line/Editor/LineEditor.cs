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
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Line))]
    public class LineEditor : Editor
    {
        SerializedProperty _template;
        SerializedProperty _length;
        SerializedProperty _scrollSpeed;
        SerializedProperty _noiseDirectivity;
        SerializedProperty _randomSeed;

        SerializedProperty _displaceFrequency;
        SerializedProperty _displaceAmplitude;
        SerializedProperty _displaceSpeed;

        SerializedProperty _twistFrequency;
        SerializedProperty _twistAmplitude;
        SerializedProperty _twistSpeed;

        SerializedProperty _tiltFrequency;
        SerializedProperty _tiltAmplitude;
        SerializedProperty _tiltSpeed;

        SerializedProperty _baseScale;
        SerializedProperty _scaleFrequency;
        SerializedProperty _scaleAmplitude;
        SerializedProperty _scaleSpeed;

        static GUIContent _textAmplitude = new GUIContent("Amplitude");
        static GUIContent _textFrequency = new GUIContent("Frequency");
        static GUIContent _textSpeed = new GUIContent("Speed");

        void OnEnable()
        {
            _template = serializedObject.FindProperty("_template");
            _length = serializedObject.FindProperty("_length");
            _scrollSpeed = serializedObject.FindProperty("_scrollSpeed");
            _randomSeed = serializedObject.FindProperty("_randomSeed");
            _noiseDirectivity = serializedObject.FindProperty("_noiseDirectivity");

            _displaceFrequency = serializedObject.FindProperty("_displaceFrequency");
            _displaceAmplitude = serializedObject.FindProperty("_displaceAmplitude");
            _displaceSpeed = serializedObject.FindProperty("_displaceSpeed");

            _twistFrequency = serializedObject.FindProperty("_twistFrequency");
            _twistAmplitude = serializedObject.FindProperty("_twistAmplitude");
            _twistSpeed = serializedObject.FindProperty("_twistSpeed");

            _tiltFrequency = serializedObject.FindProperty("_tiltFrequency");
            _tiltAmplitude = serializedObject.FindProperty("_tiltAmplitude");
            _tiltSpeed = serializedObject.FindProperty("_tiltSpeed");

            _baseScale = serializedObject.FindProperty("_baseScale");
            _scaleFrequency = serializedObject.FindProperty("_scaleFrequency");
            _scaleAmplitude = serializedObject.FindProperty("_scaleAmplitude");
            _scaleSpeed = serializedObject.FindProperty("_scaleSpeed");
        }

        public override void OnInspectorGUI()
        {
            var targetInstance = target as Line;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_template);

            if (EditorGUI.EndChangeCheck())
                targetInstance.RequestReconfigurationFromEditor();

            EditorGUILayout.PropertyField(_length);
            EditorGUILayout.PropertyField(_scrollSpeed);
            EditorGUILayout.PropertyField(_baseScale);
            EditorGUILayout.PropertyField(_noiseDirectivity);
            EditorGUILayout.PropertyField(_randomSeed);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Noise Field To Position", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_displaceFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_displaceAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_displaceSpeed, _textSpeed);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Noise Field To Twist", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_twistFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_twistAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_twistSpeed, _textSpeed);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Noise Field To Tilt", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_tiltFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_tiltAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_tiltSpeed, _textSpeed);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Noise Field To Scale", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_scaleFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_scaleAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_scaleSpeed, _textSpeed);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
