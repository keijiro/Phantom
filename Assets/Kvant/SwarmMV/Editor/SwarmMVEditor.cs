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
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SwarmMV))]
    public class SwarmMVEditor : Editor
    {
        SerializedProperty _template;
        SerializedProperty _randomSeed;

        SerializedProperty _throttle;
        SerializedProperty _twist;
        SerializedProperty _flow;

        SerializedProperty _attractor;
        SerializedProperty _attractorPosition;
        SerializedProperty _attractorRadius;
        SerializedProperty _forcePerDistance;
        SerializedProperty _forceRandomness;
        SerializedProperty _drag;

        SerializedProperty _noiseAmplitude;
        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseSpread;
        SerializedProperty _noiseSpeed;

        SerializedProperty _swirlAmplitude;
        SerializedProperty _swirlFrequency;

        SerializedProperty _lineWidth;
        SerializedProperty _lineWidthRandomness;

        static GUIContent _textAmplitude  = new GUIContent("Amplitude");
        static GUIContent _textFlow       = new GUIContent("Flow Vector");
        static GUIContent _textFrequency  = new GUIContent("Frequency");
        static GUIContent _textRandomness = new GUIContent("Randomness");
        static GUIContent _textSpeed      = new GUIContent("Speed");
        static GUIContent _textSpread     = new GUIContent("Spread");

        void OnEnable()
        {
            _template   = serializedObject.FindProperty("_template");
            _randomSeed = serializedObject.FindProperty("_randomSeed");

            _throttle = serializedObject.FindProperty("_throttle");
            _twist    = serializedObject.FindProperty("_twist");
            _flow     = serializedObject.FindProperty("_flow");

            _attractor         = serializedObject.FindProperty("_attractor");
            _attractorPosition = serializedObject.FindProperty("_attractorPosition");
            _attractorRadius   = serializedObject.FindProperty("_attractorRadius");
            _forcePerDistance  = serializedObject.FindProperty("_forcePerDistance");
            _forceRandomness   = serializedObject.FindProperty("_forceRandomness");
            _drag              = serializedObject.FindProperty("_drag");

            _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseSpread    = serializedObject.FindProperty("_noiseSpread");
            _noiseSpeed     = serializedObject.FindProperty("_noiseSpeed");

            _swirlAmplitude = serializedObject.FindProperty("_swirlAmplitude");
            _swirlFrequency = serializedObject.FindProperty("_swirlFrequency");

            _lineWidth           = serializedObject.FindProperty("_lineWidth");
            _lineWidthRandomness = serializedObject.FindProperty("_lineWidthRandomness");
        }

        public override void OnInspectorGUI()
        {
            var targetInstance = target as SwarmMV;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_template);
            EditorGUILayout.PropertyField(_randomSeed);

            if (EditorGUI.EndChangeCheck())
                targetInstance.RequestReconfigurationFromEditor();

            EditorGUILayout.PropertyField(_throttle);
            EditorGUILayout.PropertyField(_lineWidth);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_lineWidthRandomness, _textRandomness);
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(_twist);
            EditorGUILayout.PropertyField(_flow, _textFlow);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Head: Dynamics", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_attractor);
            if (_attractor.hasMultipleDifferentValues ||
                _attractor.objectReferenceValue == null)
                EditorGUILayout.PropertyField(_attractorPosition);
            EditorGUILayout.PropertyField(_attractorRadius);
            EditorGUILayout.PropertyField(_forcePerDistance);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_forceRandomness, _textRandomness);
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(_drag);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Head: Noise Field", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_noiseAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_noiseFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_noiseSpread, _textSpread);
            EditorGUILayout.PropertyField(_noiseSpeed, _textSpeed);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Trail: Noise Field", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_swirlAmplitude, _textAmplitude);
            EditorGUILayout.PropertyField(_swirlFrequency, _textFrequency);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
