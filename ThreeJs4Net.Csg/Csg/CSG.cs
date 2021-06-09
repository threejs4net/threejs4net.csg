using System.Collections.Generic;
using ThreeJs4Net.Core;
using ThreeJs4Net.Csg;
using ThreeJs4Net.Materials;
using ThreeJs4Net.Math;
using ThreeJs4Net.Objects;

namespace ThreeJs4Net.Csg
{
    public class CSG
    {


        //import {
        //  BufferAttribute,
        //  BufferGeometry,
        //  Material,
        //  Matrix3,
        //  Matrix4,
        //  Mesh,
        //  Vector3,
        //} from 'three';
        //import { NBuf2, NBuf3 } from './NBuf';
        //import { Node } from './Node';
        //import { Polygon } from './Polygon';
        //import { Vector } from './Vector';
        //import { Vertex } from './Vertex';

        ///**
        // * Holds a binary space partition tree representing a 3D solid. Two solids can
        // * be combined using the `union()`, `subtract()`, and `intersect()` methods.
        // */
        //export class CSG {
        //  static fromPolygons(polygons: Polygon[]): CSG {
        //    const csg = new CSG();
        //    csg.polygons = polygons;
        //    return csg;
        //  }

        public static CSG fromPolygons(IList<Polygon> polygons)
        {
            var csg = new CSG();
            csg.polygons = polygons;
            return csg;
        }

        //  static fromGeometry(geom: BufferGeometry, objectIndex?: any): CSG {
        //    let polys = [];
        //    const posattr = geom.attributes.position;
        //    const normalattr = geom.attributes.normal;
        //    const uvattr = geom.attributes.uv;
        //    const colorattr = geom.attributes.color;
        //    let index;

        //    if (geom.index) {
        //      index = geom.index.array;
        //    } else {
        //      index = new Array((posattr.array.length / posattr.itemSize) | 0);
        //      for (let i = 0; i < index.length; i++) index[i] = i;
        //    }

        //    const triCount = (index.length / 3) | 0;
        //    polys = new Array(triCount);

        //    for (let i = 0, pli = 0, l = index.length; i < l; i += 3, pli++) {
        //      const vertices = new Array(3);
        //      for (let j = 0; j < 3; j++) {
        //        const vi = index[i + j];
        //        const vp = vi * 3;
        //        const vt = vi * 2;
        //        const x = posattr.array[vp];
        //        const y = posattr.array[vp + 1];
        //        const z = posattr.array[vp + 2];
        //        const nx = normalattr.array[vp];
        //        const ny = normalattr.array[vp + 1];
        //        const nz = normalattr.array[vp + 2];
        //        const u = uvattr.array[vt];
        //        const v = uvattr.array[vt + 1];

        //        vertices[j] = new Vertex(
        //          new Vector(x, y, z),
        //          new Vector(nx, ny, nz),
        //          new Vector(u, v, 0),
        //          colorattr &&
        //            new Vector(
        //              colorattr.array[vt],
        //              colorattr.array[vt + 1],
        //              colorattr.array[vt + 2]
        //            )
        //        );
        //      }

        //      polys[pli] = new Polygon(vertices, objectIndex);
        //    }

        //    return CSG.fromPolygons(polys);
        //  }

        //TODO: Review with Leo
        public static CSG fromGeometry(BufferGeometry geom, int? objectIndex = null)
        {
            Polygon[] polys = null;

            //Review with Leo
            var posattr = geom.GetAttribute<float>("position");
            var normalattr = geom.GetAttribute<float>("normal");
            var uvattr = geom.GetAttribute<float>("uv");

            //Review with Leo
            BufferAttribute<float> colorattr = null;
            if (geom.Attributes.ContainsKey("color"))
            {
                colorattr = geom.GetAttribute<float>("color");
            }

            var grps = geom.groups;

            uint[] index;

            if (geom.Index != null)
            {
                index = geom.Index.Array;
            }
            else
            {
                var length = (posattr.Array.Length / posattr.ItemSize) | 0;
                index = new uint[length];
                for (uint i = 0; i < index.Length; i++) index[i] = i;
            }

            var triCount = (index.Length / 3) | 0;
            polys = new Polygon[triCount];

            for (int i = 0, pli = 0, l = index.Length; i < l; i += 3, pli++)
            {
                //Review with Leo
                var vertices = new Vertex[3];
                for (int j = 0; j < 3; j++)
                {
                    var vi = index[i + j];
                    var vp = vi * 3;
                    var vt = vi * 2;
                    var x = posattr.Array[vp];
                    var y = posattr.Array[vp + 1];
                    var z = posattr.Array[vp + 2];
                    var nx = normalattr.Array[vp];
                    var ny = normalattr.Array[vp + 1];
                    var nz = normalattr.Array[vp + 2];
                    var u = uvattr.Array[vt];
                    var v = uvattr.Array[vt + 1];

                    vertices[j] = new Vertex(
                      new Vector(x, y, z),
                      new Vector(nx, ny, nz),
                      new Vector(u, v, 0),
                      //Review with Leo
                      colorattr != null ?
                        new Vector(
                          colorattr.Array[vt],
                          colorattr.Array[vt + 1],
                          colorattr.Array[vt + 2]
                        ) : null
                    );
                }


                if (objectIndex == null && grps != null && grps.Count > 0)
                {
                    foreach (var grp in grps)
                    {
                        if (index[i] >= grp.Start && index[i] < grp.Start + grp.Count)
                        {
                            polys[pli] = new Polygon(vertices, grp.MaterialIndex);
                        }
                    }
                }
                else
                {
                    polys[pli] = new Polygon(vertices, objectIndex);
                }

            }

            return CSG.fromPolygons(polys);
        }

