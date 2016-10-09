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
using System.Collections.Generic;
using System.Linq;

namespace Kvant
{
    public class SwarmMVTemplate : ScriptableObject
    {
        #region Public properties

        /// Number of lines (editable)
        public int lineCount {
            get { return Mathf.Clamp(_lineCount, 1, 1024); }
        }

        [SerializeField] int _lineCount = 128;

        /// History length (read only)
        public int historyLength {
            get { return _historyLength; }
        }

        [SerializeField] int _historyLength;

        /// Tmplate mesh (read only)
        public Mesh mesh {
            get { return _mesh; }
        }

        [SerializeField] Mesh _mesh;

        #endregion

        #region Public methods

        #if UNITY_EDITOR

        // Template mesh rebuilding method
        public void RebuildMesh()
        {
            // Working buffers
            var vtx_out = new List<Vector3>();
            var uv0_out = new List<Vector2>();
            var idx_out = new List<int>();

            // Calculate the history length.
            _historyLength = 65535 / (lineCount * 2) + 2;

            // Vertex array
            for (var y = 0; y < lineCount; y++)
            {
                var v = (y + 0.5f) / lineCount;

                for (var x = 1; x < _historyLength - 1; x++)
                {
                    var u = (x + 0.5f) / _historyLength;

                    vtx_out.Add(Vector3.right * -0.5f);
                    vtx_out.Add(Vector3.right * +0.5f);

                    uv0_out.Add(new Vector2(u, v));
                    uv0_out.Add(new Vector2(u, v));
                }
            }

            // Index array
            for (var y = 0; y < lineCount; y++)
            {
                var vi = y * (_historyLength - 2) * 2;

                for (var x = 0; x < _historyLength - 3; x++)
                {
                    idx_out.Add(vi);
                    idx_out.Add(vi + 2);
                    idx_out.Add(vi + 1);

                    idx_out.Add(vi + 1);
                    idx_out.Add(vi + 2);
                    idx_out.Add(vi + 3);

                    vi += 2;
                }
            }

            // Reset the mesh asset.
            _mesh.Clear();
            _mesh.SetVertices(vtx_out);
            _mesh.SetUVs(0, uv0_out);
            _mesh.SetIndices(idx_out.ToArray(), MeshTopology.Triangles, 0);
            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);
            _mesh.Optimize();
            _mesh.UploadMeshData(true);
        }

        #endif

        #endregion

        #region ScriptableObject functions

        void OnEnable()
        {
            if (_mesh == null) {
                _mesh = new Mesh();
                _mesh.name = "SwarmMV Template";
            }
        }

        #endregion
    }
}
