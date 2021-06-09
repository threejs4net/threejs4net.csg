using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//import { Vector } from './Vector';

namespace ThreeJs4Net.Csg
{
    public class Vertex
    {

        //**
        // * Represents a vertex of a polygon. Use your own vertex class instead of this
        // * one to provide additional features like texture coordinates and vertex
        // * colors. Custom vertex classes need to provide a `pos` property and `clone()`,
        // * `flip()`, and `interpolate()` methods that behave analogous to the ones
        // * defined by `CSG.Vertex`. This class provides `normal` so convenience
        // * functions like `CSG.sphere()` can return a smooth vertex normal, but `normal`
        // * is not used anywhere else.
        // */
        //export class Vertex {
        //  pos: Vector;
        //  normal: Vector;
        //  uv: Vector;
        //  color: Vector;

        public Vector pos { get; set; }
        public Vector normal { get; set; }
        public Vector uv { get; set; }
        public Vector color { get; set; }

        //  constructor(pos: Vector, normal: Vector, uv: Vector, color: Vector) {
        //    this.pos = new Vector().copy(pos);
        //    this.normal = new Vector().copy(normal);
        //    this.uv = new Vector().copy(uv);
        //    this.uv.z = 0;
        //    color && (this.color = new Vector().copy(color));
        //  }

        public Vertex(Vector pos, Vector normal, Vector uv, Vector color)
        {
            this.pos = new Vector().copy(pos);
            this.normal = new Vector().copy(normal);
            this.uv = new Vector().copy(uv);
            this.uv.z = 0;
            //TODO: Review with Leo
            if (color != null)
            {
                this.color = new Vector().copy(color);
            };
        }

        //  clone(): Vertex {
        //    return new Vertex(this.pos, this.normal, this.uv, this.color);
        //  }

        public Vertex clone()
        {
            return new Vertex(this.pos, this.normal, this.uv, this.color);
        }

        //  // Invert all orientation-specific data (e.g. vertex normal). Called when the
        //  // orientation of a polygon is flipped.
        //  flip(): void {
        //    this.normal.negate();
        //  }

        // Invert all orientation-specific data (e.g. vertex normal). Called when the
        // orientation of a polygon is flipped.
        public void flip()
        {
            this.normal.negate();
        }

        //  // Create a new vertex between this vertex and `other` by linearly
        //  // interpolating all properties using a parameter of `t`. Subclasses should
        //  // override this to interpolate additional properties.
        //  interpolate(other: Vertex, t: number): Vertex {
        //    return new Vertex(
        //      this.pos.clone().lerp(other.pos, t),
        //      this.normal.clone().lerp(other.normal, t),
        //      this.uv.clone().lerp(other.uv, t),
        //      this.color && other.color && this.color.clone().lerp(other.color, t)
        //    );
        //  }
        //}

        // Create a new vertex between this vertex and `other` by linearly
        // interpolating all properties using a parameter of `t`. Subclasses should
        // override this to interpolate additional properties.
        public Vertex interpolate(Vertex other, float t)
        {
            //TODO: Review with Leo
            if (this.color != null && other.color != null)
            {
                this.color.clone().lerp(other.color, t);
            }

            return new Vertex(
                this.pos.clone().lerp(other.pos, t),
                this.normal.clone().lerp(other.normal, t),
                this.uv.clone().lerp(other.uv, t),
                this.color
            );
        }
    }

}
