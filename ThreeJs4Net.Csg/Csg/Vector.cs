using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net.Math;

//import { Vector3 } from 'three';


namespace ThreeJs4Net.Csg
{
    public class Vector
    {

        //**
        // * Represents a 3D vector.
        // */
        //export class Vector {
        //  constructor(public x = 0, public y = 0, public z = 0) {}

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Vector(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //  copy(v: Vector | Vector3): Vector {
        //    this.x = v.x;
        //    this.y = v.y;
        //    this.z = v.z;
        //    return this;
        //  }

        public Vector copy(Vector v)
        {
            var _v = v; //.clone();
            this.x = _v.x;
            this.y = _v.y;
            this.z = _v.z;
            return this;
        }

        public Vector copy(Vector3 v)
        {
            var _v = v;
            this.x = _v.X;
            this.y = _v.Y;
            this.z = _v.Z;
            return this;
        }

        //  clone(): Vector {
        //    return new Vector(this.x, this.y, this.z);
        //  }

        public Vector clone()
        {
            return new Vector(this.x, this.y, this.z);
        }

        //  negate(): Vector {
        //    this.x *= -1;
        //    this.y *= -1;
        //    this.z *= -1;
        //    return this;
        //  }

        public Vector negate()
        {
            this.x *= -1;
            this.y *= -1;
            this.z *= -1;
            return this;
        }

        //  add(a: Vector): Vector {
        //    this.x += a.x;
        //    this.y += a.y;
        //    this.z += a.z;
        //    return this;
        //  }

        public Vector add(Vector a)
        {
            this.x += a.x;
            this.y += a.y;
            this.z += a.z;
            return this;
        }

        //  sub(a: Vector): Vector {
        //    this.x -= a.x;
        //    this.y -= a.y;
        //    this.z -= a.z;
        //    return this;
        //  }

        public Vector sub(Vector a)
        {
            var _a = a; //.clone();
            this.x -= _a.x;
            this.y -= _a.y;
            this.z -= _a.z;
            return this;
        }


        //  times(a: number): Vector {
        //    this.x *= a;
        //    this.y *= a;
        //    this.z *= a;
        //    return this;
        //  }

        public Vector times(float a)
        {
            this.x *= a;
            this.y *= a;
            this.z *= a;
            return this;
        }

        //  dividedBy(a: number): Vector {
        //    this.x /= a;
        //    this.y /= a;
        //    this.z /= a;
        //    return this;
        //  }

        public Vector dividedby(float a)
        {
            this.x /= a;
            this.y /= a;
            this.z /= a;
            return this;
        }

        //  lerp(a: Vector, t: number): Vector {
        //    return this.add(new Vector().copy(a).sub(this).times(t));
        //  }

        public Vector lerp(Vector a, float t)
        {
            return this.add(new Vector().copy(a).sub(this).times(t));
        }

        //  unit(): Vector {
        //    return this.dividedBy(this.length());
        //  }

        public Vector unit()
        {
            return this.dividedby(this.length());
        }

        //  length(): number {
        //    return Math.sqrt(this.x ** 2 + this.y ** 2 + this.z ** 2);
        //  }

        public float length()
        {
            //TODO: ** operator?
            //Review with Leo
            //Debug.WriteLine($"this.x { this.x}");
            //Debug.WriteLine($"this.x * 2f, { this.x * .2f}");
            //Debug.WriteLine($"this.x * 2f * 2f, { (this.x * .2f) * .2f}");

            //return (float)System.Math.Sqrt(((this.x * .2f) * .2f) + ((this.y * .2f) * .2f) + ((this.z * .2f) * .2f));
            var l = (float)System.Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            //Debug.WriteLine($"length: {l}");
            return l;
        }

        //  normalize(): Vector {
        //    return this.unit();
        //  }

        public Vector normalize()
        {
            return this.unit();
        }

        //  cross(b: Vector): Vector {
        //    const a = this.clone();
        //    const ax = a.x,
        //      ay = a.y,
        //      az = a.z;
        //    const bx = b.x,
        //      by = b.y,
        //      bz = b.z;

        //    this.x = ay * bz - az * by;
        //    this.y = az * bx - ax * bz;
        //    this.z = ax * by - ay * bx;

        //    return this;
        //  }

        public Vector cross(Vector b)
        {
            var _b = b; //.clone();
            var a = this.clone();
            var ax = a.x;
            var ay = a.y;
            var az = a.z;

            var bx = _b.x;
            var bz = _b.z;
            var by = _b.y;

            this.x = ay * bz - az * by;
            this.y = az * bx - ax * bz;
            this.z = ax * by - ay * bx;

            return this;
        }

        //  dot(b: Vector): number {
        //    return this.x * b.x + this.y * b.y + this.z * b.z;
        //  }

        public float dot(Vector b)
        {
            return this.x * b.x + this.y * b.y + this.z * b.z;
        }

        //  toVector3(): Vector3 {
        //    return new Vector3(this.x, this.y, this.z);
        //  }
        //}

        public Vector3 toVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }
    }


}
