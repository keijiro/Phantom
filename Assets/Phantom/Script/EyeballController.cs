using UnityEngine;

public class EyeballController : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _behold;

    public float behold {
        get { return _behold; }
        set { _behold = value; }
    }

    [SerializeField] Transform _target;

    Vector4 _prevLookAt;

    void Update()
    {
        var lookAt = Camera.main.transform.position;

        transform.position = _target.position;
        transform.rotation = Quaternion.Slerp(
            _target.rotation,
            Quaternion.FromToRotation(Vector3.forward, lookAt - _target.position),
            _behold
        );

        var lookAt2 = (Vector4)lookAt;
        lookAt2.w = _behold;
        Shader.SetGlobalVector("_Eyeball_LookAt", lookAt2);

        Shader.SetGlobalVector("_Eyeball_PreviousLookAt", _prevLookAt);
        _prevLookAt = lookAt2;
    }
}