        //  static toGeometry(csg: CSG, toMatrix: Matrix4): BufferGeometry {
        //    let triCount = 0;
        //    const ps = csg.polygons;
        //    for (const p of ps) {
        //      triCount += p.vertices.length - 2;
        //    }
        //    const geom = new BufferGeometry();

        //    const vertices = new NBuf3(triCount * 3 * 3);
        //    const normals = new NBuf3(triCount * 3 * 3);
        //    const uvs = new NBuf2(triCount * 2 * 3);
        //    let colors: NBuf3;
        //    const grps = [];
        //    for (const p of ps) {
        //      const pvs = p.vertices;
        //      const pvlen = pvs.length;
        //      if (p.shared !== undefined) {
        //        if (!grps[p.shared]) grps[p.shared] = [];
        //      }
        //      if (pvlen && pvs[0].color !== undefined) {
        //        if (!colors) colors = new NBuf3(triCount * 3 * 3);
        //      }
        //      for (let j = 3; j <= pvlen; j++) {
        //        p.shared !== undefined &&
        //          grps[p.shared].push(
        //            vertices.top / 3,
        //            vertices.top / 3 + 1,
        //            vertices.top / 3 + 2
        //          );
        //        vertices.write(pvs[0].pos);
        //        vertices.write(pvs[j - 2].pos);
        //        vertices.write(pvs[j - 1].pos);
        //        normals.write(pvs[0].normal);
        //        normals.write(pvs[j - 2].normal);
        //        normals.write(pvs[j - 1].normal);
        //        uvs.write(pvs[0].uv);
        //        uvs.write(pvs[j - 2].uv);
        //        uvs.write(pvs[j - 1].uv);
        //        if (colors) {
        //          colors.write(pvs[0].color);
        //          colors.write(pvs[j - 2].color);
        //          colors.write(pvs[j - 1].color);
        //        }
        //      }
        //    }
        //    geom.setAttribute('position', new BufferAttribute(vertices.array, 3));
        //    geom.setAttribute('normal', new BufferAttribute(normals.array, 3));
        //    geom.setAttribute('uv', new BufferAttribute(uvs.array, 2));
        //    colors && geom.setAttribute('color', new BufferAttribute(colors.array, 3));
        //    if (grps.length) {
        //      let index = [];
        //      let gbase = 0;
        //      for (let gi = 0; gi < grps.length; gi++) {
        //        geom.addGroup(gbase, grps[gi].length, gi);
        //        gbase += grps[gi].length;
        //        index = index.concat(grps[gi]);
        //      }
        //      geom.setIndex(index);
        //    }
        //    const inv = new Matrix4().copy(toMatrix).invert();
        //    geom.applyMatrix4(inv);
        //    geom.computeBoundingSphere();
        //    geom.computeBoundingBox();

        //    return geom;
        //  }

        //  static fromMesh(mesh: Mesh, objectIndex?: any): CSG {
        //    const csg = CSG.fromGeometry(mesh.geometry, objectIndex);
        //    const ttvv0 = new Vector3();
        //    const tmpm3 = new Matrix3();
        //    tmpm3.getNormalMatrix(mesh.matrix);
        //    for (let i = 0; i < csg.polygons.length; i++) {
        //      const p = csg.polygons[i];
        //      for (let j = 0; j < p.vertices.length; j++) {
        //        const v = p.vertices[j];
        //        v.pos.copy(ttvv0.copy(v.pos.toVector3()).applyMatrix4(mesh.matrix));
        //        v.normal.copy(ttvv0.copy(v.normal.toVector3()).applyMatrix3(tmpm3));
        //      }
        //    }
        //    return csg;
        //  }


