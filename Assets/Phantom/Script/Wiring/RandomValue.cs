using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Convertion/Random Value")]
    public class RandomValue : NodeBase
    {
        #region Editable properties

        [SerializeField]
        float _minimum = 0;

        [SerializeField]
        float _maximum = 1;

        #endregion

        #region Node I/O

        [Inlet]
        public void Bang()
        {
            if (!enabled) return;

            _outputEvent.Invoke(Random.Range(_minimum, _maximum));
        }

        [SerializeField, Outlet]
        FloatEvent _outputEvent = new FloatEvent();

        #endregion
    }
}
