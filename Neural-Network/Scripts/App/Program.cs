using Deepforge;
using Deepforge.Struct;

namespace Deepforge.App;

class Program {
    static void Main() {

        string filePath = @"Neural-Network/Resources/Data/banknote-authentication.csv";
        var lines = File.ReadAllLines(filePath).Skip(1);

        List<Vector> evidence = new List<Vector>();
        List<Vector> labels = new List<Vector>();
        foreach (var line in lines) {
            var values = line.Split(',');
            Vector input = new Vector(4, [
                                            float.Parse(values[0]), 
                                            float.Parse(values[1]), 
                                            float.Parse(values[2]), 
                                            float.Parse(values[3])
                                         ]);
            Vector output = new Vector(1, [float.Parse(values[4])]);

            evidence.Add(input);
            labels.Add(output);
        }

        // Shuffle evidence and labels
        var rng = new Random();
        var combined = evidence.Zip(labels, (e, l) => new { Evidence = e, Label = l }).OrderBy(x => rng.Next()).ToList();
        evidence = combined.Select(x => x.Evidence).ToList();
        labels = combined.Select(x => x.Label).ToList();

        // Split into train and test data
        int trainSize = (int)(evidence.Count * 0.2);
        var trainX = evidence.Take(trainSize).ToArray();
        var testX = evidence.Skip(trainSize).ToArray();
        var trainY = labels.Take(trainSize).ToArray();
        var testY = labels.Skip(trainSize).ToArray();

        // Create a neural network
        Network model = new Network(4, [], 1);

        model.Train(trainX, trainY);
        model.Test(testX, testY);
    }
}
