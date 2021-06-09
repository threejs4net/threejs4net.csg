using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net.Core;

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

    }
}
