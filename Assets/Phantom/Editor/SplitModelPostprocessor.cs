using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Phantom
{
    public class SplitModelPostprocessor : AssetPostprocessor
    {
        void OnPostprocessModel(GameObject go)
        {
            if (assetPath.IndexOf("Split.fbx") >= 0)
                foreach (var smr in go.GetComponentsInChildren<MeshFilter>())
                    ProcessModelInplace(smr.sharedMesh);
        }

        static void ProcessModelInplace(Mesh mesh)
        {
            // - Make all vertices unique.
            // - Embed adjacent vertex positions into UV1 & UV2.

            var ia_i = mesh.triangles;
            var va_i = mesh.vertices;
            var na_i = mesh.normals;
            var ta_i = mesh.tangents;
            var uv_i = mesh.uv;

            var vcount = ia_i.Length;
            var va_o = new List<Vector3>(vcount);
            var na_o = new List<Vector3>(vcount);
            var ta_o = new List<Vector4>(vcount);
            var uv_o = new List<Vector2>(vcount);
            var v1_o = new List<Vector3>(vcount);
            var v2_o = new List<Vector3>(vcount);

            for (var i = 0; i < vcount;)
            {
                var v0 = ia_i[i++];
                var v1 = ia_i[i++];
                var v2 = ia_i[i++];

                va_o.Add(va_i[v0]);
                va_o.Add(va_i[v1]);
                va_o.Add(va_i[v2]);

                na_o.Add(na_i[v0]);
                na_o.Add(na_i[v1]);
                na_o.Add(na_i[v2]);

                ta_o.Add(ta_i[v0]);
                ta_o.Add(ta_i[v1]);
                ta_o.Add(ta_i[v2]);

                uv_o.Add(uv_i[v0]);
                uv_o.Add(uv_i[v1]);
                uv_o.Add(uv_i[v2]);

                v1_o.Add(va_i[v1]);
                v1_o.Add(va_i[v2]);
                v1_o.Add(va_i[v0]);

                v2_o.Add(va_i[v2]);
                v2_o.Add(va_i[v0]);
                v2_o.Add(va_i[v1]);
            }

            mesh.SetVertices(va_o);
            mesh.SetNormals(na_o);
            mesh.SetTangents(ta_o);
            mesh.SetUVs(0, uv_o);
            mesh.SetUVs(1, v1_o);
            mesh.SetUVs(2, v2_o);

            var ia_o = new int[vcount];
            for (var i = 0; i < vcount; i++) ia_o[i] = i;
            mesh.SetTriangles(ia_o, 0);

            mesh.Optimize();
            mesh.UploadMeshData(true);
        }
    }
}
