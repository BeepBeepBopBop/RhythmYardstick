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

        public const float YardstickHeight = CanvasHeight - (VerticalIndentation * 2);

        public const float YardStickWidth = CanvasWidth - (HorizontalIndentation * 2);

        public const float HorizontalIndentation = CanvasWidth * 0.02f;

        public const float VerticalIndentation = CanvasHeight * 0.02f;

        public static Color YardstickColor = Color.FromRgb(255, 0, 0);

        public static Color SubdivisionColor = Color.FromRgb(0, 0, 255);

        public static Color NoteToPlayColor = Color.FromRgb(128, 0, 128);

        public static float YardstickThickness = 6f;

        public static float Top = VerticalIndentation;

        public static float Bottom = Top + YardstickHeight;


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float beatmMarkHeight = YardstickHeight;
            float beatWidth = YardStickWidth / Configuration.BeatCount;

            canvas.StrokeSize = YardstickThickness;
            canvas.FontColor = YardstickColor;
            canvas.FontSize = 20;
            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;

            for (int beatNumber = 1; beatNumber <= Configuration.BeatCount; beatNumber++)
            {
                float beatX = beatWidth * (beatNumber - 1) + HorizontalIndentation;

                canvas.StrokeColor = YardstickColor;
                canvas.DrawLine(beatX, Top, beatX, Bottom);
                canvas.DrawString(beatNumber.ToString(), beatX - YardstickThickness, Top - (Top * VerticalIndentation / 2), HorizontalAlignment.Left);

                if (beatNumber == Configuration.BeatCount)
                {
                    canvas.DrawLine(beatX + beatWidth, Top, beatX + beatWidth, Bottom);
                    canvas.DrawLine(HorizontalIndentation, Bottom, YardStickWidth, Bottom);
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
            float beatWidth = YardstickGraphics.YardStickWidth / Configuration.RoundCount;
            float beatX = (NoteToPlay.Item1 - 1) * beatWidth + YardstickGraphics.HorizontalIndentation;
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