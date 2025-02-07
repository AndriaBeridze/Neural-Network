namespace Deepforge.Struct;

/*
    A class representing a mathematical vector.
    Neural networks use lots of linear algebra, and vector class helps automate the process.
*/
class Vector {
    private int size;
    private double[] values = [];

    Random rnd = new Random();

    public Vector(int size, double[]? init = null) {
        this.size = size;
        if (init != null) {
            // Pre-defined initialization vector
            if (size != init.Length) {
                // Wrong size initialization vector
                throw new Exception($"The initialization vector must be {size} in size.");
            }
            values = init;
        } else {
            // Random initialization vector
            values = new double[size];
            for (int i = 0; i < size; i++) {
                values[i] = rnd.NextDouble() * 2 - 1;
            }
        }
    }
    
    public int Size => size;

    // Indexer for the vector
    // Allows for easy access to the vector values
    public double this[int index] {
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

    // Addition
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

    // Subtraction
    public static Vector operator -(Vector a, Vector b) {
        return a + b * -1;
    }

    // Dot product
    public static double operator *(Vector a, Vector b) {
        if (a.Size != b.Size) {
            throw new Exception($"Vectors must have the same length when adding.");
        }

        double res = 0;
        for (int i = 0; i < a.Size; i++) {
            res += a[i] * b[i];
        }

        return res;
    }

    // Scalar multiplication
    public static Vector operator *(Vector a, double b) {
        Vector res = new Vector(a.Size);
        for (int i = 0; i < a.Size; i++) {
            res[i] = a[i] * b;
        }

        return res;
    }

    public static Vector operator *(double a, Vector b) {
        return b * a;
    }

    // Scalar division
    public static Vector operator /(Vector a, double b) {
        return a * (1 / b);
    }

    public override string ToString() {
        string res = "";
        for (int i = 0; i < size; i++) {
            res += values[i] + (i == size - 1 ? "" : " ");
        }

        return res;
    }
}