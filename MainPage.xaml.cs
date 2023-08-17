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
        InitializeComponent();
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        _currentBeatNumber = 0;
        int bpmMiliseconds = 60000 / Configuration.BPM;
        _timer = new Timer(TimerCallback, null, bpmMiliseconds, bpmMiliseconds);
        _isStarted = true;
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
        Application.Current.Dispatcher.Dispatch(DisplayNoteToPlay);
    }

    private void StopExercise()
    {
        if (_isStarted)
        {
            RemoveGraphicElementFromView(typeof(RhythmGraphics));
            RemoveGraphicElementFromView(typeof(BeatIndexGraphics));
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

    bool lastIsDisplayBeat = true;

    private void DisplayBeat()
    {
        lastIsDisplayBeat = true;
        GraphicsView beatIndexGraphics = new GraphicsView()
        {
            Drawable = new BeatIndexGraphics(_currentBeatNumber),
            WidthRequest = 600,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Start
        };

        RemoveGraphicElementFromView(typeof(BeatIndexGraphics));
        layout.Children.Insert(1, beatIndexGraphics);
    }

    private void DisplayNoteToPlay()
    {
        lastIsDisplayBeat = false;
        GraphicsView noteToPlayGraphics = new GraphicsView
        {
            Drawable = new RhythmGraphics(NoteToPlay),
            WidthRequest = 600,
            HeightRequest = 100,
            HorizontalOptions = LayoutOptions.Start,
        };

        RemoveGraphicElementFromView(typeof(RhythmGraphics));
        layout.Children.Add(noteToPlayGraphics);
    }

    private void RemoveGraphicElementFromView(Type type)
    {
        List<GraphicsView> elementsToRemove = new List<GraphicsView>();

        foreach (var child in layout.Children)
        {
            if (child is GraphicsView childGraphicsView && childGraphicsView.Drawable.GetType() == type)
            {
                elementsToRemove.Add(childGraphicsView);
            }
        }

        foreach (var element in elementsToRemove)
        {
            layout.Remove(element);
        }
    }
}

