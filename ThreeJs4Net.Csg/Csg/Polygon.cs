using System.Collections.Generic;
using System.Linq;

namespace ThreeJs4Net.Csg
{
    public class Polygon
    {
        //import { Plane } from './Plane';
        //import { Vertex } from './Vertex';

        ///**
        // * Represents a convex polygon. The vertices used to initialize a polygon must
        // * be coplanar and form a convex loop. They do not have to be `Vertex`
        // * instances but they must behave similarly (duck typing can be used for
        // * customization).
        // *
        // * Each convex polygon has a `shared` property, which is shared between all
        // * polygons that are clones of each other or were split from the same polygon.
        // * This can be used to define per-polygon properties (such as surface color).
        // */
        //export class Polygon {
        //  plane: Plane;

        //  constructor(public vertices: Vertex[], public shared: any) {
        //    this.plane = Plane.fromPoints(
        //      vertices[0].pos,
        //      vertices[1].pos,
        //      vertices[2].pos
        //    );
        //  }

        public Plane plane { get; set; }
        public IList<Vertex> vertices { get; set; }
        public int? shared;

        //TODO: Review with Leo
        public Polygon(IList<Vertex> vertices, int? shared)
        {
            this.vertices = vertices;
            this.shared = shared;
            this.plane = Plane.fromPoints(
              vertices[0].pos,
              vertices[1].pos,
              vertices[2].pos
            );
        }

        //  clone(): Polygon {
        //    return new Polygon(
        //      this.vertices.map((v) => v.clone()),
        //      this.shared
        //    );
        //  }

        public Polygon clone()
        {
            for (int i = 0; i < this.vertices.Count; i++)
            {
                this.vertices[i].clone();
            }
            return new Polygon(this.vertices, this.shared);
        }

        //  flip(): void {
        //    this.vertices.reverse().map((v) => v.flip());
        //    this.plane.flip();
        //  }
        //}

        public void flip()
        {
            var _v = this.vertices.Reverse().ToList();

            for (int i = 0; i < _v.Count; i++)
            {
                _v[i].flip();
            }
            this.plane.flip();
        }
    }

}
