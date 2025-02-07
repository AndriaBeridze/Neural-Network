using System.Security.Policy;
using Deepforge.Struct;
using Deepforge.Utility;

namespace Deepforge.App;

class Program {
    static void Main() {
        string filePath = @"Neural-Network/Resources/Data/banknote-authentication.csv";

        Network model = new Network("Neural-Network/Resources/Models/bank-auth.txt");

        // Load the data
        Vector[] evidence = [];
        Vector[] labels = [];
        (evidence, labels) = DataParser.ParseData(filePath, [4]);

        Vector[] trainX = [], trainY = [], testX = [], testY = [];
        (_, _, testX, testY) = DataParser.TrainTestSplit(evidence, labels, trainSize : 0.8);

        model.Test(testX, testY);
    }
}
