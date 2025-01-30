namespace Network.App;

using Deepforge.Struct;

class Program {
    static void Main() {
        Layer layer = new Layer(2, 3);
        Vector input = new Vector(2, new float[] { 1, 2 });
        Vector output = layer.Forward(input);

        Console.WriteLine(layer);
        Console.WriteLine(input);
        Console.WriteLine(output);
    }
}
