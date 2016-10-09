using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Signaling/Float Slot")]
    public class FloatSlot : NodeBase
    {
        #region Node I/O

        [SerializeField, Outlet]
        FloatEvent _outputEvent = new FloatEvent();

        #endregion

        #region Slot/Signal functions

        public void Signal(float value)
        {
            if (enabled) _outputEvent.Invoke(value);
        }

        #endregion
    }
}
