using UnityEngine;

public class EyeballController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _behold;

    public float behold {
        get { return _behold; }
        set { _behold = value; }
    }

    [SerializeField] Transform _motionTarget;

    Vector4 _prevLookAt;

    void Update()
    {
        var lookAt = Camera.main.transform.position;

        transform.position = _motionTarget.position;
        transform.rotation = Quaternion.Slerp(
            _motionTarget.rotation,
            Quaternion.FromToRotation(Vector3.forward, lookAt - _motionTarget.position),
            _behold
        );

        var lookAt2 = (Vector4)lookAt;
        lookAt2.w = _behold;
        Shader.SetGlobalVector("_Eyeball_LookAt", lookAt2);

        Shader.SetGlobalVector("_Eyeball_PreviousLookAt", _prevLookAt);
        _prevLookAt = lookAt2;
    }
}
