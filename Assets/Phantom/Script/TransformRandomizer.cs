using UnityEngine;

namespace Phantom
{
    public class TransformRandomizer : MonoBehaviour
    {
        #region Editable properties

        [SerializeField] Vector3 _positionRange;
        [SerializeField] Vector3 _rotationRange;

        [SerializeField, Range(0, 1)] float _amplitude = 1;

        public float amplitude {
            get { return _amplitude; }
            set {
                _amplitude = value;
                UpdateTransform();
            }
        }

        #endregion

        #region Public methods

        public void Clear()
        {
            transform.localPosition = _originalPosition;
            transform.localRotation = _originalRotation;
            _px.Clear();
            _py.Clear();
            _pz.Clear();
            _rx.Clear();
            _ry.Clear();
            _rz.Clear();
        }

        public void Randomize()
        {
            _px.Randomize(0, 1);
            _py.Randomize(0, 1);
            _pz.Randomize(0, 1);
            _rx.Randomize(0, 1);
            _ry.Randomize(0, 1);
            _rz.Randomize(0, 1);
            UpdateTransform();
        }

        public void RandomizeWithMinimumDifference(float minDiff)
        {
            _px.Randomize(minDiff, 1);
            _py.Randomize(minDiff, 1);
            _pz.Randomize(minDiff, 1);
            _rx.Randomize(minDiff, 1);
            _ry.Randomize(minDiff, 1);
            _rz.Randomize(minDiff, 1);
            UpdateTransform();
        }

        public void RandomizeWithConstantDifference(float diff)
        {
            _px.Randomize(diff, diff);
            _py.Randomize(diff, diff);
            _pz.Randomize(diff, diff);
            _rx.Randomize(diff, diff);
            _ry.Randomize(diff, diff);
            _rz.Randomize(diff, diff);
            UpdateTransform();
        }

        #endregion

        #region Randomized scalar value class

        struct RandomValue
        {
            float _current;

            public float _range;

            public float CurrentValue {
                get { return _current * _range; }
            }

            public RandomValue(float range)
            {
                _current = 0;
                _range = range;
            }

            public void Clear()
            {
                _current = 0;
            }

            public void Randomize(float minDiff, float maxDiff)
            {
                var diff = Random.Range(minDiff, maxDiff);

                if (_current - diff < -1)
                {
                    // diff sould be positive.
                }
                else if (_current + diff > 1)
                {
                    // diff should be negative.
                    diff = -diff;
                }
                else
                {
                    // Invert diff randomly.
                    if (Random.value < 0.5f) diff = -diff;
                }

                _current += diff;
            }
        }

        #endregion

        #region Private members

        RandomValue _px;
        RandomValue _py;
        RandomValue _pz;

        RandomValue _rx;
        RandomValue _ry;
        RandomValue _rz;

        Vector3 _originalPosition;
        Quaternion _originalRotation;

        void UpdateTransform()
        {
            var p = new Vector3(_px.CurrentValue, _py.CurrentValue, _pz.CurrentValue);
            var r = new Vector3(_rx.CurrentValue, _ry.CurrentValue, _rz.CurrentValue);

            p *= _amplitude;
            r *= _amplitude;

            transform.localPosition = _originalPosition + p;
            transform.localRotation = Quaternion.Euler(r) * _originalRotation;
        }

        #endregion

        #region MonoBehaviour functions

        void Start()
        {
            _px = new RandomValue(_positionRange.x);
            _py = new RandomValue(_positionRange.y);
            _pz = new RandomValue(_positionRange.z);

            _rx = new RandomValue(_rotationRange.x);
            _ry = new RandomValue(_rotationRange.y);
            _rz = new RandomValue(_rotationRange.z);

            _originalPosition = transform.localPosition;
            _originalRotation = transform.localRotation;
        }

        #endregion
    }
}
