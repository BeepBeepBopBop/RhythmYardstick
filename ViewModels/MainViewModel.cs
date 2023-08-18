using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmYardstick
{
    public partial class MainViewModel : ObservableObject
    {
        private IDrawable _beatIndexDrawable;

        private bool _beatIndexVisible;

        private IDrawable _noteToPlayDrawable;

        private bool _noteToPlayVisible;

        public IDrawable BeatIndexDrawable
        {
            get
            {
                return _beatIndexDrawable;
            }
            set
            {
                _beatIndexDrawable = value;
                OnPropertyChanged();
            }
        }

        public bool BeatIndexVisible
        {
            get => _beatIndexVisible;
            set
            {
                _beatIndexVisible = value;
                OnPropertyChanged();
            }
        }

        public IDrawable NoteToPlayDrawable
        {
            get
            {
                return _noteToPlayDrawable;
            }
            set
            {
                _noteToPlayDrawable = value;
                OnPropertyChanged();
            }
        }

        public bool NoteToPlayVisible
        {
            get => _noteToPlayVisible;
            set
            {
                _noteToPlayVisible = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
        }
    }
}
