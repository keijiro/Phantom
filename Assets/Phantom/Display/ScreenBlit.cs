using UnityEngine;
using UnityEngine.Rendering;

namespace Phantom
{
    [ExecuteInEditMode]
    public class ScreenBlit : MonoBehaviour
    {
        [SerializeField] RenderTexture _source;

        [SerializeField, Range(0, 1)] float _invert = 0;

        public float invert {
            get { return _invert; }
            set { _invert = value; }
        }

        [SerializeField] Color _overlayColor;

        public Color overlayColor {
            get { return _overlayColor; }
            set { _overlayColor = value; }
        }

        [SerializeField, Range(0, 1)] float _intensity = 1;

        public float intensity {
            get { return _intensity; }
            set { _intensity = value; }
        }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;
        CommandBuffer _blitCommand;

        void OnEnable()
        {
            _material = new Material(Shader.Find("Hidden/Phantom/ScreenBlit"));
            _material.hideFlags = HideFlags.HideAndDontSave;

            if (_blitCommand == null) _blitCommand = new CommandBuffer();
            _blitCommand.Clear();
            _blitCommand.Blit((Texture)_source, BuiltinRenderTextureType.CurrentActive, _material, 0);

            GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterEverything, _blitCommand);
        }

        void OnDisable()
        {
            GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.AfterEverything, _blitCommand);
        }

        void OnDestroy()
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);

            _material = null;
        }

        void LateUpdate()
        {
            _material.SetFloat("_Invert", _invert);
            _material.SetColor("_Color", _overlayColor);
            _material.SetFloat("_Intensity", _intensity);
        }
    }
}
