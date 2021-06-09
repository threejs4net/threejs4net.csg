using System;
using System.Windows.Forms;
using ThreeJs4Net.Geometries;
using ThreeJs4Net.Materials;
using ThreeJs4Net.Math;
using ThreeJs4Net.Objects;
using ThreeJs4Net.Csg;

namespace ThreeJs4Net.Demo
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            //// Make 2 box meshes..
            //var meshA = new Mesh(
            //  new BoxGeometry(0.2f, 0.2f, 0.2f),
            //  new MeshNormalMaterial()
            //);
            //var meshB = new Mesh(new BoxGeometry(0.2f, 0.2f, 0.2f));

            ////// Offset one of the boxes by half its width..
            //meshB.Position.Add(new Vector3(0.1f, 0.1f, 0.1f));

            ////// Make sure the .matrix of each mesh is current
            //meshA.UpdateMatrix();
            //meshB.UpdateMatrix();

            ////// Subtract meshB from meshA
            //var mesh = CSG.subtract(meshA, meshB);


        }
    }
}
