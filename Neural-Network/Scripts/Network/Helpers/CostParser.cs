using Deepforge.Math;

namespace Deepforge.Helper;

class CostParser {
    // Cost function
    public static double Cost(Vector error) {
        double res = 0;
        for (int i = 0; i < error.Size; i++) {
            // Cost function is the mean squared error
            res += (float) error[i] * error[i] * 0.5;
        }

        return res;
    }

    public static double Cost(Vector target, Vector output) {
        return Cost(target - output);
    }

    // Cost function for a batch
    public static double Cost(Vector[] target, Vector[] output) {
        double res = 0;
        for (int i = 0; i < target.Length; i++) {
            res += Cost(target[i], output[i]);
        }

        return res / target.Length;
    }
}