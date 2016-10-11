using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CameraClearFlagsOut))]
    public class CameraClearFlagsOutEditor : Editor
    {
        SerializedProperty _targetCamera;

        void OnEnable()
        {
            _targetCamera = serializedObject.FindProperty("_targetCamera");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_targetCamera);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
