using Deepforge.API;

namespace Deepforge.Utility;

class DataParser {
    // Parse the data from a CSV file
    public static (Vector[], Vector[]) ParseData(string path, int[] labelIndices, Dictionary<string, int>? labelMap = null) {
        var lines = File.ReadAllLines(path).Skip(1);

        List<Vector> evidences = new List<Vector>();
        List<Vector> labels = new List<Vector>();

        foreach (var line in lines) {
            var values = line.Split(',');
            Vector evidence = new Vector(values.Length - labelIndices.Length);
            Vector label = new Vector(labelIndices.Length);

            int labelIndex = 0;
            int evidenceIndex = 0;
            for (int i = 0; i < values.Length; i++) {
                if (!labelIndices.Contains(i)) {
                    try {
                        evidence[evidenceIndex] = double.Parse(values[i]);
                        evidenceIndex++;
                    } catch {
                        if (labelMap != null) evidence[evidenceIndex++] = labelMap[values[i]];
                    }
                } else {
                    try {
                        label[labelIndex] = double.Parse(values[i]);
                        labelIndex++;
                    } catch {
                        if (labelMap != null) label[labelIndex++] = labelMap[values[i]];
                    }
                }
            }

            evidences.Add(evidence);
            labels.Add(label);
        }
        
        return (evidences.ToArray(), labels.ToArray());
    }

    // Linearize the data, so it's in the range [0, 1]
    public static void Linearize(ref Vector[] data, int minValue, int maxValue) {
        for (int i = 0; i < data.Length; i++) {
            data[i] = Linearize(data[i], minValue, maxValue);
        }
    }

    public static Vector Linearize(Vector vector, int minValue, int maxValue) {
        Vector res = new Vector(vector.Size);
        for (int i = 0; i < vector.Size; i++) {
            res[i] = (vector[i] - minValue) / (maxValue - minValue);
        }

        return res;
    }

    // Standardize the data
    // It's a common practice to standardize the data before feeding it to the neural network
    // Formula: (x - mean) / (standard deviation)
    public static void Standardize(ref Vector[] data) {
        int vectorSize = data[0].Size;
        Vector mean = new Vector(vectorSize);
        Vector std = new Vector(vectorSize);

        // Calculate mean
        for (int i = 0; i < vectorSize; i++) {
            double sum = 0;
            for (int j = 0; j < data.Length; j++) {
                sum += data[j][i];
            }
            mean[i] = sum / data.Length;
        }

        // Calculate standard deviation
        for (int i = 0; i < vectorSize; i++) {
            double sum = 0;
            for (int j = 0; j < data.Length; j++) {
                sum += Math.Pow(data[j][i] - mean[i], 2);
            }
            std[i] = Math.Sqrt(sum / data.Length);
        }

        // Standardize data
        for (int i = 0; i < data.Length; i++) {
            for (int j = 0; j < vectorSize; j++) {
                data[i][j] = (data[i][j] - mean[j]) / std[j];
            }
        }
    }

    // Split the data into training and testing sets
    public static (Vector[], Vector[], Vector[], Vector[]) TrainTestSplit(Vector[] evidence, Vector[] labels, double trainSize = 0.5) {
        int trainLength = (int) (evidence.Length * trainSize);
        int testLength = evidence.Length - trainLength;

        Vector[] trainX = new Vector[trainLength];
        Vector[] trainY = new Vector[trainLength];
        Vector[] testX = new Vector[testLength];
        Vector[] testY = new Vector[testLength];

        Random rnd = new Random();
        List<int> indices = Enumerable.Range(0, evidence.Length).ToList();
        for (int i = 0; i < trainLength; i++) {
            int index = rnd.Next(indices.Count);
            trainX[i] = evidence[indices[index]];
            trainY[i] = labels[indices[index]];
            indices.RemoveAt(index);
        }

        for (int i = 0; i < testLength; i++) {
            int index = rnd.Next(indices.Count);
            testX[i] = evidence[indices[index]];
            testY[i] = labels[indices[index]];
            indices.RemoveAt(index);
        }

        return (trainX, trainY, testX, testY);
    }

    // One-hot encode the labels
    // When the label is 9 and we have 10 classes, the one-hot encoding will be [0, 0, 0, 0, 0, 0, 0, 0, 0, 1]
    public static void OneHotEncode(ref Vector[] labels, int classes) {
        for (int i = 0; i < labels.Length; i++) {
            labels[i] = OneHotEncode(labels[i], classes);
        }
    }

    public static Vector OneHotEncode(Vector label, int classes) {
        Vector res = new Vector(classes);
        for (int i = 0; i < classes; i++) {
            res[i] = label[0] == i ? 1 : 0;
        }

        return res;
    }
}