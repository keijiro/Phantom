using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Convertion/HueToColor")]
    public class HueToColor : NodeBase
    {
        #region Editable properties

        [SerializeField, Range(0, 1)]
        float _saturation = 1;

        [SerializeField]
        float _brightness = 1;

        #endregion

        #region Node I/O

        [Inlet]
        public float hue {
            set {
                if (!enabled) return;

                var color = Color.HSVToRGB(value, _saturation, _brightness);
                _colorEvent.Invoke(color);
            }
        }

        [SerializeField, Outlet]
        ColorEvent _colorEvent = new ColorEvent();

        #endregion
    }
}
