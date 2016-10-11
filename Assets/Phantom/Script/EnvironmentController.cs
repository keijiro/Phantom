using UnityEngine;

namespace Phantom
{
    public class EnvironmentController : MonoBehaviour
    {
        [SerializeField, Range(0, 2)] float _intensity = 1.0f;

        public float intensity {
            get { return _intensity; }
            set { _intensity = value; }
        }

        [SerializeField] bool _modifyFog;

        Material _cameraSkybox;
        float _cameraSkyboxExposure;

        Color _fogColor;

        static Material GetSkyboxMaterial()
        {
            var skyboxComponent = Camera.main.GetComponent<Skybox>();
            if (skyboxComponent != null && skyboxComponent.material != null)
                return skyboxComponent.material;
            else
                return RenderSettings.skybox;
        }

        static void SetSkyboxMaterial(Material material)
        {
            var skyboxComponent = Camera.main.GetComponent<Skybox>();
            if (skyboxComponent != null)
                skyboxComponent.material = material;
            else
                RenderSettings.skybox = material;
        }

        void Start()
        {
            // Make a clone of a skybox material that is set to the main camera.
            var skybox = GetSkyboxMaterial();
            if (skybox != null) {
                _cameraSkybox = new Material(skybox);
                _cameraSkyboxExposure = _cameraSkybox.GetFloat("_Exposure");
                SetSkyboxMaterial(_cameraSkybox);
            }

            _fogColor = RenderSettings.fogColor;
        }

        void LateUpdate()
        {
            RenderSettings.ambientIntensity = _intensity;
            RenderSettings.reflectionIntensity = _intensity;

            if (_cameraSkybox != null) {
                var exp = _cameraSkyboxExposure * _intensity;
                _cameraSkybox.SetFloat("_Exposure", exp);
            }

            if (_modifyFog)
                RenderSettings.fogColor = _fogColor * _intensity;
        }
    }
}
