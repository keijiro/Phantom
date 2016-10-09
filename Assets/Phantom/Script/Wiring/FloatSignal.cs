using UnityEngine;
using System.Collections.Generic;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Signaling/Float Signal")]
    public class FloatSignal : NodeBase
    {
        #region Node I/O

        [Inlet]
        public float input {
            set {
                if (!enabled) return;
                foreach (var slot in SlotList) slot.Signal(value);
            }
        }

        #endregion

        #region Private members

        List<FloatSlot> _slotList;

        List<FloatSlot> SlotList {
            get {
                if (_slotList == null) _slotList = ScanSlots(this.name);
                return _slotList;
            }
        }

        static List<FloatSlot> ScanSlots(string slotName)
        {
            var list = new List<FloatSlot>();
            foreach (var slot in FindObjectsOfType<FloatSlot>())
                if (slot.name == slotName) list.Add(slot);
            return list;
        }

        #endregion
    }
}
