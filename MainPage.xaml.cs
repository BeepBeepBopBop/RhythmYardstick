using Microsoft.Maui.Platform;
using Plugin.Maui.Audio;
using System.Runtime.CompilerServices;

namespace RhythmYardstick;

public partial class MainPage : ContentPage
{
    private Timer _timer;

    private bool _isStarted;

    private int _elapsedRounds;

    private int _currentBeatNumber;

    private Random _randomBeat = new Random();

    private Random _randomSubdivision = new Random();

    public Tuple<int, int> NoteToPlay { get; set; }

    public MainPage()
    {
        BindingContext = new MainViewModel();
        InitializeComponent();
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        _currentBeatNumber = 0;
        ((MainViewModel)BindingContext).BeatIndexVisible = true;
        int bpmMiliseconds = 60000 / Configuration.BPM;
        _timer = new Timer(TimerCallback, null, bpmMiliseconds, bpmMiliseconds);
        _isStarted = true;
        Application.Current.Dispatcher.Dispatch(StartNewExercise);
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        Application.Current.Dispatcher.Dispatch(StopExercise);
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private async void TimerCallback(object state)
    {
        if (_isStarted)
        {
            //var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("drumsticks.mp3"));
            //audioPlayer.Play();

            _currentBeatNumber++;

            if (_currentBeatNumber > Configuration.BeatCount)
            {
                _currentBeatNumber = 1;
                _elapsedRounds++;
            }

            if (_elapsedRounds == Configuration.RoundCount)
            {
                _elapsedRounds = 0;
                Application.Current.Dispatcher.Dispatch(StopExercise);
            }

            Application.Current.Dispatcher.Dispatch(DisplayBeat);
        }
    }

    private void StartNewExercise()
    {
        NoteToPlay = GetNoteToPlay();
        ((MainViewModel)BindingContext).NoteToPlayDrawable = new RhythmGraphics(NoteToPlay);
        ((MainViewModel)BindingContext).NoteToPlayVisible = true;
    }

    private void StopExercise()
    {
        if (_isStarted)
        {
            ((MainViewModel)BindingContext).NoteToPlayVisible = false;
            ((MainViewModel)BindingContext).BeatIndexVisible = false;
            _isStarted = false;
            _timer = null;
        }
    }

    private Tuple<int, int> GetNoteToPlay()
    {
        int beatNumber;
        int subDivisionNumber;

        beatNumber = _randomBeat.Next(1, Configuration.BeatCount);
        subDivisionNumber = _randomSubdivision.Next(0, Configuration.SubdivisionCount * Configuration.SubdivisionCount - 1);

        return new Tuple<int, int>(beatNumber, subDivisionNumber);
    }

    private void DisplayBeat()
    {
        ((MainViewModel)BindingContext).BeatIndexDrawable = new BeatIndexGraphics(_currentBeatNumber);
    }

    private void DisplayNoteToPlay()
    {
        ((MainViewModel)BindingContext).NoteToPlayDrawable = new RhythmGraphics(NoteToPlay);
    }
}