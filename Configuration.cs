using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    internal static class Configuration
    {
        private static int DefaultSubdivisionCount = 2;

        private static int DefaultBeatCount = 4;

        private static int DefaultBPM = 60;

        private static int DefaultRoundCount = 2;

        private static int DefaultExerciseCount = 2;

        public static Tuple<int, int> NoteToPlay;

        public static int BPM
        {
            get
            {
                return Preferences.Get("BPM", DefaultBPM);
            }
            set
            {
                Preferences.Set("BPM", value);
            }
        }

        public static int SubdivisionCount
        {
            get
            {
                return Preferences.Get("SubdivisionCount", DefaultSubdivisionCount);
            }
            set
            {
                Preferences.Set("SubdivisionCount", value);
            }
        }

        public static int BeatCount
        {
            get
            {
                return Preferences.Get("BeatCount", DefaultBeatCount);
            }
            set
            {
                Preferences.Set("BeatCount", value);
            }
        }

        public static int RoundCount
        {
            get
            {
                return Preferences.Get("RoundCount", DefaultRoundCount);
            }
            set
            {
                Preferences.Set("RoundCount", value);
            }
        }

        public static int ExerciseCount
        {
            get
            {
                return Preferences.Get("ExerciseCount", DefaultExerciseCount);
            }
            set
            {
                Preferences.Set("ExerciseCount", value);
            }
        }
    }
}
