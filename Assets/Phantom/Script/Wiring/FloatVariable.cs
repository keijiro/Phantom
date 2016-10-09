using UnityEngine;
using Klak.Math;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Input/Float Variable")]
    public class FloatVariable : NodeBase
    {
        #region Editable properties

        [SerializeField]
        float _floatValue;

        public float floatValue {
            get { return _floatValue; }
            set { _floatValue = value; }
        }

        [SerializeField]
        FloatInterpolator.Config _interpolator;

        #endregion

        #region Node I/O

        [SerializeField, Outlet]
        FloatEvent _valueEvent = new FloatEvent();

        #endregion

        #region MonoBehaviour functions

        FloatInterpolator _outputValue;

        void Start()
        {
            _outputValue = new FloatInterpolator(_floatValue, _interpolator);
        }

        void Update()
        {
            _outputValue.targetValue = _floatValue;
            _valueEvent.Invoke(_outputValue.Step());
        }

        #endregion
    }
}
