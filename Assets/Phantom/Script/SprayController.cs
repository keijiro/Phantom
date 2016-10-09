using UnityEngine;

namespace Phantom
{
    [RequireComponent(typeof(Kvant.SprayMV))]
    public class SprayController : MonoBehaviour
    {
        [SerializeField]
        Transform _target;

        [SerializeField, Range(0, 2)]
        float _applyVelocity;

        Kvant.SprayMV _spray;

        void Start()
        {
            _spray = GetComponent<Kvant.SprayMV>();
        }

        void Update()
        {
            var delta = _target.position - _spray.emitterCenter;

            _spray.emitterCenter = _target.position;

            if (_applyVelocity > 0)
                _spray.initialVelocity =
                    delta * (_applyVelocity / Time.deltaTime);
        }
    }
}

