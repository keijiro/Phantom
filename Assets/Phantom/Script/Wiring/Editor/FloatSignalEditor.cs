using UnityEngine;
using UnityEditor;

namespace Klak.Wiring
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FloatSignal))]
    public class FloatSignalEditor : Editor
    {
        public override void OnInspectorGUI()
        {
        }
    }
}
