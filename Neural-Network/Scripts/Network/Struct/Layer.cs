namespace Deepforge.Struct;

class Layer {
    private int nodesIn;
    private int nodesOut;

    private Matrix weights;
    private Vector biases;

    private string activation;

    private Vector lastInput;

    public Layer(int nodesIn, int nodesOut, string activation = "sigmoid") {
        this.nodesIn = nodesIn;
        this.nodesOut = nodesOut;
        this.activation = activation;

        weights = new Matrix(nodesOut, nodesIn);
        biases = new Vector(nodesOut);

        lastInput = new Vector(nodesIn);
    }

    public int NodesIn => nodesIn;
    public int NodesOut => nodesOut;

    public Matrix Weights => weights;
    public Vector Biases => biases;

    public Vector LastInput => lastInput;

    public Vector Forward(Vector input) {
        lastInput = input;
        return Activate(weights * input + biases);
    }

    private Vector Activate(Vector input) {
        Vector res = new Vector(input.Size);
        for (int i = 0; i < input.Size; i++) {
            res[i] = Activate(input[i]);
        }

        return res;
    }

    private double Activate(double x) {
        switch (activation) {
            case "sigmoid":
                return 1 / (1 + Math.Exp(-x));
            case "tanh":
                return Math.Tanh(x);
            case "relu":
                return Math.Max(0, x);
            default:
                throw new Exception("Activation function not recognized.");
        }
    }

    public Vector Derivative(Vector input) {
        Vector res = new Vector(input.Size);
        for (int i = 0; i < input.Size; i++) {
            res[i] = Derivative(input[i]);
        }

        return res;
    }

    private double Derivative(double x) {
        switch (activation) {
            case "sigmoid":
                return x * (1 - x);
            case "tanh":
                return 1 - x * x;
            case "relu":
                return x > 0 ? 1 : 0;
            default:
                throw new Exception("Activation function not recognized.");
        }
    }

    public void ApplyGradient(Matrix gradientW, Vector gradientB, double learningRate) {
        weights -= gradientW * learningRate;
        biases -= gradientB * learningRate;
    }
}