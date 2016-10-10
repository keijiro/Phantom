using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HueToColor))]
    public class HueToColorEditor : Editor
    {
        SerializedProperty _hue;
        SerializedProperty _saturation;
        SerializedProperty _brightness;
        SerializedProperty _colorEvent;

        static GUIContent _textHue = new GUIContent("Initial Hue");
        static GUIContent _textSaturation = new GUIContent("Initial Saturation");
        static GUIContent _textBrightness = new GUIContent("Initial Brightness");

        void OnEnable()
        {
            _hue = serializedObject.FindProperty("_hue");
            _saturation = serializedObject.FindProperty("_saturation");
            _brightness = serializedObject.FindProperty("_brightness");
            _colorEvent = serializedObject.FindProperty("_colorEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_hue, _textHue);
            EditorGUILayout.PropertyField(_saturation, _textSaturation);
            EditorGUILayout.PropertyField(_brightness, _textBrightness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_colorEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
