using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BoolVariable))]
    public class BoolVariableEditor : Editor
    {
        SerializedProperty _trueEvent;
        SerializedProperty _falseEvent;

        void OnEnable()
        {
            _trueEvent = serializedObject.FindProperty("_trueEvent");
            _falseEvent = serializedObject.FindProperty("_falseEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_trueEvent);
            EditorGUILayout.PropertyField(_falseEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
