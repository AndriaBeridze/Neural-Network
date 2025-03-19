namespace Deepforge.Math;

/*
    A class representing a mathematical matrix.
    Neural networks use lots of linear algebra, and matrix class helps automate the process.
*/
class Matrix {
    private int row, column;
    private Vector[] values = [];

    public Matrix(int row, int column, Vector[]? init = null) {
        this.row = row;
        this.column = column;
        if (init != null) {
            // Pre-defined initialization matrix
            if (init.Length != row) {
                throw new Exception($"The initialization matrix must be {row}x{column} in size.");
            }
            values = new Vector[row];
            for (int i = 0; i < row; i++) {
                if (init[i].Size != column) {
                    throw new Exception($"The initialization matrix must be {row}x{column} in size.");
                }
                values[i] = init[i];
            }
        } else {
            // Random initialization matrix
            values = new Vector[row];
            for (int i = 0; i < row; i++) {
                values[i] = new Vector(column);
            }
        }
    }

    public int Row => row;
    public int Column => column;

    public Vector this[int index] {
        get {
            if (index <= -1 || index >= row) {
                throw new Exception($"Index is outside of the bounds.");
            }
            return values[index];
        } set {
            if (index <= -1 || index >= row) {
                throw new Exception($"Index is outside of the bounds.");
            } else if (value.Size != column) {
                throw new Exception($"The vector must be of size {column}.");
            }
            values[index] = value;  
        }
    }

    // Addition
    public static Matrix operator +(Matrix a, Matrix b) {
        if (a.Row != b.Row || a.Column != b.Column) {
            throw new Exception("Dimensions of two matrices have to be the same when adding.");
        }

        Matrix res = new Matrix(a.Row, a.Column);
        for (int i = 0; i < a.Row; i++) {
            res[i] = a[i] + b[i];
        }

        return res;
    }

    // Subtraction
    public static Matrix operator -(Matrix a, Matrix b) {
        return a + b * -1;
    }

    // Multiplication
    public static Vector operator *(Matrix a, Vector b) {
        if (a.Column != b.Size) {
            throw new Exception("Column dimension of the matrix has to be equal to the size of the vector when multiplying.");
        }

        Vector res = new Vector(a.Row);
        for (int i = 0; i < a.Row; i++) {
            res[i] = a[i] * b;
        }

        return res;
    } 

    // Scalar multiplication
    public static Matrix operator *(Matrix a, double b) {
        Matrix res = new Matrix(a.Row, a.Column);
        for (int i = 0; i < a.Row; i++) {
            res[i] = a[i] * b;
        }

        return res;
    } 

    public static Matrix operator *(double a, Matrix b) {
        return b * a;
    }

    // Scalar division
    public static Matrix operator /(Matrix a, double b) {
        return a * (1 / b);
    }

    // Transpose
    public static Matrix operator ~(Matrix a) {
        Matrix res = new Matrix(a.Column, a.Row);
        for (int i = 0; i < a.Row; i++) {
            for (int j = 0; j < a.Column; j++) {
                res[j][i] = a[i][j];
            }
        }

        return res;
    }

    public override string ToString() {
        string res = "";
        for (int i = 0; i < row; i++) {
            res += values[i] + (i == row - 1 ? "" : "\n");
        }

        return res;
    }
}