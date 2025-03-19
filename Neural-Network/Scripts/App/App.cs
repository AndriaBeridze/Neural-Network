using Deepforge.Utility;
using Raylib_cs;

namespace Deepforge.App;

class App {
    private Rectangle desk = new Rectangle(0, 0, 0, 0);
    private Canvas canvas = new Canvas(28, 28, Settings.PixelSize);
    private ScoreBoard scoreBoard = new ScoreBoard(200, 200);

    public App() {
        canvas = new Canvas(28, 28, Settings.PixelSize);
        scoreBoard = new ScoreBoard(Settings.CanvasLength * Settings.PixelSize, Settings.CanvasLength * Settings.PixelSize);

        // Background desk for better visuals
        int deskX = Settings.ScreenWidth / 2 - Settings.CanvasLength * Settings.PixelSize - Settings.CenterOffset - Settings.DeskPadding;
        int deskY = Settings.ScreenHeight / 2 - Settings.CanvasLength * Settings.PixelSize / 2 - Settings.DeskPadding;
        int deskWidth = 2 * (Settings.CanvasLength * Settings.PixelSize + Settings.DeskPadding + Settings.CenterOffset);
        int deskHeight = Settings.CanvasLength * Settings.PixelSize + 2 * Settings.DeskPadding;
        desk = new Rectangle(deskX, deskY, deskWidth, deskHeight);
    }

    public void Update() {
        double[] values = canvas.Update(); // Update the canvas + get the values from it
        scoreBoard.Update(values); // Update the score board with the values
    }

    public void Render() {
        Raylib.DrawRectangleRec(desk, Theme.DeskColor);
        canvas.Render();
        scoreBoard.Render();
    }
}