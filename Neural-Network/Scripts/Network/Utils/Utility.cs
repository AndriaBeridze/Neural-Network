using Deepforge.Struct;

namespace Deepforge.Utility;

class Util {
    public static float Cost(Vector error) {
        float res = 0;
        for (int i = 0; i < error.Size; i++) {
            res += (float) error[i] * error[i];
        }

        return res;
    }
}