using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RhythmYardstick
{
    public partial class MainViewModel : ObservableObject
    {
        private Stopwatch _stopwatch = new Stopwatch();

        private int _elapsedRounds;
        private int _elapsedExercises;
        private int _currentBeatNumber;

        [ObservableProperty]
        private string _statusLabelText;
        [ObservableProperty]
        private int _warmupBeatsRemaining;
        [ObservableProperty]
        private bool _warmupCountdownStarted;

        private Random _randomBeat = new Random(DateTime.Now.Nanosecond);
        private Random _randomSubdivision = new Random(DateTime.Now.Millisecond);

        public Tuple<int, int> NoteToPlay { get; set; }

        private Timer _timer;
        private IDrawable _beatIndexDrawable;
        private bool _beatIndexVisible;
        private IDrawable _noteToPlayDrawable;
        private bool _noteToPlayVisible;
        private bool _isStarted;

        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;
                OnPropertyChanged();
            }
        }

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

        [RelayCommand]
        public void OnStartButtonClicked()
        {
            if (_timer == null)
            {
                IsStarted = true;
#if DEBUG_ELAPSED
                _stopwatch.Start();
#endif
                _currentBeatNumber = 0;
                int bpmMiliseconds = 60000 / Configuration.BPM;
                _timer = new Timer(TimerCallback, null, bpmMiliseconds, bpmMiliseconds);
                Application.Current.Dispatcher.Dispatch(StartSession);
            }
            else
            {
                StopSession();
            }
        }

        [RelayCommand]
        public async Task OnSettingsButtonClicked()
        {
            if (_timer != null)
            {
                Application.Current.Dispatcher.Dispatch(StopSession);
            }

            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        private async void TimerCallback(object state)
        {
#if DEBUG_ELAPSED
            _stopwatch.Stop();
            StatusLabelText = _stopwatch.ElapsedMilliseconds.ToString();
            _stopwatch.Restart();
#endif

            var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("drumsticks.mp3"));
            audioPlayer.Play();

            if (WarmupBeatsRemaining > 0)
            {
                if (WarmupBeatsRemaining == Configuration.BeatCount + 1)
                {
                    WarmupCountdownStarted = true;
                }

                WarmupBeatsRemaining--;

                if (WarmupBeatsRemaining > 0)
                {
                    return;
                }
                else
                {
                    StatusLabelText = string.Empty;
                    WarmupCountdownStarted = false;
                }
            }

            _currentBeatNumber++;

            if (_currentBeatNumber > Configuration.BeatCount)
            {
                _currentBeatNumber = 1;
                _elapsedRounds++;
            }

            if (_elapsedRounds == Configuration.RoundCount)
            {
                _elapsedRounds = 0;
                _elapsedExercises++;

                if (_elapsedExercises == Configuration.ExerciseCount)
                {
                    _elapsedExercises = 0;
                    StopSession();
                    return;
                }
                else
                {
                    StartNewExercise();
                }
            }

            DisplayBeat();
        }

        private void StartSession()
        {
            WarmupBeatsRemaining = Configuration.BeatCount + 1;
            StatusLabelText = "Get ready...";
            StartNewExercise();
        }

        private void StartNewExercise()
        {
            NoteToPlay = GetNoteToPlay();
            DisplayNoteToPlay();
        }

        private void StopSession()
        {
            IsStarted = false;
            NoteToPlayVisible = false;
            BeatIndexVisible = false;
            WarmupCountdownStarted = false;
            StatusLabelText = string.Empty;
            _timer.Dispose();
            _timer = null;
        }

        private Tuple<int, int> GetNoteToPlay()
        {
            int beatNumber;
            int subDivisionNumber;

            beatNumber = _randomBeat.Next(1, Configuration.BeatCount + 1);
            subDivisionNumber = _randomSubdivision.Next(1, (int)Math.Pow(2, Configuration.SubdivisionCount));

            return new Tuple<int, int>(beatNumber, subDivisionNumber);
        }

        private void DisplayBeat()
        {
            BeatIndexDrawable = new BeatIndexGraphics(_currentBeatNumber);
            BeatIndexVisible |= true;
        }

        private void DisplayNoteToPlay()
        {
            NoteToPlayDrawable = new RhythmGraphics(NoteToPlay);
            NoteToPlayVisible = true;
        }
    }
}
