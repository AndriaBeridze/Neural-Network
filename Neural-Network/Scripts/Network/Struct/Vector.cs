namespace Deepforge.Struct;

class Vector {
    private int size;
    private float[] values = [];

    Random rnd = new Random();

    public Vector(int size, float[]? init = null) {
        this.size = size;
        if (init != null) {
            if (size != init.Length) {
                throw new Exception($"The initialization vector must be {size} in size.");
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
                throw new Exception($"Index is outside of the bounds.");
            }

            return values[index];
        } set {
            if (index <= -1 || index >= this.size) {
                throw new Exception($"Index is outside of the bounds.");
            }

            values[index] = value;
        }
    }

    public static Vector operator +(Vector a, Vector b) {
        if (a.Size != b.Size) {
            throw new Exception($"Vectors must have the same length when adding.");
        }

        Vector res = new Vector(a.Size);
        for (int i = 0; i < a.Size; i++) {
            res[i] = a[i] + b[i];
        }

        return res;
    }

    public static Vector operator -(Vector a, Vector b) {
        return a + b * -1;
    }

    public static float operator *(Vector a, Vector b) {
        if (a.Size != b.Size) {
            throw new Exception($"Vectors must have the same length when adding.");
        }

        float res = 0;
        for (int i = 0; i < a.Size; i++) {
            res += a[i] * b[i];
        }

        return res;
    }

    public static Vector operator *(Vector a, float b) {
        Vector res = new Vector(a.Size);
        for (int i = 0; i < a.Size; i++) {
            res[i] = a[i] * b;
        }

        return res;
    }

    public static Vector operator *(float a, Vector b) {
        return b * a;
    }

    public static Vector operator /(Vector a, float b) {
        return a * (1 / b);
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