namespace Deepforge.Struct;

class Matrix {
    private int row, column;
    private Vector[] values = [];

    public Matrix(int row, int column, Vector[]? init = null) {
        this.row = row;
        this.column = column;
        if (init != null) {
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
                throw new Exception($"Matrix - Index is outside of the bounds.");
            }
            return values[index];
        } set {}
    }

    public static Matrix operator +(Matrix a, Matrix b) {
        if (a.Row != b.Row || a.Column != b.Column) {
            throw new Exception("Dimensions of two matrices have to be the same when adding.");
        }

        Vector[] res = new Vector[a.Row];
        for (int i = 0; i < a.Row; i++) {
            res[i] = a[i] + b[i];
        }

        return new Matrix(a.Row, a.Column, res);
    }

    public static Matrix operator -(Matrix a, Matrix b) {
        return a + b * -1;
    }

    public static Vector operator *(Matrix a, Vector b) {
        if (a.Column != b.Size) {
            throw new Exception("Column dimension of the matrix has to be equal to the size of the vector when multiplying.");
        }

        float[] res = new float[a.Row];
        for (int i = 0; i < a.Row; i++) {
            res[i] = a[i] * b;
        }

        return new Vector(a.Row, res);
    } 

    public static Matrix operator *(Matrix a, float b) {
        Vector[] res = new Vector[a.Row];
        for (int i = 0; i < a.Row; i++) {
            res[i] = a[i] * b;
        }

        return new Matrix(a.Row, a.Column, res);
    } 

    public static Matrix operator *(float a, Matrix b) {
        return b * a;
    }

    public static Matrix operator /(Matrix a, float b) {
        return a * (1 / b);
    }

    public override string ToString() {
        string res = "";
        for (int i = 0; i < row; i++) {
            string r = values[i].ToString();
            r = r.Replace('[', '|');
            r = r.Replace(']', '|');

            res += r + (i != row - 1 ? "\n" : "");
        }

        return res;
    }
}