using Microsoft.Maui.Platform;
using Plugin.Maui.Audio;
using System.Runtime.CompilerServices;

namespace RhythmYardstick;

public partial class MainPage : ContentPage
{
    private Timer _timer;

    private int _elapsedRounds;

    private int _currentBeatNumber;

    private Random _randomBeat = new Random(DateTime.Now.Nanosecond);

    private Random _randomSubdivision = new Random(DateTime.Now.Millisecond);

    public Tuple<int, int> NoteToPlay { get; set; }

    public MainPage()
    {
        BindingContext = new MainViewModel();
        InitializeComponent();
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        if (_timer == null)
        {
            _currentBeatNumber = 0;
            int bpmMiliseconds = 60000 / Configuration.BPM;
            _timer = new Timer(TimerCallback, null, bpmMiliseconds, bpmMiliseconds);
            Application.Current.Dispatcher.Dispatch(StartNewExercise);
        }
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        if (_timer != null)
        {
            Application.Current.Dispatcher.Dispatch(StopExercise);
        }

        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private async void TimerCallback(object state)
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
            StopExercise();
        }
        else
        {
            DisplayBeat();
        }
    }

    private void StartNewExercise()
    {
        NoteToPlay = GetNoteToPlay();
        DisplayNoteToPlay();
    }

    private void StopExercise()
    {
        ((MainViewModel)BindingContext).NoteToPlayVisible = false;
        ((MainViewModel)BindingContext).BeatIndexVisible = false;
        _timer.Dispose();
        _timer = null;
    }

    private Tuple<int, int> GetNoteToPlay()
    {
        int beatNumber;
        int subDivisionNumber;

        beatNumber = _randomBeat.Next(1, Configuration.BeatCount + 1);
        subDivisionNumber = _randomSubdivision.Next(1, Math.Min(1, Configuration.SubdivisionCount * Configuration.SubdivisionCount - 1));

        return new Tuple<int, int>(beatNumber, subDivisionNumber);
    }

    private void DisplayBeat()
    {
        MainViewModel viewModel = (MainViewModel)BindingContext;
        viewModel.BeatIndexDrawable = new BeatIndexGraphics(_currentBeatNumber);
        viewModel.BeatIndexVisible |= true;
    }

    private void DisplayNoteToPlay()
    {
        MainViewModel viewModel = (MainViewModel)BindingContext;
        viewModel.NoteToPlayDrawable = new RhythmGraphics(NoteToPlay);
        viewModel.NoteToPlayVisible = true;
    }
}