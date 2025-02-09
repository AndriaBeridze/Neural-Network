using Deepforge.Utility;
using Deepforge.API;
using Raylib_cs;

namespace Deepforge.App;

class ScoreBoard {
    private int width;
    private int height;

    private Network model;

    private static int fontSize = 70;
    private Font font = Raylib.LoadFontEx("Neural-Network/Resources/Nunito-Medium.ttf", fontSize, null, 250);

    private int[] index = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private double[] score = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
    private string[] names = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };

    public ScoreBoard(int width, int height) {
        this.width = width;
        this.height = height;

        model = new Network("Neural-Network/Models/mnist.txt");
    }

    public void Update(double[] values) {
        Console.Clear();
        Vector prediction = model.Predict(new Vector(28 * 28, values));

        double sum = 0;
        for (int i = 0; i < 10; i++) {
            sum += prediction[i];
        }

        var indexedPrediction = prediction.ToArray().Select((value, index) => new { Value = value, Index = index })
                                                    .OrderByDescending(item => item.Value)
                                                    .ToList();

        for (int i = 0; i < 10; i++) {
            index[i] = indexedPrediction[i].Index;
            score[i] = indexedPrediction[i].Value / sum;
        }
    }

    public void Render() {
        int startX = Settings.ScreenWidth / 2 + Settings.CenterOffset;
        int startY = Settings.ScreenHeight / 2 - height / 2;
        Rectangle desk = new Rectangle(startX, startY, width, height);

        Raylib.DrawRectangleRec(desk, Theme.ScoreBoardColor);

        int offset = 20;
        int textX = startX + offset;
        int textY = startY + offset;
        for (int i = 0; i < 10; i++) {
            Color color = new Color(180, 180, 180, 255);
            if (i == 0) {
                color = Color.White;
            }

            Raylib.DrawTextEx(font, $"{names[index[i]]}", new System.Numerics.Vector2(textX, textY), fontSize, 1, color);
            

            int size = (int) Raylib.MeasureTextEx(font, $"{score[i] * 100:00.00}%", fontSize, 1).X;
            int perTextX = textX - offset * 2 - size + width;

            Raylib.DrawTextEx(font, $"{score[i] * 100:00.00}%", new System.Numerics.Vector2(perTextX, textY), fontSize, 1, color);
            
            textY += offset + fontSize * 10 / 15;
        }
    }
}