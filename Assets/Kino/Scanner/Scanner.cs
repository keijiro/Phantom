using UnityEngine;

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Scanner")]
    public class Scanner : MonoBehaviour
    {
        #region Common property

        [Header("Global Settings")]

        [SerializeField] Vector3 _axis = Vector3.forward;
        [SerializeField, Range(0.1f, 100.0f)] float _exponent = 1;
        [SerializeField] float _fallOff = 50;

        [Header("Channel 1")]

        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)] Color _color1 = Color.red;
        [SerializeField] float _interval1 = 1;
        [SerializeField] float _speed1 = 1;

        public Color color1 {
            get { return _color1; }
            set { _color1 = value; }
        }

        [Header("Channel 2")]

        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)] Color _color2 = Color.green;
        [SerializeField] float _interval2 = 1;
        [SerializeField] float _speed2 = 1;

        public Color color2 {
            get { return _color2; }
            set { _color2 = value; }
        }

        [Header("Channel 3")]

        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)] Color _color3 = Color.blue;
        [SerializeField] float _interval3 = 1;
        [SerializeField] float _speed3 = 1;

        public Color color3 {
            get { return _color3; }
            set { _color3 = value; }
        }

        #endregion

        #region Private members

        [SerializeField, HideInInspector] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour functions

        void OnEnable()
        {
            _material = new Material(Shader.Find("Hidden/Kino/Scanner"));
            _material.hideFlags = HideFlags.HideAndDontSave;

            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        }

        void OnDestroy()
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
            _material = null;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var matrix = GetComponent<Camera>().cameraToWorldMatrix;
            _material.SetMatrix("_InverseView", matrix);

            _material.SetVector("_Axis", _axis);
            _material.SetFloat("_Exponent", _exponent);
            _material.SetFloat("_FallOff", 1 / _fallOff);

            _material.SetColor("_Color1", _color1);
            _material.SetColor("_Color2", _color2);
            _material.SetColor("_Color3", _color3);

            _material.SetFloat("_Scale1", 1 / _interval1);
            _material.SetFloat("_Scale2", 1 / _interval2);
            _material.SetFloat("_Scale3", 1 / _interval3);

            _material.SetFloat("_Speed1", _speed1);
            _material.SetFloat("_Speed2", _speed2);
            _material.SetFloat("_Speed3", _speed3);

            Graphics.Blit(source, destination, _material, 0);
        }

        #endregion
    }
}