        public static BufferGeometry toGeometry(CSG csg, Matrix4 toMatrix)
        {
            var triCount = 0;
            var ps = csg.polygons;

            foreach (var p in ps)
            {
                triCount += p.vertices.Count - 2;
            }

            var geom = new BufferGeometry();

            var vertices = new NBuf3(triCount * 3 * 3);
            var normals = new NBuf3(triCount * 3 * 3);
            var uvs = new NBuf2(triCount * 2 * 3);
            NBuf3 colors = null;
            //Review with Leo
            var grps = new Dictionary<int, List<uint>>();
            foreach (var p in ps)
            {
                var pvs = p.vertices;
                var pvlen = pvs.Count;
                if (p.shared.HasValue)
                {
                    //TODO: Review with Leo
                    if (!grps.ContainsKey(p.shared.Value)) grps[p.shared.Value] = new List<uint>();
                }
                //Review with Leo
                if (pvlen > 0 && pvs[0].color != null)
                {
                    if (colors == null) colors = new NBuf3(triCount * 3 * 3);
                }
                for (var j = 3; j <= pvlen; j++)
                {
                    //Review with Leo
                    if (p.shared.HasValue)
                    {
                        //Review with Leo
                        if (!grps.ContainsKey(p.shared.Value))
                        {
                            grps[p.shared.Value] = new List<uint>();
                        }
                        grps[p.shared.Value].Add((uint)(vertices.top / 3));
                        grps[p.shared.Value].Add((uint)(vertices.top / 3 + 1));
                        grps[p.shared.Value].Add((uint)(vertices.top / 3 + 2));
                    }
                    vertices.write(pvs[0].pos);
                    vertices.write(pvs[j - 2].pos);
                    vertices.write(pvs[j - 1].pos);
                    normals.write(pvs[0].normal);
                    normals.write(pvs[j - 2].normal);
                    normals.write(pvs[j - 1].normal);
                    uvs.write(pvs[0].uv);
                    uvs.write(pvs[j - 2].uv);
                    uvs.write(pvs[j - 1].uv);

                    if (colors != null)
                    {
                        colors.write(pvs[0].color);
                        colors.write(pvs[j - 2].color);
                        colors.write(pvs[j - 1].color);
                    }
                }
            }
            //Review with Leo
            geom.SetAttribute("position", new BufferAttribute<float>(vertices.array, 3));
            //Review with Leo
            geom.SetAttribute("normal", new BufferAttribute<float>(normals.array, 3));
            //Review with Leo
            geom.SetAttribute("uv", new BufferAttribute<float>(uvs.array, 2));
            if (colors != null)
            {
                //Review with Leo
                geom.SetAttribute("color", new BufferAttribute<float>(colors.array, 3));
            }
            //Review with Leo
            if (grps != null && grps.Count > 0)
            {
                for (int gi = 0; gi < grps.Count; gi++)
                {
                    if (!grps.ContainsKey(gi))
                    {
                        grps[gi] = new List<uint>();
                    }
                }
                //Review with Leo
                var index = new List<uint>();
                var gbase = 0;
                
                for (var gi = 0; gi < grps.Count; gi++)
                {
                    geom.AddGroup(gbase, grps[gi].Count, gi);
                    gbase += grps[gi].Count;
                    //Review with Leo
                    index.AddRange(grps[gi]);

                }
                //TODO: @@
                var bufferIndex = new BufferAttribute<uint>(index.ToArray(), 1);
                geom.SetIndex(bufferIndex);
            }
            var inv = new Matrix4().Copy(toMatrix).Invert();
            geom.ApplyMatrix4(inv);
            geom.ComputeBoundingSphere();
            geom.ComputeBoundingBox();

            return geom;
        }

        //        static fromMesh(mesh: Mesh, objectIndex?: any) : CSG
        //{
        //    const csg = CSG.fromGeometry(mesh.geometry, objectIndex);
        //        const ttvv0 = new Vector3();
        //        const tmpm3 = new Matrix3();
        //        tmpm3.getNormalMatrix(mesh.matrix);
        //    for (let i = 0; i<csg.polygons.length; i++)
        //    {
        //        const p = csg.polygons[i];
        //        for (let j = 0; j<p.vertices.length; j++)
        //        {
        //            const v = p.vertices[j];
        //        v.pos.copy(ttvv0.copy(v.pos.toVector3()).applyMatrix4(mesh.matrix));
        //            v.normal.copy(ttvv0.copy(v.normal.toVector3()).applyMatrix3(tmpm3));
        //        }
        //}
        //return csg;
        //}



