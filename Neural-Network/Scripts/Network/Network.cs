namespace Deepforge;

using Deepforge;

class Network {
    List<Layer> layers;

    public Network(int inputSize, int[] hiddenSizes, int outputSize) {
        layers = new List<Layer>();

        int prevSize = inputSize;
        foreach (int size in hiddenSizes) {
            layers.Add(new Layer(prevSize, size, activation : "relu"));
            prevSize = size;
        }
        layers.Add(new Layer(prevSize, outputSize));
    }

    public float[] FeedForward(float[] input) {
        float[] output = input;

        foreach (Layer layer in layers) {
            output = layer.FeedForward(output);
        }

        return output;
    }

    public float CalculateCost(float[] output, float[] target) {
        float cost = 0;
        for (int i = 0; i < output.Length; i++) {
            cost += (output[i] - target[i]) * (output[i] - target[i]);
        }

        return cost;
    }

    public void Learn(float[] input, float[] target, float learningRate) {
        float[] output = FeedForward(input);
        float cost = CalculateCost(output, target);

        List<float[,]> gradientsW = new List<float[,]>();
        List<float[]> gradientsB = new List<float[]>();

        for (int i = layers.Count - 1; i >= 0; i--) {
            float[,] gradientW = new float[layers[i].OutputSize, layers[i].InputSize];
            float[] gradientB = new float[layers[i].OutputSize];
            for (int j = 0; j < layers[i].OutputSize; j++) {
            for (int k = 0; k < layers[i].InputSize; k++) {
                float h = 0.0001f;
                layers[i].AdjustWeight(j, k, h);

                float newCost = CalculateCost(FeedForward(input), target);
                float gradient = (newCost - cost) / h;
                gradientW[j, k] = gradient;

                layers[i].AdjustWeight(j, k, -h);

                layers[i].AdjustBias(j, h);

                newCost = CalculateCost(FeedForward(input), target);
                gradient = (newCost - cost) / h;
                gradientB[j] = gradient;

                layers[i].AdjustBias(j, -h);
            }
            }

            gradientsW.Add(gradientW);
            gradientsB.Add(gradientB);
        }
        
        for (int i = layers.Count - 1; i >= 0; i--) {
            for (int j = 0; j < layers[i].OutputSize; j++) {
            for (int k = 0; k < layers[i].InputSize; k++) {
                layers[i].AdjustWeight(j, k, -learningRate * gradientsW[layers.Count - 1 - i][j, k]);
            }
            }
        }

        for (int i = layers.Count - 1; i >= 0; i--) {
            for (int j = 0; j < layers[i].OutputSize; j++) {
            layers[i].AdjustBias(j, -learningRate * gradientsB[layers.Count - 1 - i][j]);
            }
        }
    }

    public void Train(float[][] inputs, float[][] targets, int epochs = 10, float learningRate = 0.3f) {
        for (int epoch = 0; epoch < epochs; epoch++) {
            Console.WriteLine($"Epoch {epoch + 1}/{epochs}");
            for (int i = 0; i < inputs.Length; i++) {
            Learn(inputs[i], targets[i], learningRate);
            }

            // Calculate training accuracy
            int correctCount = 0;
            for (int i = 0; i < inputs.Length; i++) {
            float[] output = FeedForward(inputs[i]);
            if (output[0] > 0.5f && targets[i][0] == 1) {
                correctCount++;
            } else if (output[0] <= 0.5f && targets[i][0] == 0) {
                correctCount++;
            }
            }
            float accuracy = (float)correctCount / inputs.Length * 100;
            Console.WriteLine($"Training Accuracy after epoch {epoch + 1}: {accuracy}%");
        }
        Console.WriteLine("Training complete");
    }

    public void Test(float[][] inputs, float[][] targets) {
        int correctCount = 0;
        for (int i = 0; i < inputs.Length; i++) {
            float[] output = FeedForward(inputs[i]);

            for (int j = 0; j < inputs[i].Length; j++) {
                Console.Write($"{inputs[i][j],8:F4} ");
            }
            Console.Write("->");
            for (int j = 0; j < targets[i].Length; j++) {
                Console.Write($"{targets[i][j],8:F4} ");
            }
            Console.Write(" | ");
            for (int j = 0; j < output.Length; j++) {
                Console.Write($"{output[j],8:F4} ");
            }
            Console.WriteLine();

            if (output[0] > 0.5f && targets[i][0] == 1) {
                correctCount++;
            } else if (output[0] <= 0.5f && targets[i][0] == 0) {
                correctCount++;
            }
        }
        float accuracy = (float)correctCount / inputs.Length * 100;
        Console.WriteLine($"Accuracy: {accuracy}%");
    }
}