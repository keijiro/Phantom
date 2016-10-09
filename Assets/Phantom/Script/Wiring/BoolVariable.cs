using UnityEngine;
using Klak.Math;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Input/Bool Variable")]
    public class BoolVariable : NodeBase
    {
        #region Editable properties

        public bool boolValue {
            set {
                (value ? _trueEvent : _falseEvent).Invoke();
            }
        }

        #endregion

        #region Node I/O

        [SerializeField, Outlet]
        VoidEvent _trueEvent = new VoidEvent();

        [SerializeField, Outlet]
        VoidEvent _falseEvent = new VoidEvent();

        #endregion
    }
}
