using System.Security.Policy;
using Deepforge.Struct;
using Deepforge.Utility;

namespace Deepforge.App;

class Program {
    static void Main() {
        string filePath = @"Neural-Network/Resources/Models/mnist.txt";

        Network model = new Network(filePath);

        Vector[] evidence, labels;
        (evidence, labels) = DataParser.ParseData("Neural-Network/Resources/Data/mnist.csv", [0]);

        DataParser.Linearize(ref evidence, 0, 255);
        DataParser.OneHotEncode(ref labels, 10);

        model.Test(evidence, labels);
    }
}
