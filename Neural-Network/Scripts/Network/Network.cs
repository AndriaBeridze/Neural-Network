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

    public void Learn(Vector input, Vector target, double learningRate) {
        Vector output = Forward(input); // Forward pass

        Matrix[] gradientW = new Matrix[layers.Count];
        Vector[] gradientB = new Vector[layers.Count];

        Vector outputError = output - target;
        Vector derivative = layers[^1].Derivative(output);
        Vector delta = new(target.Size);

        for (int i = 0; i < target.Size; i++) {
            delta[i] = outputError[i] * derivative[i];
        }

        for (int layerIndex = layers.Count - 1; layerIndex >= 0; layerIndex--) {
            Layer layer = layers[layerIndex];

            gradientW[layerIndex] = new Matrix(layer.NodesOut, layer.NodesIn);
            gradientB[layerIndex] = new Vector(layer.NodesOut);

            for (int i = 0; i < layer.NodesOut; i++) {
                gradientB[layerIndex][i] = delta[i]; // Bias gradient
                for (int j = 0; j < layer.NodesIn; j++) {
                    gradientW[layerIndex][i][j] = delta[i] * layer.LastInput[j]; // Weight gradient
                }
            }

            outputError = ~layer.Weights * delta;
            derivative = layer.Derivative(layer.LastInput);

            delta = new Vector(outputError.Size);
            for (int i = 0; i < outputError.Size; i++) {
                delta[i] = outputError[i] * derivative[i];
            }
        }

        // Apply gradients with learning rate
        for (int layerIndex = 0; layerIndex < layers.Count; layerIndex++) {
            layers[layerIndex].ApplyGradient(gradientW[layerIndex], gradientB[layerIndex], learningRate);
        }
    }


    public void Train(Vector[] inputs, Vector[] targets, double learningRate = 0.1f, int batchSize = 100) {
        Console.WriteLine("Training...");
        Console.WriteLine("Cost: 0.0000");
        while (true) {
            for (int i = 0; i < inputs.Length; i++) {
                Learn(inputs[i], targets[i], learningRate);
            }

            double cost = Util.Cost(targets, inputs.Select(Predict).ToArray());
            
            Console.SetCursorPosition(6, Console.CursorTop - 1);
            Console.WriteLine($"{cost:F4}");

            if (cost <= 0.01) break;
        }
        Console.WriteLine("Training complete.");
        Console.WriteLine($"Training accuracy: {Accuracy(inputs, targets)*100:F2}%");
    }

    public void Test(Vector[] inputs, Vector[] targets) {
        Console.WriteLine($"Testing accuracy: {Accuracy(inputs, targets)*100:F2}%");
    }

    public double Accuracy(Vector[] inputs, Vector[] targets) {
        int correct = 0;
        for (int i = 0; i < inputs.Length; i++) {
            Vector output = Predict(inputs[i]);
            Vector error = targets[i] - output;
            double cost = Util.Cost(error);

            if (cost < 0.1f) {
                correct++;
            }   
        }

        return (double) correct / inputs.Length;
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