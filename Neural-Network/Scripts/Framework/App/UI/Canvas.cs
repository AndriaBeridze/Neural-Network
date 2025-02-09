using Deepforge.Utility;
using Raylib_cs;
using System.Numerics;

namespace Deepforge.App;

class Canvas {
    private int width;
    private int height;
    private int pixelSize;

    int[,] pixelValues = new int[0, 0];

    private double influenceRadius = 1.3;

    private static int fontSize = 50;
    private Font font = Raylib.LoadFontEx("Neural-Network/Resources/Nunito-Medium.ttf", fontSize, null, 250);

    public Canvas(int width, int height, int pixelSize) {
        this.width = width;
        this.height = height;
        this.pixelSize = pixelSize;

        pixelValues = new int[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                pixelValues[x, y] = 0;
            }
        }  
    }

    public double[] Update() {
        int startX = Settings.ScreenWidth / 2 - width * pixelSize - Settings.CenterOffset;
        int startY = Settings.ScreenHeight / 2 - height * pixelSize / 2;
        if (Raylib.IsMouseButtonDown(MouseButton.Left) || Raylib.IsMouseButtonDown(MouseButton.Right)) {
            int scale = Raylib.IsMouseButtonDown(MouseButton.Left) ? 1 : -2;
            Vector2 mousePosition = Raylib.GetMousePosition();
            double x = (mousePosition.X - startX) / pixelSize;
            double y = (mousePosition.Y - startY) / pixelSize;

            if (x >= 0 && x < width && y >= 0 && y < height) {
                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        double dist = Math.Sqrt((x - i) * (x - i) + (y - j) * (y - j));

                        double influence = 0.5 * Math.Max(0, (influenceRadius - dist) / influenceRadius);
                        pixelValues[i, j] += (int) (influence * 255) * scale;
                        pixelValues[i, j] = Math.Min(255, pixelValues[i, j]);
                        pixelValues[i, j] = Math.Max(0, pixelValues[i, j]);
                    }
                }
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    pixelValues[x, y] = 0;
                }
            }
        }

        double[] values = new double[width * height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int index = x + y * width;
                values[index] = pixelValues[x, y] / 255.0;
            }
        }
        return Process(values);
    }

    public void Render() {
        int startX = Settings.ScreenWidth / 2 - width * pixelSize - Settings.CenterOffset;
        int startY = Settings.ScreenHeight / 2 - height * pixelSize / 2;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int xPos = startX + x * pixelSize;
                int yPos = startY + y * pixelSize;
                int pixelValue = pixelValues[x, y];
                Color color = new Color(pixelValue, pixelValue, pixelValue, 255);

                Raylib.DrawRectangle(xPos, yPos, pixelSize, pixelSize, color);
            }
        }

        int instructionsX = startX;
        int instructionsY = startY + height * pixelSize + Settings.DeskPadding * 2;
        Raylib.DrawTextEx(font, "Left click to draw | Right click to erase | Space to clear", new Vector2(instructionsX, instructionsY), fontSize, 1, Color.White);
    }

    private double[] Process(double[] values) {
        double[,] pic = new double[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                pic[x, y] = values[x + y * width];
            }
        }

        int minX = width, minY = height, maxX = 0, maxY = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (pic[x, y] > 0) {
                    minX = Math.Min(minX, x);
                    minY = Math.Min(minY, y);
                    maxX = Math.Max(maxX, x);
                    maxY = Math.Max(maxY, y);
                }
            }
        }

        int deltaX = maxX - minX;
        int deltaY = maxY - minY;
        double scale = 20.0f / Math.Max(deltaX, deltaY);
        double[,] fit = new double[20, 20];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (values[x + y * width] > 0) {
                    int xIndex = (int) ((x - minX) * scale);
                    int yIndex = (int) ((y - minY) * scale);

                    if (xIndex < 0 || xIndex >= 20 || yIndex < 0 || yIndex >= 20) {
                        continue;
                    }

                    fit[xIndex, yIndex] += pic[x, y];
                    fit[xIndex, yIndex] = Math.Min(1, fit[xIndex, yIndex]);
                }
            }
        }

        // Calculate the center of mass
        double sum = 0;
        double xCenter = 0;
        double yCenter = 0;
        for (int x = 0; x < 20; x++) {
            for (int y = 0; y < 20; y++) {
                sum += fit[x, y];
                xCenter += x * fit[x, y];
                yCenter += y * fit[x, y];
            }
        }
        xCenter /= sum;
        yCenter /= sum;

        int xOffset = (int)(10 - xCenter);
        int yOffset = (int)(10 - yCenter);

        double[,] processed = new double[28, 28];
        for (int x = 0; x < 20; x++) {
            for (int y = 0; y < 20; y++) {
                int newX = x + xOffset + 4;
                int newY = y + yOffset + 4;
                if (newX >= 0 && newX < 28 && newY >= 0 && newY < 28) {
                    processed[newX, newY] = fit[x, y];
                }
            }
        }

        double[] processedValues = new double[28 * 28];
        for (int x = 0; x < 28; x++) {
            for (int y = 0; y < 28; y++) {
                processedValues[x + y * 28] = processed[x, y];
            }
        }

        return processedValues;
    }
        
}