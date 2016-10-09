using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Input/Bang Input")]
    public class BangInput : NodeBase
    {
        #region Node I/O

        public void Bang()
        {
            _bangEvent.Invoke();
        }

        [SerializeField, Outlet]
        VoidEvent _bangEvent = new VoidEvent();

        #endregion
    }
}
