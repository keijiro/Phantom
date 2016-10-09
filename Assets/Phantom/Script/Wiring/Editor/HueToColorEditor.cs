using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HueToColor))]
    public class HueToColorEditor : Editor
    {
        SerializedProperty _saturation;
        SerializedProperty _brightness;
        SerializedProperty _colorEvent;

        void OnEnable()
        {
            _saturation = serializedObject.FindProperty("_saturation");
            _brightness = serializedObject.FindProperty("_brightness");
            _colorEvent = serializedObject.FindProperty("_colorEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_saturation);
            EditorGUILayout.PropertyField(_brightness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_colorEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
