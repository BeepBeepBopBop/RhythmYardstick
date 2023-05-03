using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    internal static class Configuration
    {
        public static int SubdivisionCount = 4;

        public static int BeatCount = 4;

        public static int BPM = 60;

        public static int RoundCount = 4;

        public static Tuple<int, int> NoteToPlay;
    }
}
