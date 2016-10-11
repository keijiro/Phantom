using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Output/Component/Camera Clear Flags")]
    public class CameraClearFlagsOut : NodeBase
    {
        #region Editable properties

        [SerializeField]
        Camera _targetCamera;

        #endregion

        #region Node I/O

        [Inlet]
        public void SolidColor()
        {
            if (!enabled) return;
            _targetCamera.clearFlags = CameraClearFlags.SolidColor;
        }

        [Inlet]
        public void Skybox()
        {
            if (!enabled) return;
            _targetCamera.clearFlags = CameraClearFlags.Skybox;
        }

        #endregion
    }
}
