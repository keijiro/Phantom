using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FloatVariable))]
    public class FloatVariableEditor : Editor
    {
        SerializedProperty _floatValue;
        SerializedProperty _interpolator;
        SerializedProperty _valueEvent;

        static GUIContent _textInitialValue = new GUIContent("Initial Value");

        void OnEnable()
        {
            _floatValue = serializedObject.FindProperty("_floatValue");
            _interpolator = serializedObject.FindProperty("_interpolator");
            _valueEvent = serializedObject.FindProperty("_valueEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_floatValue, _textInitialValue);
            EditorGUILayout.PropertyField(_interpolator);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_valueEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
