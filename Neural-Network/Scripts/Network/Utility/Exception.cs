namespace Deepforge.Utility;

class Exception {
    public Exception(string message) {
        Console.WriteLine("Error: " + message);
        Environment.Exit(0);
    }
}