        //Review with Leo
        public static CSG fromMesh(Mesh mesh, int? objectIndex = null)
        {
            //TODO: Review with Leo
            var buffer = (BufferGeometry)mesh.Geometry;
            var csg = CSG.fromGeometry(buffer, objectIndex);
            var ttvv0 = new Vector3();
            var tmpm3 = new Matrix3();
            tmpm3.GetNormalMatrix(mesh.Matrix);
            for (var i = 0; i < csg.polygons.Count; i++)
            {
                var p = csg.polygons[i];
                for (var j = 0; j < p.vertices.Count; j++)
                {
                    var v = p.vertices[j];
                    v.pos.copy(ttvv0.Copy(v.pos.toVector3()).ApplyMatrix4(mesh.Matrix));
                    v.normal.copy(ttvv0.Copy(v.normal.toVector3()).ApplyMatrix3(tmpm3));
                }
            }
            return csg;
        }

        //  static toMesh(
        //    csg: CSG,
        //    toMatrix: Matrix4,
        //    toMaterial?: Material | Material[]
        //  ): Mesh {
        //    const geom = CSG.toGeometry(csg, toMatrix);
        //    const m = new Mesh(geom, toMaterial);
        //    m.matrix.copy(toMatrix);
        //    m.matrix.decompose(m.position, m.quaternion, m.scale);
        //    m.rotation.setFromQuaternion(m.quaternion);
        //    m.updateMatrixWorld();
        //    m.castShadow = m.receiveShadow = true;
        //    return m;
        //  }

        public static Mesh toMesh(
          CSG csg,
          Matrix4 toMatrix,
          Material[] toMaterial

        )
        {
            var geom = CSG.toGeometry(csg, toMatrix);
            var m = new Mesh(geom, toMaterial[0]);
            m.Matrix.Copy(toMatrix);
            m.Matrix.Decompose(m.Position, m.Quaternion, m.Scale);
            m.Rotation.SetFromQuaternion(m.Quaternion);
            m.UpdateMatrixWorld();
            m.CastShadow = m.ReceiveShadow = true;
            return m;
        }

        //TODO: Review with Leo
        public static Mesh toMesh(
          CSG csg,
          Matrix4 toMatrix,
          Material toMaterial)
        {
            return toMesh(csg, toMatrix, new Material[] { toMaterial });
        }

        //  static union(meshA: Mesh, meshB: Mesh): Mesh {
        //    const csgA = CSG.fromMesh(meshA);
        //    const csgB = CSG.fromMesh(meshB);
        //    return CSG.toMesh(csgA.union(csgB), meshA.matrix, meshA.material);
        //  }

        public static Mesh union(Mesh meshA, Mesh meshB)
        {
            var csgA = CSG.fromMesh(meshA);
            var csgB = CSG.fromMesh(meshB);
            return CSG.toMesh(csgA.union(csgB), meshA.Matrix, meshA.Material);
        }

        //  static subtract(meshA: Mesh, meshB: Mesh): Mesh {
        //    const csgA = CSG.fromMesh(meshA);
        //    const csgB = CSG.fromMesh(meshB);
        //    return CSG.toMesh(csgA.subtract(csgB), meshA.matrix, meshA.material);
        //  }

        public static Mesh subtract(Mesh meshA, Mesh meshB)
        {
            var csgA = CSG.fromMesh(meshA);
            var csgB = CSG.fromMesh(meshB);

            var m1 = meshA.Matrix;

            var m2 = meshA.Material;

            var s1 = csgA.subtract(csgB);

            var mesh = CSG.toMesh(s1, m1, m2);

            return mesh;

            //return CSG.toMesh(csgA.subtract(csgB), meshA.Matrix, meshA.Material);
        }

        //  static intersect(meshA: Mesh, meshB: Mesh): Mesh {
        //    const csgA = CSG.fromMesh(meshA);
        //    const csgB = CSG.fromMesh(meshB);
        //    return CSG.toMesh(csgA.intersect(csgB), meshA.matrix, meshA.material);
        //  }

