using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Csg
{
    public static class Utils
    {
        /// <summary>
        /// Convert geometry to buffer geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static BufferGeometry ToBufferGeometry(this Geometry geometry)
        {
            var coordBuffer = new List<float>();

            for (int i = 0; i < geometry.Faces.Count(); i++)
            {
                var faces = geometry.Faces[i];
                var va = geometry.Vertices[faces.A];
                var vb = geometry.Vertices[faces.B];
                var vc = geometry.Vertices[faces.C];

                coordBuffer.Add(va.X);
                coordBuffer.Add(va.Y);
                coordBuffer.Add(va.Z);

                coordBuffer.Add(vb.X);
                coordBuffer.Add(vb.Y);
                coordBuffer.Add(vb.Z);

                coordBuffer.Add(vc.X);
                coordBuffer.Add(vc.Y);
                coordBuffer.Add(vc.Z);
            }

            var bufferAttribute = new BufferAttribute<float>(coordBuffer.ToArray(), 3);
            var bufferGeometry = new BufferGeometry();

            bufferGeometry.SetAttribute("position", bufferAttribute);
            bufferGeometry.ComputeBoundingBox();
            bufferGeometry.ComputeVertexNormals();
            bufferGeometry.ComputeFaceNormals();

            return bufferGeometry;
        }

        /// <summary>
        /// Compute BufferGeometry UVs
        /// </summary>
        /// <param name="bufferGeometry"></param>
        /// <param name="transformMatrix"></param>
        public static void ComputeUv(BufferGeometry bufferGeometry, Matrix4 transformMatrix)
        {
            bufferGeometry.ComputeBoundingBox();
            bufferGeometry.ComputeVertexNormals();

            if (transformMatrix == null)
            {
                transformMatrix = new Matrix4();
            }

            var geom = bufferGeometry;
            geom.ComputeBoundingBox();
            var bbox = geom.BoundingBox;

            var bboxSizeX = bbox.Max.X - bbox.Min.X;
            var bboxSizeZ = bbox.Max.Z - bbox.Min.Z;
            var bboxSizeY = bbox.Max.Y - bbox.Min.Y;

            var size = Mathf.Max(Mathf.Max(bboxSizeX, bboxSizeY), bboxSizeZ);

            var uvBbox = new Box3(new Vector3(-size / 2, -size / 2, -size / 2), new Vector3(size / 2, size / 2, size / 2));

            Utils.ApplyUv(bufferGeometry, transformMatrix, uvBbox, size);
        }

        private static void ApplyUv(BufferGeometry geom, Matrix4 transformMatrix, Box3 bbox, float bboxMaxSize)
        {
            var position = geom.GetAttribute<float>("position");
            var coords = new float[2 * position.Array.Length / 3];

            var uvs = geom.GetAttribute<float>("uv");
            if (uvs == null)
            {
                geom.SetAttribute("uv", new BufferAttribute<float>(coords.ToArray(), 2));
            }

            //maps 3 verts of 1 face on the better side of the cube
            //side of the cube can be XY, XZ or YZ
            Vector2[] makeUVs(Vector3 v0, Vector3 v1, Vector3 v2)
            {

                //pre-rotate the model so that cube sides match world axis
                v0.ApplyMatrix4(transformMatrix);
                v1.ApplyMatrix4(transformMatrix);
                v2.ApplyMatrix4(transformMatrix);

                //get normal of the face, to know into which cube side it maps better
                var n = new Vector3();
                n.CrossVectors(v1.Clone().Sub(v0), v1.Clone().Sub(v2)).Normalize();

                n.X = Mathf.Abs(n.X);
                n.Y = Mathf.Abs(n.Y);
                n.Z = Mathf.Abs(n.Z);

                var uv0 = new Vector2();
                var uv1 = new Vector2();
                var uv2 = new Vector2();
                // xz mapping
                if (n.Y > n.X && n.Y > n.Z)
                {
                    uv0.X = (v0.X - bbox.Min.X) / bboxMaxSize;
                    uv0.Y = (bbox.Max.Z - v0.Z) / bboxMaxSize;

                    uv1.X = (v1.X - bbox.Min.X) / bboxMaxSize;
                    uv1.Y = (bbox.Max.Z - v1.Z) / bboxMaxSize;

                    uv2.X = (v2.X - bbox.Min.X) / bboxMaxSize;
                    uv2.Y = (bbox.Max.Z - v2.Z) / bboxMaxSize;
                }
                else
                    if (n.X > n.Y && n.X > n.Z)
                {
                    uv0.X = (v0.Z - bbox.Min.Z) / bboxMaxSize;
                    uv0.Y = (v0.Y - bbox.Min.Y) / bboxMaxSize;

                    uv1.X = (v1.Z - bbox.Min.Z) / bboxMaxSize;
                    uv1.Y = (v1.Y - bbox.Min.Y) / bboxMaxSize;

                    uv2.X = (v2.Z - bbox.Min.Z) / bboxMaxSize;
                    uv2.Y = (v2.Y - bbox.Min.Y) / bboxMaxSize;
                }
                else
                        if (n.Z > n.Y && n.Z > n.X)
                {
                    uv0.X = (v0.X - bbox.Min.X) / bboxMaxSize;
                    uv0.Y = (v0.Y - bbox.Min.Y) / bboxMaxSize;

                    uv1.X = (v1.X - bbox.Min.X) / bboxMaxSize;
                    uv1.Y = (v1.Y - bbox.Min.Y) / bboxMaxSize;

                    uv2.X = (v2.X - bbox.Min.X) / bboxMaxSize;
                    uv2.Y = (v2.Y - bbox.Min.Y) / bboxMaxSize;
                }

                return new Vector2[] { uv0, uv1, uv2 };
            };


            if (geom.Index != null)
            {
                for (var vi = 0; vi < geom.Index.Array.Length; vi += 3)
                {
                    var idx0 = (int)geom.Index.Array[vi];
                    var idx1 = (int)geom.Index.Array[vi + 1];
                    var idx2 = (int)geom.Index.Array[vi + 2];

                    var vx0 = position.Array[3 * idx0];
                    var vy0 = position.Array[3 * idx0 + 1];
                    var vz0 = position.Array[3 * idx0 + 2];

                    var vx1 = position.Array[3 * idx1];
                    var vy1 = position.Array[3 * idx1 + 1];
                    var vz1 = position.Array[3 * idx1 + 2];

                    var vx2 = position.Array[3 * idx2];
                    var vy2 = position.Array[3 * idx2 + 1];
                    var vz2 = position.Array[3 * idx2 + 2];

                    var v0 = new Vector3(vx0, vy0, vz0);
                    var v1 = new Vector3(vx1, vy1, vz1);
                    var v2 = new Vector3(vx2, vy2, vz2);

                    var uvsResults = makeUVs(v0, v1, v2);

                    coords[2 * idx0] = uvsResults[0].X;
                    coords[2 * idx0 + 1] = uvsResults[0].Y;

                    coords[2 * idx1] = uvsResults[1].X;
                    coords[2 * idx1 + 1] = uvsResults[1].Y;

                    coords[2 * idx2] = uvsResults[2].X;
                    coords[2 * idx2 + 1] = uvsResults[2].Y;
                }
            }
            else
            {
                for (var vi = 0; vi < position.Array.Length; vi += 9)
                {
                    var vx0 = position.Array[vi];
                    var vy0 = position.Array[vi + 1];
                    var vz0 = position.Array[vi + 2];

                    var vx1 = position.Array[vi + 3];
                    var vy1 = position.Array[vi + 4];
                    var vz1 = position.Array[vi + 5];

                    var vx2 = position.Array[vi + 6];
                    var vy2 = position.Array[vi + 7];
                    var vz2 = position.Array[vi + 8];

                    var v0 = new Vector3(vx0, vy0, vz0);
                    var v1 = new Vector3(vx1, vy1, vz1);
                    var v2 = new Vector3(vx2, vy2, vz2);

                    var uvsResult = makeUVs(v0, v1, v2);

                    var idx0 = vi / 3;
                    var idx1 = idx0 + 1;
                    var idx2 = idx0 + 2;

                    coords[2 * idx0] = uvsResult[0].X;
                    coords[2 * idx0 + 1] = uvsResult[0].Y;

                    coords[2 * idx1] = uvsResult[1].X;
                    coords[2 * idx1 + 1] = uvsResult[1].Y;

                    coords[2 * idx2] = uvsResult[2].X;
                    coords[2 * idx2 + 1] = uvsResult[2].Y;
                }
            }

            geom.SetAttribute("uv", new BufferAttribute<float>(coords.ToArray(), 2));
            uvs = geom.GetAttribute<float>("uv");
            uvs.needsUpdate = true;
        }
    }
}
