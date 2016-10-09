using UnityEngine;
using System.Collections.Generic;
using Klak.Math;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Mixing/Float Selector")]
    public class FloatSelector : NodeBase
    {
        #region Node I/O

        [Inlet]
        public float input1 {
            set {
                if (!enabled) return;
                _input1Value = value;
                _outputEvent.Invoke(MixValues());
            }
        }

        [Inlet]
        public float input2 {
            set {
                if (!enabled) return;
                _input2Value = value;
                _outputEvent.Invoke(MixValues());
            }
        }

        [Inlet]
        public float parameter {
            set {
                if (!enabled) return;
                _parameter = value;
                _outputEvent.Invoke(MixValues());
            }
        }

        [SerializeField, Outlet]
        FloatEvent _outputEvent = new FloatEvent();

        #endregion

        #region Private members

        float _input1Value;
        float _input2Value;
        float _parameter;

        float MixValues()
        {
            return BasicMath.Lerp(_input1Value, _input2Value, _parameter);
        }

        #endregion
    }
}
