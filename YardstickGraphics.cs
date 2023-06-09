﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    internal class YardstickGraphics : IDrawable
    {
        public const float CanvasWidth = 600;

        public const float CanvasHeight = 400;

        public const float YardstickHeight = CanvasHeight - (YardstickTextSize * 1.5f) - YardstickThickness;

        public const float YardStickWidth = CanvasWidth - (HorizontalIndentation * 2);

        public const float YardstickTextSize = 20;

        public const float YardstickThickness = 5f;

        public const float HorizontalIndentation = CanvasWidth * 0.02f;

        public const float VerticalIndentation = CanvasHeight * 0.02f;

        public static Color YardstickColor = Color.FromRgb(255, 0, 0);

        public static Color SubdivisionColor = Color.FromRgb(0, 0, 255);

        public static Color NoteToPlayColor = Color.FromRgb(128, 0, 128);

        public static float Top = VerticalIndentation + YardstickTextSize;

        public static float Bottom = Top + YardstickHeight + YardstickThickness;

        public static readonly Microsoft.Maui.Graphics.Font YardstickFont = Microsoft.Maui.Graphics.Font.DefaultBold;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
#if BETA
            canvas.StrokeSize = 10F;
            //canvas.StrokeLineJoin = LineJoin.Round;
            canvas.StrokeLineCap = LineCap.Square;
            canvas.StrokeColor = YardstickColor;
            canvas.DrawLine(100, 100, 200, 100);
            canvas.DrawLine(100, 100, 100, 200);
#endif

            float beatmMarkHeight = YardstickHeight;
            float beatWidth = YardStickWidth / Configuration.BeatCount;

            canvas.StrokeLineCap = LineCap.Square;
            canvas.StrokeSize = YardstickThickness;
            canvas.FontColor = YardstickColor;
            canvas.FontSize = YardstickTextSize;
            canvas.Font = YardstickFont;

            for (int beatNumber = 1; beatNumber <= Configuration.BeatCount; beatNumber++)
            {
                float beatX = beatWidth * (beatNumber - 1) + HorizontalIndentation;

                DrawBeatMark(canvas, beatNumber.ToString(), beatX);

                if (beatNumber == Configuration.BeatCount)
                {
                    canvas.DrawLine(HorizontalIndentation, Bottom, HorizontalIndentation + YardStickWidth, Bottom);
                    DrawBeatMark(canvas, "(1)", beatX + beatWidth);
                }

                canvas.StrokeColor = SubdivisionColor;

                for (int subdivision = 1; subdivision <= Configuration.SubdivisionCount; subdivision++)
                {
                    int scale = (int)Math.Pow(2, subdivision);

                    for (int subdivisionMarkNumber = 1; subdivisionMarkNumber <= scale - 1; subdivisionMarkNumber++)
                    {
                        float x = beatX + subdivisionMarkNumber * (beatWidth / scale);
                        float y = Bottom - YardstickThickness;

                        canvas.DrawLine(x, y, x, y - beatmMarkHeight / scale);
                    }
                }
            }
        }


        private static void DrawBeatMark(ICanvas canvas, string text, float beatX)
        {
            var textSize = canvas.GetStringSize(text, YardstickFont, YardstickTextSize);
            canvas.StrokeColor = YardstickColor;
            canvas.DrawString(text, beatX - (textSize.Width / 2) - (YardstickThickness / 2), YardstickTextSize, HorizontalAlignment.Left);
            canvas.DrawLine(beatX, Top, beatX, Bottom);
        }

    }

    public class RhythmGraphics : IDrawable
    {
        public const float RhythmThickness = 2.5f;

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
            float beatWidth = YardstickGraphics.YardStickWidth / Configuration.BeatCount;
            float beatX = (NoteToPlay.Item1 - 1) * beatWidth + YardstickGraphics.HorizontalIndentation;
            int scale = (int)Math.Pow(2, Configuration.SubdivisionCount);
            float x = beatX + NoteToPlay.Item2 * (beatWidth / scale);

            float arrowHeight = 80;
            float arrowTipSize = arrowHeight / 3.5f;
            float y1 = RhythmThickness * 2;
            float y2 = y1 + arrowHeight;
            canvas.StrokeColor = YardstickGraphics.NoteToPlayColor;
            canvas.StrokeSize = RhythmThickness;
            canvas.StrokeLineCap = LineCap.Round;
            canvas.StrokeLineJoin = LineJoin.Round;
            canvas.DrawLine(x, y1, x, y2);


            PathF path = new PathF(x - (arrowTipSize / 2), arrowTipSize + RhythmThickness);

            path.LineTo(x, RhythmThickness);
            path.LineTo(x + (arrowTipSize / 2), arrowTipSize + RhythmThickness);
            canvas.DrawPath(path);
        }
    }

    public class BeatIndexGraphics : IDrawable
    {
        public int BeatNumber { get; }

        public BeatIndexGraphics(int beatNumber)
        {
            BeatNumber = beatNumber;
        }

        public BeatIndexGraphics()
        {
            BeatNumber = 1;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float beatWidth = YardstickGraphics.YardStickWidth / Configuration.BeatCount;
            float beatX = (BeatNumber - 1) * beatWidth + YardstickGraphics.HorizontalIndentation;
            float radius = RhythmGraphics.RhythmThickness * 3;

            canvas.StrokeColor = YardstickGraphics.NoteToPlayColor;
            canvas.StrokeSize = RhythmGraphics.RhythmThickness;

            canvas.DrawCircle(beatX, radius * 2, radius);
        }
    }
}