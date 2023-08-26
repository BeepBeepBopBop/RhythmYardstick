using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    public partial class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel()
        {
            BPM = Configuration.BPM;
            SubdivisionCount = Configuration.SubdivisionCount;
            BeatCount = Configuration.BeatCount;
        }

        public int SubdivisionCount
        {
            get => Configuration.SubdivisionCount;
            set
            {
                Configuration.SubdivisionCount = value;
                OnPropertyChanged();
            }
        }

        public int BeatCount
        {
            get => Configuration.BeatCount;
            set
            {
                Configuration.BeatCount = value;
                OnPropertyChanged();
            }
        }

        public int BPM
        {
            get => Configuration.BPM;
            set
            {
                Configuration.BPM = value;
                OnPropertyChanged();
            }
        }

        public int ExerciseCount
        {
            get => Configuration.ExerciseCount;
            set
            {
                Configuration.ExerciseCount = value;
                OnPropertyChanged();
            }
        }

        public int RoundCount
        {
            get => Configuration.RoundCount;
            set
            {
                Configuration.RoundCount = value;
                OnPropertyChanged();
            }
        }
    }
}