        public static Mesh intersect(Mesh meshA, Mesh meshB)
        {
            var csgA = CSG.fromMesh(meshA);
            var csgB = CSG.fromMesh(meshB);
            return CSG.toMesh(csgA.intersect(csgB), meshA.Matrix, meshA.Material);
        }

        //  private polygons = new Array<Polygon>();

        private static Matrix3 _tmpm3 = new Matrix3();
        private static bool doRemove = false;
        private static string currentOp = string.Empty;
        private static dynamic currentPrim = null;
        private static CSG nextPrim = null;
        private static Mesh sourceMesh = null;
        private IList<Polygon> polygons = null;

        public CSG()
        {
            this.polygons = new List<Polygon>();
        }


        //  clone(): CSG {
        //    const csg = new CSG();
        //    csg.polygons = this.polygons.map((p) => {
        //      return p.clone();
        //    });
        //    return csg;
        //  }

        public CSG clone()
        {
            var csg = new CSG();
            csg.polygons = new List<Polygon>();
            for (int i = 0; i < this.polygons.Count; i++)
            {
                var p = this.polygons[i].clone();
                csg.polygons.Add(p);
            }
            return csg;
        }


        //  toPolygons(): Polygon[] {
        //    return this.polygons;
        //  }

        //  union(csg: CSG): CSG {
        //    const a = new Node(this.clone().polygons);
        //    const b = new Node(csg.clone().polygons);
        //    a.clipTo(b);
        //    b.clipTo(a);
        //    b.invert();
        //    b.clipTo(a);
        //    b.invert();
        //    a.build(b.allPolygons());
        //    return CSG.fromPolygons(a.allPolygons());
        //  }

        public CSG union(CSG csg)
        {
            var a = new Node(this.clone().polygons);
            var b = new Node(csg.clone().polygons);

            a.ClipTo(b);
            b.ClipTo(a);
            b.invert();
            b.ClipTo(a);
            b.invert();
            a.build(b.allPolygons());

            return CSG.fromPolygons(a.allPolygons());

        }

        //  subtract(csg: CSG): CSG {
        //    const a = new Node(this.clone().polygons);
        //    const b = new Node(csg.clone().polygons);
        //    a.invert();
        //    a.clipTo(b);
        //    b.clipTo(a);
        //    b.invert();
        //    b.clipTo(a);
        //    b.invert();
        //    a.build(b.allPolygons());
        //    a.invert();
        //    return CSG.fromPolygons(a.allPolygons());
        //  }

        public CSG subtract(CSG csg)
        {
            var _a = this.clone().polygons;
            var a = new Node(_a);

            var _b = csg.clone().polygons;
            var b = new Node(_b);

            a.invert();
            a.ClipTo(b);
            b.ClipTo(a);
            b.invert();
            b.ClipTo(a);
            b.invert();
            a.build(b.allPolygons());
            a.invert();
            return CSG.fromPolygons(a.allPolygons());
        }

        //  intersect(csg: CSG): CSG {
        //    const a = new Node(this.clone().polygons);
        //    const b = new Node(csg.clone().polygons);
        //    a.invert();
        //    b.clipTo(a);
        //    b.invert();
        //    a.clipTo(b);
        //    b.clipTo(a);
        //    a.build(b.allPolygons());
        //    a.invert();
        //    return CSG.fromPolygons(a.allPolygons());
        //  }

        public CSG intersect(CSG csg)
        {
            var a = new Node(this.clone().polygons);
            var b = new Node(csg.clone().polygons);
            a.invert();
            b.ClipTo(a);
            b.invert();
            a.ClipTo(b);
            b.ClipTo(a);
            a.build(b.allPolygons());
            a.invert();
            return CSG.fromPolygons(a.allPolygons());
        }

        //  // Return a new CSG solid with solid and empty space switched. This solid is
        //  // not modified.
        //  inverse(): CSG {
        //    const csg = this.clone();
        //    for (const p of csg.polygons) {
        //      p.flip();
        //    }
        //    return csg;
        //  }

        public CSG Inverse()
        {
            var csg = this.clone();
            for (int i = 0; i < csg.polygons.Count; i++)
            {
                csg.polygons[i].flip();
            }
            return csg;
        }

        //  toMesh(toMatrix: Matrix4, toMaterial?: Material | Material[]): Mesh {
        //    return CSG.toMesh(this, toMatrix, toMaterial);
        //  }

        //  toGeometry(toMatrix: Matrix4): BufferGeometry {
        //    return CSG.toGeometry(this, toMatrix);
        //  }
        //}


    }
}
