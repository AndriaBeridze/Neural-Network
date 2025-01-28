namespace Deepforge;

class Layer {
    private int inputSize, outputSize;
    private float[,] weights;
    private float[] biases;
    private string activation;

    public Layer(int inputSize, int outputSize, string activation = "sigmoid") {
        this.inputSize = inputSize;
        this.outputSize = outputSize;

        weights = new float[outputSize, inputSize];
        biases = new float[outputSize];

        // Since the whole point of neural networks is to learn the weights and biases,
        // we can initialize them using random values.
        Random rnd = new Random();
        for (int i = 0; i < outputSize; i++) {
            for (int j = 0; j < inputSize; j++) {
                weights[i, j] = (float) rnd.NextDouble() * 2 - 1;
            }
            biases[i] = (float) rnd.NextDouble() * 2 - 1;
        }

        this.activation = activation;
    }

    public int InputSize => inputSize;
    public int OutputSize => outputSize;

    public float AdjustWeight(int i, int j, float delta) {
        float oldWeight = weights[i, j];
        weights[i, j] += delta;
        return oldWeight;
    }

    public float AdjustBias(int i, float delta) {
        float oldBias = biases[i];
        biases[i] += delta;
        return oldBias;
    }

    public float[] FeedForward(float[] input) {
        float[] output = new float[weights.GetLength(0)];

        for (int i = 0; i < weights.GetLength(0); i++) {
            float sum = 0;
            for (int j = 0; j < weights.GetLength(1); j++) {
                sum += weights[i, j] * input[j];
            }
            sum += biases[i];
            output[i] = Activation(sum);
        }

        return output;
    }

    private float Activation(float x) {
        switch (activation) {
            case "sigmoid":
                return 1 / (1 + (float) Math.Exp(-x));
            case "tanh":
                return (float) Math.Tanh(x);
            case "relu":
                return Math.Max(0, x);
            case "hyperbolic tangent":
                return (float) Math.Tanh(x);
            default:
                throw new ArgumentException("Invalid activation function");
        }
    }

    private float ActivationDerivative(float x) {
        switch (activation) {
            case "sigmoid":
                return Activation(x) * (1 - Activation(x));
            case "tanh":
                return 1 - Activation(x) * Activation(x);
            case "relu":
                return x > 0 ? 1 : 0;
            case "hyperbolic tangent":
                return 1 - Activation(x) * Activation(x);
            default:
                throw new ArgumentException("Invalid activation function");
        }
    }
}