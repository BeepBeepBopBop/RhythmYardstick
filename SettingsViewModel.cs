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
        public int SubdivisionCount
        {
            get
            {
                return Preferences.Get("SubdivisionCount", Configuration.SubdivisionCount);
            }
            set
            {
                Preferences.Set("SubdivisionCount,", Configuration.SubdivisionCount);
            }
        }

        public int BeatCount
        {
            get
            {
                return Preferences.Get("BeatCount", Configuration.BeatCount);
            }
            set
            {
                Preferences.Set("BeatCount,", Configuration.BeatCount);
            }
        }

        public int BPM
        {
            get
            {
                return Preferences.Get("BPM", Configuration.BPM);
            }
            set
            {
                Preferences.Set("BPM,", Configuration.BPM);
            }
        }
    }
}
