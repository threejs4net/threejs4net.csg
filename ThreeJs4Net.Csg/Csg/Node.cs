using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ThreeJs4Net.Csg
{
    public class Node
    {

        //import { Plane } from './Plane';
        //import { Polygon } from './Polygon';

        ///**
        // * Holds a node in a BSP tree. A BSP tree is built from a collection of polygons
        // * by picking a polygon to split along. That polygon (and all other coplanar
        // * polygons) are added directly to that node and the other polygons are added to
        // * the front and/or back subtrees. This is not a leafy BSP tree since there is
        // * no distinction between internal and leaf nodes.
        // */
        //export class Node {
        //  polygons: Polygon[];
        //  plane: Plane;
        //  front: Node;
        //  back: Node;

        private IList<Polygon> polygons;
        private Plane plane;
        private Node front;
        private Node back;


        //  constructor(polygons?: Polygon[]) {
        //    this.plane = null;
        //    this.front = null;
        //    this.back = null;
        //    this.polygons = [];
        //    if (polygons) this.build(polygons);
        //  }

        public Node(IList<Polygon> _polygons = null)
        {
            this.plane = null;
            this.front = null;
            this.back = null;
            this.polygons = new List<Polygon>();
            if (_polygons != null)
            {
                this.build(_polygons);
            }
        }

        //  clone(): Node {
        //    const node = new Node();
        //    node.plane = this.plane && this.plane.clone();
        //    node.front = this.front && this.front.clone();
        //    node.back = this.back && this.back.clone();
        //    node.polygons = this.polygons.map((p) => p.clone());
        //    return node;
        //  }

        public Node clone()
        {
            var node = new Node();
            node.plane = this.plane != null ? this.plane.clone() : null;
            node.front = this.front != null ? this.front.clone() : null;
            node.back = this.back != null ? this.back.clone() : null;

            node.polygons = new List<Polygon>();

            for (int i = 0; i < this.polygons.Count; i++)
            {
                node.polygons.Add(this.polygons[i].clone());
            }

            return node;
        }

        //  // Convert solid space to empty space and empty space to solid space.
        //  invert(): void {
        //    for (let i = 0; i < this.polygons.length; i++) this.polygons[i].flip();

        //    this.plane && this.plane.flip();
        //    this.front && this.front.invert();
        //    this.back && this.back.invert();
        //    const temp = this.front;
        //    this.front = this.back;
        //    this.back = temp;
        //  }

        public void invert()
        {
            for (var i = 0; i < this.polygons.Count; i++) this.polygons[i].flip();

            //TODO: Review with Leo
            if (this.plane != null) { this.plane.flip(); }
            //TODO: Review with Leo
            if (this.front != null) { this.front.invert(); }
            //TODO: Review with Leo
            if (this.back != null) { this.back.invert(); }

            var temp = this.front;
            this.front = this.back;
            this.back = temp;
        }

        //  // Recursively remove all polygons in `polygons` that are inside this BSP
        //  // tree.
        //  clipPolygons(polygons: Polygon[]): Polygon[] {
        //    if (!this.plane) return polygons.slice();

        //    let front = new Array<Polygon>(),
        //      back = new Array<Polygon>();

        //    for (let i = 0; i < polygons.length; i++) {
        //      this.plane.splitPolygon(polygons[i], front, back, front, back);
        //    }

        //    if (this.front) front = this.front.clipPolygons(front);
        //    this.back ? (back = this.back.clipPolygons(back)) : (back = []);

        //    return front.concat(back);
        //  }

        // Recursively remove all polygons in `polygons` that are inside this BSP
        // tree.
        public IList<Polygon> clipPolygons(IList<Polygon> polygons)
        {

            if (this.plane == null)
            {
                return polygons;
                //js: return polygons.Slice();
                //see: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/slice
            }

            IList<Polygon> _front = new List<Polygon>();
            IList<Polygon> _back = new List<Polygon>();

            for (var i = 0; i < polygons.Count; i++)
            {
                this.plane.splitPolygon(polygons[i], _front, _back, _front, _back);
            }

            if (this.front != null) { _front = this.front.clipPolygons(_front); }

            //TODO: Review with Leo
            _back = this.back != null ? this.back.clipPolygons(_back) : new List<Polygon>();
            return _front.Concat(_back).ToList();
        }



        //  // Remove all polygons in this BSP tree that are inside the other BSP tree
        //  // `bsp`.
        //  clipTo(bsp: Node): void {
        //    this.polygons = bsp.clipPolygons(this.polygons);
        //    if (this.front) this.front.clipTo(bsp);
        //    if (this.back) this.back.clipTo(bsp);
        //  }

        // Remove all polygons in this BSP tree that are inside the other BSP tree
        // `bsp`.
        public void ClipTo(Node bsp)
        {
            this.polygons = bsp.clipPolygons(this.polygons);
            //TODO: Review with Leo
            if (this.front != null)
            {
                this.front.ClipTo(bsp);
            }
            //TODO: Review with Leo
            if (this.back != null)
            {
                this.back.ClipTo(bsp);
            }
        }

        //  // Return a list of all polygons in this BSP tree.
        //  allPolygons(): Polygon[] {
        //    let polygons = this.polygons.slice();
        //    if (this.front) polygons = polygons.concat(this.front.allPolygons());
        //    if (this.back) polygons = polygons.concat(this.back.allPolygons());
        //    return polygons;
        //  }

        // Return a list of all polygons in this BSP tree.
        public IList<Polygon> allPolygons()
        {
            //var polygons = this.polygons.slice();
            var polygons = this.polygons;

            if (this.front != null)
            {
                polygons = polygons.Concat(this.front.allPolygons()).ToList();
            }
            if (this.back != null)
            {
                polygons = polygons.Concat(this.back.allPolygons()).ToList();
            }
            return polygons;
        }

        //  // Build a BSP tree out of `polygons`. When called on an existing tree, the
        //  // new polygons are filtered down to the bottom of the tree and become new
        //  // nodes there. Each set of polygons is partitioned using the first polygon
        //  // (no heuristic is used to pick a good split).
        //  build(polygons: Polygon[]): void {
        //    if (!polygons.length) return;
        //    if (!this.plane) this.plane = polygons[0].plane.clone();
        //    const front = [],
        //      back = [];
        //    for (let i = 0; i < polygons.length; i++) {
        //      this.plane.splitPolygon(
        //        polygons[i],
        //        this.polygons,
        //        this.polygons,
        //        front,
        //        back
        //      );
        //    }
        //    if (front.length) {
        //      if (!this.front) this.front = new Node();
        //      this.front.build(front);
        //    }
        //    if (back.length) {
        //      if (!this.back) this.back = new Node();
        //      this.back.build(back);
        //    }
        //  }
        //}

        // Build a BSP tree out of `polygons`. When called on an existing tree, the
        // new polygons are filtered down to the bottom of the tree and become new
        // nodes there. Each set of polygons is partitioned using the first polygon
        // (no heuristic is used to pick a good split).
        public void build(IList<Polygon> _polygons)
        {
            //Debug.WriteLine($"i. polygons.Count: {polygons.Count}");
            //TODO: Review with Leo
            if (_polygons == null)
            {
                return;
            }
            //TODO: Review with Leo
            if (_polygons.Count == 0)
            {
                return;
            }
            //TODO: Review with Leo
            if (this.plane == null)
            {
                this.plane = _polygons[0].plane.clone();
            }

            IList<Polygon> front = new List<Polygon>();
            IList<Polygon> back = new List<Polygon>();

            for (var i = 0; i < _polygons.Count; i++)
            {
                this.plane.splitPolygon(
                  _polygons[i],
                  this.polygons,
                  this.polygons,
                  front,
                  back
                );
                //Debug.WriteLine($"j. i:{i}");
            }
            if (front.Count > 0)
            {
                Debug.WriteLine($"g. front.Count: {front.Count}");
                //TODO: Review with Leo
                if (this.front == null) { this.front = new Node(); }
                this.front.build(front);
            }
            if (back.Count > 0)
            {
                Debug.WriteLine($"h. back.Count: {back.Count}");
                //TODO: Review with Leo
                if (this.back == null) { this.back = new Node(); }
                this.back.build(back);
            }
        }
    }

}

