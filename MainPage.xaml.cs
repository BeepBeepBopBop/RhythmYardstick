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
        _timer = new Timer(TimerCallback, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        _isStarted = true;
    }


    private async void TimerCallback(object state)
    {
        if (_isStarted)
        {
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
                StartNewExercise();
            }

            Application.Current.Dispatcher.Dispatch(DisplayBeat);
        }
    }

    private void StartNewExercise()
    {
        NoteToPlay = GetNoteToPlay();
        Application.Current.Dispatcher.Dispatch(DisplayNoteToPlay);
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
        GraphicsView beatIndexGraphics = new GraphicsView()
        {
            Drawable = new BeatIndexGraphics(_currentBeatNumber),
            WidthRequest = 600,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Start
        };

        RemoveGraphicElementFromView(typeof(BeatIndexGraphics));
        layout.Children.Insert(0, beatIndexGraphics);
    }

    private void DisplayNoteToPlay()
    {
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

