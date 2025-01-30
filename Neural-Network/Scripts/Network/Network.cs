using System.Runtime.Versioning;
using Deepforge.Struct;
using Deepforge.Utility;

namespace Deepforge;

class Network {
    private List<Layer> layers = new List<Layer>();

    public Network(int input, int[] hidden, int output) {
        int prev = input;
        for (int i = 0; i < hidden.Length; i++) {
            layers.Add(new Layer(prev, hidden[i]));
            prev = hidden[i];
        }
        layers.Add(new Layer(prev, output));
    }

    public Vector Forward(Vector input) {
        Vector res = input;
        foreach (Layer layer in layers) {
            res = layer.Forward(res);
        }

        return res;
    }

    public void Learn(Vector input, Vector target, float learningRate) {
        Vector output = Forward(input);
        Vector error = target - output;
        float cost = Util.Cost(error);

        float h = 0.0001f;
        List<Matrix> weightGradients = new List<Matrix>();
        List<Vector> biasGradients = new List<Vector>();
        for (int i = layers.Count - 1; i >= 0; i--) {
            Matrix gradientW = new Matrix(layers[i].NodesOut, layers[i].NodesIn);
            Vector gradientB = new Vector(layers[i].NodesOut);

            for (int j = 0; j < layers[i].NodesOut; j++) {
                for (int k = 0; k < layers[i].NodesIn; k++) {
                    layers[i].Weights[j][k] += h;
                    float delta = Util.Cost(target - Forward(input)) - cost;
                    layers[i].Weights[j][k] -= h;

                    gradientW[j][k] = delta / h;
                }
            }

            for (int j = 0; j < layers[i].NodesOut; j++) {
                layers[i].Biases[j] += h;
                float delta = Util.Cost(target - Forward(input)) - cost;
                layers[i].Biases[j] -= h;

                gradientB[j] = delta / h;
            }

            weightGradients.Add(gradientW);
            biasGradients.Add(gradientB);
        }

        weightGradients.Reverse();
        biasGradients.Reverse();

        for (int i = 0; i < layers.Count; i++) {
            layers[i].ApplyGradient(weightGradients[i], biasGradients[i], learningRate);
        }
    }

    public void Train(Vector[] inputs, Vector[] targets, int epochs = 10, float learningRate = 0.3f) {
        Console.WriteLine("Training model...");
        for (int i = 0; i < epochs; i++) {
            Console.Write($"Epoch {i + 1:D2}/{epochs:D2}: [");
            int progressBarWidth = 50;
            for (int j = 0; j < inputs.Length; j++) {
                Learn(inputs[j], targets[j], learningRate);
                int progress = (int)((float)(j + 1) / inputs.Length * progressBarWidth);
                Console.Write(new string('#', progress) + new string('-', progressBarWidth - progress) + $"] {TestAccuracy(inputs, targets):F4}");
                Console.SetCursorPosition(14, Console.CursorTop);
            }
            Console.WriteLine();
        }

        Console.WriteLine("Training complete.");
        Console.WriteLine();
        Console.WriteLine($"Training accuracy: {TestAccuracy(inputs, targets):F4}");
    }

    public void Test(Vector[] inputs, Vector[] targets) {
        Console.WriteLine($"Test accuracy: {TestAccuracy(inputs, targets):F4}");
    }

    public float TestAccuracy(Vector[] inputs, Vector[] targets) {
        int correct = 0;
        for (int i = 0; i < inputs.Length; i++) {
            Vector output = Predict(inputs[i]);
            Vector error = targets[i] - output;
            float cost = Util.Cost(error);

            if (cost < 0.1f) {
                correct++;
            }   
        }

        return (float) correct / inputs.Length;
    }

    public Vector Predict(Vector input) {
        return Forward(input);
    }

    public override string ToString() {
        string res = "";
        foreach (Layer layer in layers) {
            res += layer.ToString() + "\n";
        }

        return res;
    }
}