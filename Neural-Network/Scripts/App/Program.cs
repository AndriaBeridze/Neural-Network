namespace Network.App;

using Deepforge;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program {
    static void Main() {
        Network network = new Network(4, [3], 1);


        string filePath = "Neural-Network/Resources/TestData/banknote-authentication.csv";
        List<float[]> data = new List<float[]>();

        List<float[]> evidence = new List<float[]>();
        List<float[]> labels = new List<float[]>();

        try {
            var lines = File.ReadAllLines(filePath);
            bool firstLine = true;
            foreach (var line in lines) {
                if (firstLine) {
                    firstLine = false;
                    continue;
                }
                var values = line.Split(',').Select(float.Parse).ToArray();
                evidence.Add(values.Take(values.Length - 1).ToArray());
                labels.Add([values.Last()]);
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
        

        // Split data into training and testing sets
        // Shuffle the data
        var rng = new Random();
        var combined = evidence.Zip(labels, (e, l) => new { Evidence = e, Label = l }).OrderBy(x => rng.Next()).ToList();
        evidence = combined.Select(x => x.Evidence).ToList();
        labels = combined.Select(x => x.Label).ToList();

        int trainSize = (int)(evidence.Count * 0.5);
        int testSize = evidence.Count - trainSize;

        var xTrain = evidence.Take(trainSize).ToArray();
        var xTest = evidence.Skip(trainSize).Take(testSize).ToArray();
        var yTrain = labels.Take(trainSize).ToArray();
        var yTest = labels.Skip(trainSize).Take(testSize).ToArray();

        network.Train(xTrain, yTrain);
        network.Test(xTest, yTest);
    }
}
