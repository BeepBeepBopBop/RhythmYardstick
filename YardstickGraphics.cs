﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    internal class YardstickGraphics : IDrawable
    {
        public const float YardstickTextSize = 40;

        public const float YardstickThickness = 3;

        public static Color YardstickColor = Color.FromRgb(255, 0, 0);

        public static Color SubdivisionColor = Color.FromRgb(0, 0, 255);

        public static Color NoteToPlayColor = Color.FromRgb(128, 0, 128);

        public static readonly Microsoft.Maui.Graphics.Font YardstickFont = Microsoft.Maui.Graphics.Font.DefaultBold;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            ComputeYardstickPositionAndDimensions(canvas, dirtyRect, out float left, out float top, out float bottom,
                out float yardstickWidth, out float yardstickHeight, out float beatWidth);

            canvas.StrokeLineCap = LineCap.Square;
            canvas.StrokeSize = YardstickThickness;
            canvas.FontColor = YardstickColor;
            canvas.FontSize = YardstickTextSize;
            canvas.Font = YardstickFont;

            for (int beatNumber = 1; beatNumber <= Configuration.BeatCount; beatNumber++)
            {
                float beatX = beatWidth * (beatNumber - 1) + left;

                DrawBeatMark(canvas, beatNumber.ToString(), beatX, top, bottom, yardstickWidth);

                if (beatNumber == Configuration.BeatCount)
                {
                    canvas.DrawLine(left, bottom, left + yardstickWidth, bottom);
                    DrawBeatMark(canvas, "(1)", beatX + beatWidth, top, bottom, yardstickWidth);
                }

                canvas.StrokeColor = SubdivisionColor;

                for (int subdivision = 1; subdivision <= Configuration.SubdivisionCount; subdivision++)
                {
                    int scale = (int)Math.Pow(2, subdivision);

                    for (int subdivisionMarkNumber = 1; subdivisionMarkNumber <= scale - 1; subdivisionMarkNumber++)
                    {
                        float x = beatX + subdivisionMarkNumber * (beatWidth / scale);
                        float y = bottom - YardstickThickness;

                        canvas.DrawLine(x, y, x, y - yardstickHeight / scale);
                    }
                }
            }
        }

        public static void ComputeYardstickPositionAndDimensions(ICanvas canvas, RectF dirtyRect, out float left, out float top, out float bottom,
                    out float yardstickWidth, out float yardstickHeight, out float beatWidth)
        {
            float thickestDrawingElement = Math.Max(BeatIndexGraphics.BeatIndexThickness * 3 * 2 + BeatIndexGraphics.BeatIndexThickness, YardstickThickness);
            float firstBeatTextSize = canvas.GetStringSize("1", YardstickFont, YardstickTextSize).Width;
            float lastBeatTextSize = canvas.GetStringSize("(1)", YardstickFont, YardstickTextSize).Width;
            float spacingFromTop = YardstickTextSize + thickestDrawingElement;

            float rightIndent;

            if (firstBeatTextSize > thickestDrawingElement)
            {
                left = firstBeatTextSize / 2;

            }
            else
            {
                left = thickestDrawingElement / 2;
            }
            if (lastBeatTextSize > thickestDrawingElement)
            {
                rightIndent = lastBeatTextSize / 2;
            }
            else
            {
                rightIndent = thickestDrawingElement / 2;
            }

            left = (float)Math.Ceiling(left);
            rightIndent = (float)Math.Ceiling(rightIndent);

            yardstickWidth = dirtyRect.Width - left - rightIndent;
            yardstickHeight = dirtyRect.Height - spacingFromTop - thickestDrawingElement;

            beatWidth = yardstickWidth / Configuration.BeatCount;
            top = spacingFromTop;
            bottom = dirtyRect.Height - thickestDrawingElement / 2;
        }

        private static void DrawBeatMark(ICanvas canvas, string text, float beatX, float top, float bottom, float yardstickWidth)
        {
            var largestStringSize = canvas.GetStringSize("(1)", YardstickFont, YardstickTextSize);

            canvas.StrokeColor = YardstickColor;
            canvas.DrawString(text, beatX, largestStringSize.Height, HorizontalAlignment.Center);
            canvas.DrawLine(beatX, top, beatX, bottom);
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
            YardstickGraphics.ComputeYardstickPositionAndDimensions(canvas, dirtyRect, out float left, out float top, out float bottom,
     out float yardstickWidth, out float yardstickHeight, out float beatWidth);

            float beatX = (NoteToPlay.Item1 - 1) * beatWidth + left;
            int scale = (int)Math.Pow(2, Configuration.SubdivisionCount);
            float x = beatX + NoteToPlay.Item2 * (beatWidth / scale);

            float arrowHeight = dirtyRect.Height;
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
        public const float BeatIndexThickness = 3;

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
            YardstickGraphics.ComputeYardstickPositionAndDimensions(canvas, dirtyRect, out float left, out float top, out float bottom,
                 out float yardstickWidth, out float yardstickHeight, out float beatWidth);
            float beatX = left + (BeatNumber - 1) * beatWidth;
            float radius = BeatIndexThickness * 3;
            float offset = 0;

            canvas.StrokeColor = YardstickGraphics.NoteToPlayColor;
            canvas.StrokeSize = BeatIndexThickness;

            canvas.DrawCircle(beatX + offset, radius * 2, radius);
        }
    }
}