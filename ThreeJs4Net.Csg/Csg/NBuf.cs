namespace ThreeJs4Net.Csg
{
    public class NBuf3
    {

        //import { Vector } from './Vector';

        //export class NBuf3 {
        //  top = 0;
        //  array: Float32Array;

        public int top = 0;
        public float[] array;

        //  constructor(ct: number) {
        //    this.array = new Float32Array(ct);
        //  }

        public NBuf3(int ct)
        {
            this.array = new float[ct];
        }

        //  write(v: Vector): void {
        //    this.array[this.top++] = v.x;
        //    this.array[this.top++] = v.y;
        //    this.array[this.top++] = v.z;
        //  }

        public void write(Vector v)
        {
            this.array[this.top++] = v.x;
            this.array[this.top++] = v.y;
            this.array[this.top++] = v.z;
        }

        //}


    }

    public class NBuf2
    {
        //export class NBuf2 {
        //  top = 0;
        //  array: Float32Array;

        public int top = 0;
        public float[] array;

        //  constructor(ct: number) {
        //    this.array = new Float32Array(ct);
        //  }
        public NBuf2(int ct)
        {
            this.array = new float[ct];
        }

        //  write(v: Vector): void {
        //    this.array[this.top++] = v.x;
        //    this.array[this.top++] = v.y;
        //  }

        public void write(Vector v)
        {
            this.array[this.top++] = v.x;
            this.array[this.top++] = v.y;
        }

        //}


    }
}
