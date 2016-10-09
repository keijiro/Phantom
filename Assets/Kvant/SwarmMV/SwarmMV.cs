//
// Kvant/SwarmMV - "Swarm" with motion vector support
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;

namespace Kvant
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    [AddComponentMenu("Kvant/Swarm MV")]
    public class SwarmMV : MonoBehaviour
    {
        #region Basic settings

        [SerializeField] SwarmMVTemplate _template;

        [SerializeField, Range(0, 1)] float _throttle = 1.0f;

        public float throttle {
            get { return _throttle; }
            set { _throttle = value; }
        }

        [SerializeField] float _lineWidth = 0.1f;

        public float lineWidth {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        [SerializeField, Range(0, 1)] float _lineWidthRandomness = 0.5f;

        public float lineWidthRandomness {
            get { return _lineWidthRandomness; }
            set { _lineWidthRandomness = value; }
        }

        [SerializeField, Range(0, 10)] float _twist = 2;

        public float twist {
            get { return _twist; }
            set { _twist = value; }
        }

        [SerializeField] Vector3 _flow = Vector3.zero;

        public Vector3 flow {
            get { return _flow; }
            set { _flow = value; }
        }

        [SerializeField] int _randomSeed = 0;

        #endregion

        #region Attractor parameters

        [SerializeField] Transform _attractor;

        public Transform attractor {
            get { return _attractor; }
            set { _attractor = value; }
        }

        [SerializeField] Vector3 _attractorPosition = Vector3.zero;

        public Vector3 attractorPosition {
            get { return _attractorPosition; }
            set { _attractorPosition = value; }
        }

        [SerializeField] float _attractorRadius = 0.1f;

        public float attractorRadius {
            get { return _attractorRadius; }
            set { _attractorRadius = value; }
        }

        [SerializeField] float _forcePerDistance = 2.0f;

        public float forcePerDistance {
            get { return _forcePerDistance; }
            set { _forcePerDistance = value; }
        }

        [SerializeField, Range(0, 1)] float _forceRandomness = 0.2f;

        public float forceRandomness {
            get { return _forceRandomness; }
            set { _forceRandomness = value; }
        }

        [SerializeField, Range(0, 6)] float _drag = 2.0f;

        public float drag {
            get { return _drag; }
            set { _drag = value; }
        }

        #endregion

        #region Noise field

        [SerializeField] float _noiseAmplitude = 1.5f;

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        [SerializeField] float _noiseFrequency = 0.1f;

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        [SerializeField] float _noiseSpread = 0.5f;

        public float noiseSpread {
            get { return _noiseSpread; }
            set { _noiseSpread = value; }
        }

        [SerializeField] float _noiseSpeed = 0.5f;

        public float noiseSpeed {
            get { return _noiseSpeed; }
            set { _noiseSpeed = value; }
        }

        [SerializeField] float _swirlAmplitude = 0.1f;

        public float swirlAmplitude {
            get { return _swirlAmplitude; }
            set { _swirlAmplitude = value; }
        }

        [SerializeField] float _swirlFrequency = 0.15f;

        public float swirlFrequency {
            get { return _swirlFrequency; }
            set { _swirlFrequency = value; }
        }

        #endregion

        #region Public functions

        #if UNITY_EDITOR

        public void RequestReconfigurationFromEditor()
        {
            _reconfigured = true;
        }

        #endif

        #endregion

        #region Private members

        // References to the built-in assets
        [SerializeField] Shader _kernels;
        [SerializeField] Material _defaultMaterial;

        // Temporary objects for simulation
        Material _material;
        RenderTexture _positionBuffer1;
        RenderTexture _positionBuffer2;
        RenderTexture _velocityBuffer1;
        RenderTexture _velocityBuffer2;
        RenderTexture _basisBuffer1;
        RenderTexture _basisBuffer2;

        // Variables for simulation
        Vector3 _noiseOffset;

        // Custom properties applied to the mesh renderer.
        MaterialPropertyBlock _propertyBlock;

        // Reset flag
        bool _reconfigured = true;

        // Create a buffer for simulation.
        RenderTexture CreateSimulationBuffer(bool forVelocity)
        {
            var format = RenderTextureFormat.ARGBFloat;
            var width = forVelocity ? 1 : _template.historyLength;
            var height = _template.lineCount;
            var buffer = new RenderTexture(width, height, 0, format);
            buffer.hideFlags = HideFlags.HideAndDontSave;
            buffer.filterMode = FilterMode.Point;
            buffer.wrapMode = TextureWrapMode.Clamp;
            return buffer;
        }

        // Try to release a temporary object.
        void ReleaseObject(Object o)
        {
            if (o != null)
                if (Application.isPlaying)
                    Destroy(o);
                else
                    DestroyImmediate(o);
        }

        // Create and initialize internal temporary objects.
        void SetUpTemporaryObjects()
        {
            if (_material == null)
            {
                var shader = Shader.Find("Hidden/Kvant/SwarmMV/Kernels");
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }

            if (_positionBuffer1 == null) _positionBuffer1 = CreateSimulationBuffer(false);
            if (_positionBuffer2 == null) _positionBuffer2 = CreateSimulationBuffer(false);
            if (_velocityBuffer1 == null) _velocityBuffer1 = CreateSimulationBuffer(true);
            if (_velocityBuffer2 == null) _velocityBuffer2 = CreateSimulationBuffer(true);
            if (_basisBuffer1 == null) _basisBuffer1 = CreateSimulationBuffer(false);
            if (_basisBuffer2 == null) _basisBuffer2 = CreateSimulationBuffer(false);
        }

        // Release internal temporary objects.
        void ReleaseTemporaryObjects()
        {
            ReleaseObject(_material); _material = null;
            ReleaseObject(_positionBuffer1); _positionBuffer1 = null;
            ReleaseObject(_positionBuffer2); _positionBuffer2 = null;
            ReleaseObject(_velocityBuffer1); _velocityBuffer1 = null;
            ReleaseObject(_velocityBuffer2); _velocityBuffer2 = null;
            ReleaseObject(_basisBuffer1); _basisBuffer1 = null;
            ReleaseObject(_basisBuffer2); _basisBuffer2 = null;
        }

        // Reset the simulation state.
        void ResetSimulationState()
        {
            _noiseOffset = Vector3.one * _randomSeed;

            UpdateSimulationParameters(0);

            Graphics.Blit(null, _positionBuffer2, _material, 0);
            Graphics.Blit(null, _velocityBuffer2, _material, 1);
            Graphics.Blit(null, _basisBuffer2, _material, 2);
        }

        // Update the parameters in the simulation kernels.
        void UpdateSimulationParameters(float dt)
        {
            var m = _material;

            // Basic parameters
            m.SetVector("_Flow", _flow);
            m.SetFloat("_DeltaTime", dt);
            m.SetFloat("_RandomSeed", _randomSeed);
            m.SetFloat("_Twist", 1 - Mathf.Exp(-_twist * dt));

            // Dynamics
            var minForce = _forcePerDistance * (1 - _forceRandomness);
            var drag = Mathf.Exp(-_drag * dt);
            m.SetVector("_Acceleration", new Vector3(minForce, _forcePerDistance, drag));

            // Attractor position
            var pos = _attractor ? _attractor.position : _attractorPosition;
            pos = transform.InverseTransformPoint(pos);
            m.SetVector("_Attractor", new Vector4(pos.x, pos.y, pos.z, _attractorRadius));

            // Noise field
            _noiseOffset += (_flow + Vector3.one * _noiseSpeed) * dt;
            m.SetVector("_NoiseParams", new Vector3(_noiseAmplitude, _noiseFrequency, _noiseSpread));
            m.SetVector("_NoiseOffset", _noiseOffset);
            m.SetVector("_SwirlParams", new Vector2(_swirlAmplitude, _swirlFrequency));
        }

        // Invoke the simulation kernels.
        void InvokeSimulationKernels(float dt)
        {
            // Swap the buffers.
            var tempPosition = _positionBuffer1;
            var tempVelocity = _velocityBuffer1;
            var tempBasis = _basisBuffer1;

            _positionBuffer1 = _positionBuffer2;
            _velocityBuffer1 = _velocityBuffer2;
            _basisBuffer1 = _basisBuffer2;

            _positionBuffer2 = tempPosition;
            _velocityBuffer2 = tempVelocity;
            _basisBuffer2 = tempBasis;

            // Invoke the velocity update kernel.
            UpdateSimulationParameters(dt);
            _material.SetTexture("_PositionBuffer", _positionBuffer1);
            _material.SetTexture("_VelocityBuffer", _velocityBuffer1);
            Graphics.Blit(null, _velocityBuffer2, _material, 4);

            // Invoke the position update kernel.
            _material.SetTexture("_VelocityBuffer", _velocityBuffer2);
            Graphics.Blit(null, _positionBuffer2, _material, 3);

            // Invoke the orthonormal basis update kernel.
            _material.SetTexture("_PositionBuffer", _positionBuffer2);
            _material.SetTexture("_BasisBuffer", _basisBuffer1);
            Graphics.Blit(null, _basisBuffer2, _material, 5);
        }

        // Update external components: mesh filter.
        void UpdateMeshFilter()
        {
            var meshFilter = GetComponent<MeshFilter>();

            // Add a new mesh filter if missing.
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
                meshFilter.hideFlags = HideFlags.NotEditable;
            }

            if (meshFilter.sharedMesh != _template.mesh)
                meshFilter.sharedMesh = _template.mesh;
        }

        // Update external components: mesh renderer.
        void UpdateMeshRenderer()
        {
            var meshRenderer = GetComponent<MeshRenderer>();

            if (_propertyBlock == null)
                _propertyBlock = new MaterialPropertyBlock();

            var pb = _propertyBlock;
            pb.SetTexture("_PositionBuffer", _positionBuffer2);
            pb.SetTexture("_BasisBuffer", _basisBuffer2);
            pb.SetTexture("_PreviousPositionBuffer", _positionBuffer1);
            pb.SetTexture("_PreviousBasisBuffer", _basisBuffer1);
            pb.SetVector("_LineWidth", new Vector2(1 -_lineWidthRandomness, 1) * _lineWidth);
            pb.SetFloat("_Throttle", _throttle);
            pb.SetFloat("_RandomSeed", _randomSeed);

            meshRenderer.SetPropertyBlock(pb);

            // Set the default material if no material is set.
            if (meshRenderer.sharedMaterial == null)
                meshRenderer.sharedMaterial = _defaultMaterial;
        }

        #endregion

        #region MonoBehaviour functions

        void Reset()
        {
            _reconfigured = true;
        }

        void OnDestroy()
        {
            ReleaseTemporaryObjects();
        }

        void LateUpdate()
        {
            // Do nothing if no template is set.
            if (_template == null) return;

            if (_reconfigured)
            {
                // Initialize temporary objects at the first frame,
                // and re-initialize them on configuration changes.
                ReleaseTemporaryObjects();
                SetUpTemporaryObjects();
                ResetSimulationState();
                _reconfigured = false;
            }

            if (Application.isPlaying)
            {
                // Advance simulation time.
                InvokeSimulationKernels(Time.deltaTime);
            }
            else
            {
                // Editor: Simulate 2 seconds from the initial state.
                ResetSimulationState();
                for (var i = 0; i < 40; i++) 
                    InvokeSimulationKernels(1.0f / 20);
            }

            // Update external components (mesh filter and renderer).
            UpdateMeshFilter();
            UpdateMeshRenderer();
        }

        void OnDrawGizmosSelected()
        {
            // Show the attractor position.
            var atpos = _attractor ? _attractor.position : _attractorPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(atpos, 0.1f);
        }

        #endregion
    }
}
