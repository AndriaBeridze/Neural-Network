namespace Deepforge.Struct;

using Deepforge.Utility;

class Vector {
    private int size;
    private float[] values = [];

    Random rnd = new Random();

    public Vector(int size, float[]? init = null) {
        this.size = size;
        if (init != null) {
            if (size != init.Length) {
                new Exception($"The initialization vector must contain exactly {size} values.");
            }
            values = init;
        } else {
            values = new float[size];
            for (int i = 0; i < size; i++) {
                values[i] = (float) rnd.NextDouble() * 2 - 1;
            }
        }
    }
    
    public int Size => size;

    public float this[int index] {
        get {
            if (index <= -1 || index >= this.size) {
                new Exception($"Index {index} is outside of the bounds.");
            }

            return values[index];
        } set {
            if (index <= -1 || index >= this.size) {
                new Exception($"Index {index} is outside of the bounds.");
            }

            values[index] = value;
        }
    }

    public static Vector operator +(Vector a, Vector b) {
        if (a.Size != b.Size) {
            new Exception($"Vectors must have the same length when adding.");
        }

        float[] sum = new float[a.Size];
        for (int i = 0; i < a.Size; i++) {
            sum[i] = a[i] + b[i];
        }
        
        return new Vector(a.Size, sum);
    }

    public static float operator *(Vector a, Vector b) {
        if (a.Size != b.Size) {
            new Exception($"Vectors must have the same length when adding.");
        }

        float sum = 0;
        for (int i = 0; i < a.Size; i++) {
            sum += a[i] * b[i];
        }
        
        return sum;
    }

    public override string ToString() {
        string rep = "[";
        for (int i = 0; i < size; i++) {
            rep += $"{values[i]:F3}" + (i != size - 1 ? " " : "");
        }
        rep += "]";

        return rep;
    }
}