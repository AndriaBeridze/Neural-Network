namespace Deepforge.API;

class Layer {
    private int nodesIn;
    private int nodesOut;

    private Matrix weights;
    private Vector biases;

    private string activation;

    private Vector lastInput; // Need for backpropagation

    public Layer(int nodesIn, int nodesOut, string activation = "sigmoid") {
        this.nodesIn = nodesIn;
        this.nodesOut = nodesOut;
        this.activation = activation;

        weights = new Matrix(nodesOut, nodesIn);
        biases = new Vector(nodesOut);

        lastInput = new Vector(nodesIn);
    }

    public Layer(int nodesIn, int nodesOut, Matrix weights, Vector biases, string activation = "sigmoid") {
        this.nodesIn = nodesIn;
        this.nodesOut = nodesOut;
        this.activation = activation;

        this.weights = weights;
        this.biases = biases;

        lastInput = new Vector(nodesIn);
    }

    public int NodesIn => nodesIn;
    public int NodesOut => nodesOut;

    public Matrix Weights => weights;
    public Vector Biases => biases;

    public string Activation => activation;

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
        /*
            Activation functions are needed to either scale or normalize the output of a neuron.
            The activation function is applied to the weighted sum of the inputs and biases.
        */
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

    // Derivative of the activation function
    // When minimizing the cost, we need to follow the gradient of the cost function backwards
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

    // Apply the gradient to the weights and biases
    // When going the opposite direction of the gradient (derivative), we are minimizing the cost
    public void ApplyGradient(Matrix gradientW, Vector gradientB, double learningRate) {
        weights -= gradientW * learningRate;
        biases -= gradientB * learningRate;
    }
}