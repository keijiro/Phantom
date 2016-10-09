using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RandomValue))]
    public class RandomValueEditor : Editor
    {
        SerializedProperty _minimum;
        SerializedProperty _maximum;
        SerializedProperty _outputEvent;

        void OnEnable()
        {
            _minimum = serializedObject.FindProperty("_minimum");
            _maximum = serializedObject.FindProperty("_maximum");
            _outputEvent = serializedObject.FindProperty("_outputEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_minimum);
            EditorGUILayout.PropertyField(_maximum);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_outputEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
