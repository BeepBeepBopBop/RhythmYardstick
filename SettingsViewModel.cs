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
                var value = Preferences.Get("SubdivisionCount", Configuration.SubdivisionCount);
                return value;
            }
            set
            {
                Preferences.Set("SubdivisionCount,", value);
            }
        }

        public int BeatCount
        {
            get
            {
                var value = Preferences.Get("BeatCount", Configuration.BeatCount);
                return value;
            }
            set
            {
                Preferences.Set("BeatCount,", value);
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
                Preferences.Set("BPM,", value);
            }
        }
    }
}
