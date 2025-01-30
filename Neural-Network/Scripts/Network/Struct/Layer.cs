namespace Deepforge.Struct;

class Layer {
    private int nodesIn;
    private int nodesOut;

    private Matrix weights;
    private Vector biases;

    private string activation;

    public Layer(int nodesIn, int nodesOut, string activation = "relu") {
        this.nodesIn = nodesIn;
        this.nodesOut = nodesOut;
        this.activation = activation;

        weights = new Matrix(nodesOut, nodesIn);
        biases = new Vector(nodesOut);
    }

    public int NodesIn => nodesIn;
    public int NodesOut => nodesOut;

    public Matrix Weights => weights;
    public Vector Biases => biases;

    public Vector Forward(Vector input) {
        return Activate(weights * input + biases);
    }

    public Vector Activate(Vector input) {
        switch (activation) {
            case "relu":
                return ReLU(input);
            case "sigmoid":
                return Sigmoid(input);
            case "tanh":
                return Tanh(input);
            default:
                throw new Exception("Activation function not recognized.");
        }
    }

    public Vector ReLU(Vector input) {
        float[] res = new float[input.Size];
        for (int i = 0; i < input.Size; i++) {
            res[i] = Math.Max(0, input[i]);
        }

        return new Vector(input.Size, res);
    }

    public Vector Sigmoid(Vector input) {
        float[] res = new float[input.Size];
        for (int i = 0; i < input.Size; i++) {
            res[i] = 1 / (1 + (float) Math.Exp(-input[i]));
        }

        return new Vector(input.Size, res);
    }

    public Vector Tanh(Vector input) {
        float[] res = new float[input.Size];
        for (int i = 0; i < input.Size; i++) {
            res[i] = (float) Math.Tanh(input[i]);
        }

        return new Vector(input.Size, res);
    }

    public override string ToString() {
        return $"Layer: {nodesIn} -> {nodesOut} ({activation})";
    }
}