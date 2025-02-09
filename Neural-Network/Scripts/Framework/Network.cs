using Deepforge.API;
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

    public Network(string path) {
        using (StreamReader reader = new StreamReader(path)) {
            string? line = reader.ReadLine();
            if (line == null) throw new InvalidDataException("The file is missing the layer count.");
            int layerCount = int.Parse(line);

            line = reader.ReadLine();
            if (line == null) throw new InvalidDataException("The file is missing the layer sizes.");
            string[] values = line.Split(" ");
            int[] sizes = new int[values.Length];
            for (int i = 0; i < values.Length; i++) {
                sizes[i] = int.Parse(values[i]);
            }

            for (int i = 0; i < layerCount; i++) {
                int nodesIn = sizes[i];
                int nodesOut = sizes[i + 1];
                Matrix weights = new Matrix(nodesOut, nodesIn);
                Vector biases = new Vector(nodesOut);
                string? activation = null;

                for (int j = 0; j < nodesOut; j++) {
                    string? lineW = reader.ReadLine();
                    if (lineW == null) throw new InvalidDataException("The file is missing the weights.");
                    string[] valuesW = lineW.Split(" ");
                    for (int k = 0; k < valuesW.Length; k++) {
                        weights[j][k] = double.Parse(valuesW[k]);
                    }
                }

                string? lineB = reader.ReadLine();
                if (lineB == null) throw new InvalidDataException("The file is missing the biases.");
                string[] valuesB = lineB.Split(" ");
                for (int j = 0; j < nodesOut; j++) {
                    biases[j] = double.Parse(valuesB[j]);
                }

                activation = reader.ReadLine();
                if (activation == null) throw new InvalidDataException("The file is missing the activation function.");

                layers.Add(new Layer(nodesIn, nodesOut, weights, biases, activation));
            }
        }
    }

    public Vector Forward(Vector input) {
        Vector res = input;
        foreach (Layer layer in layers) {
            res = layer.Forward(res);
        }

        return res;
    }

    public void Learn(Vector input, Vector target, double learningRate) {
        Vector output = Forward(input); 

        Matrix[] gradientW = new Matrix[layers.Count];
        Vector[] gradientB = new Vector[layers.Count];

        Vector outputError = output - target;
        Vector derivative = layers[^1].Derivative(output);
        Vector delta = new(target.Size);

        // delta[output_layer]
        for (int i = 0; i < target.Size; i++) {
            delta[i] = outputError[i] * derivative[i];
        }

        for (int layerIndex = layers.Count - 1; layerIndex >= 0; layerIndex--) {
            Layer layer = layers[layerIndex];

            gradientW[layerIndex] = new Matrix(layer.NodesOut, layer.NodesIn);
            gradientB[layerIndex] = new Vector(layer.NodesOut);

            for (int i = 0; i < layer.NodesOut; i++) {
                gradientB[layerIndex][i] = delta[i]; // Bias gradient, dC/db = dz/db * dC/dz = dz/db * delta = delta
                for (int j = 0; j < layer.NodesIn; j++) {
                    gradientW[layerIndex][i][j] = delta[i] * layer.LastInput[j]; // Weight gradient, dC/dw = dz/dw * dC/dz = dz/dw * delta = input * delta
                }
            }

            // Update delta
            outputError = ~layer.Weights * delta;
            derivative = layer.Derivative(layer.LastInput);

            delta = new Vector(outputError.Size);
            for (int i = 0; i < outputError.Size; i++) {
                delta[i] = outputError[i] * derivative[i];
            }
        }

        //  Apply the gradient
        for (int layerIndex = 0; layerIndex < layers.Count; layerIndex++) {
            layers[layerIndex].ApplyGradient(gradientW[layerIndex], gradientB[layerIndex], learningRate);
        }
    }

    // Train the network
    public void Train(Vector[] inputs, Vector[] targets, double learningRate = 0.2f, int epochs = 100) {
        Console.WriteLine("Training...");
        Console.WriteLine("Cost: 0.0000");
        Console.WriteLine("Accuracy: 0.00%");
        for (int epoch = 0; epoch < epochs; epoch++) {
            for (int i = 0; i < inputs.Length; i++) {
                Learn(inputs[i], targets[i], learningRate);
            }

            double cost = CostParser.Cost(targets, inputs.Select(Predict).ToArray());
            
            Console.SetCursorPosition(6, Console.CursorTop - 2);
            Console.WriteLine($"{cost:F5}");
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine($"{Accuracy(inputs, targets)*100:F2}%");
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
            double cost = CostParser.Cost(error);

            if (cost < 0.05f) {
                // Correct with high probability
                correct++;
            }   
        }

        return (double) correct / inputs.Length;
    }

    public Vector Predict(Vector input) {
        return Forward(input);
    }

    public void Save(string path) {
        using (StreamWriter writer = new StreamWriter(path)) {
            writer.WriteLine(layers.Count);
            int prev = layers[0].NodesIn;
            foreach (Layer layer in layers) {
                writer.Write(prev + " ");
                prev = layer.NodesOut;
            }
            writer.WriteLine(prev);

            foreach (Layer layer in layers) {
                writer.WriteLine(layer.Weights);
                writer.WriteLine(layer.Biases);
                writer.WriteLine(layer.Activation);
            }
        }
    }
}