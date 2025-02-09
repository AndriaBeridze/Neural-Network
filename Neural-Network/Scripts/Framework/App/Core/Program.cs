using Deepforge.Utility;
using Raylib_cs;

namespace Deepforge.App;

class Program {
    static void Main(string[] args) {
        Raylib.SetTraceLogLevel(TraceLogLevel.None);
        Raylib.InitWindow(Settings.ScreenWidth, Settings.ScreenHeight, "Neural Network by Andria Beridze");
        Raylib.SetTargetFPS(120);

        App app = new App();

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Theme.BackgroundColor);

            app.Update();
            app.Render();
            
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
