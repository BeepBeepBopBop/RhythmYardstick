using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    internal class YardstickGraphics : IDrawable
    {
        public const float CanvasWidth = 700;

        public const float CanvasHeight = 500;

        public const float VerticalIndentation = 0.15f;

        public static Color YardstickColor = Color.FromRgb(255, 0, 0);

        public static Color SubdivisionColor = Color.FromRgb(0, 0, 255);

        public static Color NoteToPlayColor = Color.FromRgb(128, 0, 128);

        public static float YardstickThickness = 6f;

        public static float Top = CanvasHeight * VerticalIndentation;

        public static float Bottom = CanvasHeight /*- (CanvasHeight * VerticalIndentation)*/;

        public static float LeftIndentation = CanvasWidth / 50F;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float beatmMarkHeight = CanvasHeight;
            float beatWidth = CanvasWidth / Configuration.BeatCount;

            canvas.StrokeSize = YardstickThickness;
            canvas.FontColor = YardstickColor;
            canvas.FontSize = 20;
            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;

            for (int beatNumber = 1; beatNumber <= Configuration.BeatCount; beatNumber++)
            {
                float beatX = beatWidth * (beatNumber - 1) + LeftIndentation;

                canvas.StrokeColor = YardstickColor;
                canvas.DrawLine(beatX, Top, beatX, Bottom);
                canvas.DrawString(beatNumber.ToString(), beatX - YardstickThickness, Top - (Top * VerticalIndentation / 2), HorizontalAlignment.Left);

                if (beatNumber == Configuration.BeatCount)
                {
                    canvas.DrawLine(beatX + beatWidth, Top, beatX + beatWidth, Bottom);
                    canvas.DrawLine(LeftIndentation, Bottom, CanvasWidth, Bottom);
                }

                canvas.StrokeColor = SubdivisionColor;

                for (int subdivision = 1; subdivision <= Configuration.SubdivisionCount; subdivision++)
                {
                    int scale = (int)Math.Pow(2, subdivision);

                    for (int subdivisionMarkNumber = 1; subdivisionMarkNumber <= scale - 1; subdivisionMarkNumber++)
                    {
                        float x = beatX + subdivisionMarkNumber * (beatWidth / scale);
                        canvas.DrawLine(x, Bottom, x, Bottom - beatmMarkHeight / scale);
                    }
                }
            }
        }
    }

    public class RhythmGraphics : IDrawable
    {
        public Tuple<int, int> NoteToPlay { get; set; }

        public RhythmGraphics()
        {
            NoteToPlay = new Tuple<int, int>(1, 0);
        }


        public RhythmGraphics(Tuple<int, int> noteToPlay)
        {
            NoteToPlay = noteToPlay;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float beatWidth = YardstickGraphics.CanvasWidth / Configuration.RoundCount;
            float beatX = (NoteToPlay.Item1 - 1) * beatWidth;
            int scale = (int)Math.Pow(2, Configuration.SubdivisionCount);
            float x = beatX + NoteToPlay.Item2 * (beatWidth / scale);

            float y1 = 0;
            float y2 = 50;

            canvas.StrokeColor = YardstickGraphics.NoteToPlayColor;
            canvas.StrokeSize = YardstickGraphics.YardstickThickness;
            canvas.StrokeLineCap = LineCap.Square;
            canvas.DrawLine(x, y1, x, y2);

            canvas.StrokeSize = 2;
            canvas.StrokeLineCap = LineCap.Round;
            PathF path = new PathF(0, 10);

            path.LineTo(5, 0);
            path.LineTo(10, 10);
            canvas.DrawPath(path);
        }
    }
}
