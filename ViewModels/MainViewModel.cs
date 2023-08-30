using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using System.Diagnostics;
namespace RhythmYardstick
{
    public partial class MainViewModel : ObservableObject
    {
        private Stopwatch _stopwatch = new Stopwatch();

        private int _elapsedRounds;

        private int _elapsedExercises;

        private int _currentBeatNumber;

        private Random _randomBeat = new Random(DateTime.Now.Nanosecond);

        private Random _randomSubdivision = new Random(DateTime.Now.Millisecond);

        public Tuple<int, int> NoteToPlay { get; set; }

        private Timer _timer;
        private IDrawable _beatIndexDrawable;
        private bool _beatIndexVisible;
        private IDrawable _noteToPlayDrawable;
        private bool _noteToPlayVisible;
        private bool _isStarted;


#if DEBUG
        private string _debug;

        public string Debug
        {
            get => _debug;
            set
            {
                _debug = value;
                OnPropertyChanged();
            }
        }
#endif

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
#if DEBUG
                _stopwatch.Start();
#endif
                _currentBeatNumber = 0;
                int bpmMiliseconds = 60000 / Configuration.BPM;
                _timer = new Timer(TimerCallback, null, bpmMiliseconds, bpmMiliseconds);
                Application.Current.Dispatcher.Dispatch(StartNewExercise);
            }
            else
            {
                IsStarted = false;
                StopExercise();
            }
        }

        [RelayCommand]
        public async Task OnSettingsButtonClicked()
        {
            if (_timer != null)
            {
                Application.Current.Dispatcher.Dispatch(StopExercise);
            }

            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        private async void TimerCallback(object state)
        {
#if DEBUG
            _stopwatch.Stop();
            Debug = _stopwatch.ElapsedMilliseconds.ToString();
            _stopwatch.Restart();
#endif

            var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("drumsticks.mp3"));
            audioPlayer.Play();

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
                    StopExercise();
                    return;
                }
                else
                {
                    StartNewExercise();
                }
            }

            DisplayBeat();
        }

        private void StartNewExercise()
        {
            NoteToPlay = GetNoteToPlay();
            DisplayNoteToPlay();
        }

        private void StopExercise()
        {
            NoteToPlayVisible = false;
            BeatIndexVisible = false;
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
