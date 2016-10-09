//
// Kvant/Line - One dimensional object array renderer
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
    [AddComponentMenu("Kvant/Line")]
    public class Line : MonoBehaviour
    {
        #region Basic settings

        [SerializeField] LineTemplate _template;

        [SerializeField] float _length = 2000;

        public float length {
            get { return _length; }
            set { _length = value; }
        }

        [SerializeField] float _scrollSpeed = 10;

        public float scrollSpeed {
            get { return _scrollSpeed; }
            set { _scrollSpeed = value; }
        }

        [SerializeField, Range(0, 1)] float _noiseDirectivity = 0.8f;

        public float noiseDirectivity {
            get { return _noiseDirectivity; }
            set { _noiseDirectivity = value; }
        }

        [SerializeField] int _randomSeed = 0;

        public int randomSeed {
            get { return _randomSeed; }
            set { _randomSeed = value; }
        }

        #endregion

        #region Position parameters

        [SerializeField] float _displaceFrequency = 0.1f;

        public float displaceFrequency {
            get { return _displaceFrequency; }
            set { _displaceFrequency = value; }
        }

        [SerializeField] float _displaceAmplitude = 1;

        public float displaceAmplitude {
            get { return _displaceAmplitude; }
            set { _displaceAmplitude = value; }
        }

        [SerializeField] float _displaceSpeed = 0.1f;

        public float displaceSpeed {
            get { return _displaceSpeed; }
            set { _displaceSpeed = value; }
        }

        #endregion

        #region Rotation parameters

        [SerializeField] float _twistFrequency = 0.1f;

        public float twistFrequency {
            get { return _twistFrequency; }
            set { _twistFrequency = value; }
        }

        [SerializeField] float _twistAmplitude = 180;

        public float twistAmplitude {
            get { return _twistAmplitude; }
            set { _twistAmplitude = value; }
        }

        [SerializeField] float _twistSpeed = 0.1f;

        public float twistSpeed {
            get { return _twistSpeed; }
            set { _twistSpeed = value; }
        }

        [SerializeField] float _tiltFrequency = 0.1f;

        public float tiltFrequency {
            get { return _tiltFrequency; }
            set { _tiltFrequency = value; }
        }

        [SerializeField] float _tiltAmplitude = 30;

        public float tiltAmplitude {
            get { return _tiltAmplitude; }
            set { _tiltAmplitude = value; }
        }

        [SerializeField] float _tiltSpeed = 0.1f;

        public float tiltSpeed {
            get { return _tiltSpeed; }
            set { _tiltSpeed = value; }
        }

        #endregion

        #region Scale parameters

        [SerializeField] float _baseScale = 1;

        public float baseScale {
            get { return _baseScale; }
            set { _baseScale = value; }
        }

        [SerializeField] float _scaleFrequency = 0.1f;

        public float scaleFrequency {
            get { return _scaleFrequency; }
            set { _scaleFrequency = value; }
        }

        [SerializeField, Range(0, 1)] float _scaleAmplitude = 0.5f;

        public float scaleAmplitude {
            get { return _scaleAmplitude; }
            set { _scaleAmplitude = value; }
        }

        [SerializeField] float _scaleSpeed = 0.1f;

        public float scaleSpeed {
            get { return _scaleSpeed; }
            set { _scaleSpeed = value; }
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
        RenderTexture _rotationBuffer1;
        RenderTexture _rotationBuffer2;

        // Variables for simulation
        Vector2 _displaceOffset;
        Vector2 _twistOffset;
        Vector2 _tiltOffset;
        Vector2 _scaleOffset;
        float _scroll;

        // Custom properties applied to the mesh renderer.
        MaterialPropertyBlock _propertyBlock;

        // Reset flag
        bool _reconfigured = true;

        // Create a buffer for simulation.
        RenderTexture CreateSimulationBuffer()
        {
            var format = RenderTextureFormat.ARGBFloat;
            var width = _template.instanceCount;
            var buffer = new RenderTexture(width, 1, 0, format);
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
                var shader = Shader.Find("Hidden/Kvant/Line/Kernels");
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }

            if (_positionBuffer1 == null) _positionBuffer1 = CreateSimulationBuffer();
            if (_positionBuffer2 == null) _positionBuffer2 = CreateSimulationBuffer();
            if (_rotationBuffer1 == null) _rotationBuffer1 = CreateSimulationBuffer();
            if (_rotationBuffer2 == null) _rotationBuffer2 = CreateSimulationBuffer();
        }

        // Release internal temporary objects.
        void ReleaseTemporaryObjects()
        {
            ReleaseObject(_material); _material = null;
            ReleaseObject(_positionBuffer1); _positionBuffer1 = null;
            ReleaseObject(_positionBuffer2); _positionBuffer2 = null;
            ReleaseObject(_rotationBuffer1); _rotationBuffer1 = null;
            ReleaseObject(_rotationBuffer2); _rotationBuffer2 = null;
        }

        // Reset the simulation state.
        void ResetSimulationState()
        {
            _displaceOffset = Vector3.one * _randomSeed;
            _twistOffset = new Vector3(83.48f, 97.89f, 15.28f) * _randomSeed;
            _tiltOffset = new Vector3(34.18f, 97.15f, 37.95f) * _randomSeed;
            _scaleOffset = new Vector3(73.84f, 29.78f, 29.41f) * _randomSeed;
            _scroll = 0;

            UpdateSimulationParameters(0);

            Graphics.Blit(null, _positionBuffer2, _material, 0);
            Graphics.Blit(null, _rotationBuffer2, _material, 1);
        }

        // Update the parameters in the simulation kernels.
        void UpdateSimulationParameters(float dt)
        {
            var m = _material;

            // Basic settings
            m.SetFloat("_Length", _length);
            m.SetFloat("_BaseScale", _baseScale);
            m.SetFloat("_RandomSeed", _randomSeed);

            // Scrolling
            _scroll += _scrollSpeed * dt;
            m.SetFloat("_Scroll", _scroll / _length);

            // Noise field animation
            var theta = _noiseDirectivity * Mathf.PI * 0.5f;
            var noiseDelta = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta));

            _displaceOffset += noiseDelta * (_displaceSpeed * dt);
            _twistOffset += noiseDelta * (_twistSpeed * dt);
            _tiltOffset += noiseDelta * (_tiltSpeed * dt);
            _scaleOffset += noiseDelta * (_scaleSpeed * dt);

            // Noise field parameters
            m.SetVector("_Displace", new Vector4(
                _displaceOffset.x, _displaceOffset.y,
                _displaceFrequency, _displaceAmplitude
            ));

            m.SetVector("_Twist", new Vector4(
                _twistOffset.x, _twistOffset.y,
                _twistFrequency, _twistAmplitude * Mathf.Deg2Rad
            ));

            m.SetVector("_Tilt", new Vector4(
                _tiltOffset.x, _tiltOffset.y,
                _tiltFrequency, _tiltAmplitude * Mathf.Deg2Rad
            ));

            m.SetVector("_Scale", new Vector4(
                _scaleOffset.x, _scaleOffset.y,
                _scaleFrequency, _scaleAmplitude
            ));
        }

        // Invoke the simulation kernels.
        void InvokeSimulationKernels(float dt)
        {
            // Swap the buffers.
            var tempPosition = _positionBuffer1;
            var tempRotation = _rotationBuffer1;
            _positionBuffer1 = _positionBuffer2;
            _rotationBuffer1 = _rotationBuffer2;
            _positionBuffer2 = tempPosition;
            _rotationBuffer2 = tempRotation;

            // Invocation
            UpdateSimulationParameters(dt);
            Graphics.Blit(null, _positionBuffer2, _material, 0);
            Graphics.Blit(null, _rotationBuffer2, _material, 1);
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

            _propertyBlock.SetTexture("_PositionBuffer", _positionBuffer2);
            _propertyBlock.SetTexture("_RotationBuffer", _rotationBuffer2);
            _propertyBlock.SetTexture("_PreviousPositionBuffer", _positionBuffer1);
            _propertyBlock.SetTexture("_PreviousRotationBuffer", _rotationBuffer1);
            _propertyBlock.SetFloat("_RandomSeed", _randomSeed);

            meshRenderer.SetPropertyBlock(_propertyBlock);

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
                // Editor: Simulate only the first frame.
                ResetSimulationState();
                InvokeSimulationKernels(1.0f / 30);
            }

            // Update external components (mesh filter and renderer).
            UpdateMeshFilter();
            UpdateMeshRenderer();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.1f, 0.1f, _length));
        }

        #endregion
    }
}
