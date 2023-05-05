using Plugin.Maui.Audio;

namespace RhythmYardstick;

public partial class MainPage : ContentPage
{
    private Timer _timer;

    private bool _isStarted;

    private TimeSpan _elapsedTime;

    private int _elapsedRounds;

    private int _currentRoundBeatCount;

    private Random _randomBeat = new Random();

    private Random _randomSubdivision = new Random();

    public Tuple<int, int> NoteToPlay { get; set; }

    public MainPage()
    {
        InitializeComponent();
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        StartNewExercise();
        _isStarted = true;
    }


    private async void TimerCallback(object state)
    {
        if (_isStarted)
        {
            var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("drumsticks.mp3"));

            audioPlayer.Play();

            _currentRoundBeatCount++;
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));

            if (_currentRoundBeatCount > Configuration.BeatCount)
            {
                _currentRoundBeatCount = 0;
                _elapsedRounds++;
            }

            if (_elapsedRounds == Configuration.RoundCount)
            {
                _elapsedRounds = 0;
                StartNewExercise();
            }
        }
    }

    private void StartNewExercise()
    {
        NoteToPlay = GetNoteToPlay();
        Application.Current.Dispatcher.Dispatch(DisplayRhythm);
    }

    private Tuple<int, int> GetNoteToPlay()
    {
        int beatNumber;
        int subDivisionNumber;

        beatNumber = _randomBeat.Next(1, Configuration.BeatCount);
        subDivisionNumber = _randomSubdivision.Next(0, Configuration.SubdivisionCount * Configuration.SubdivisionCount - 1);

        return new Tuple<int, int>(beatNumber, subDivisionNumber);
    }

    private void DisplayRhythm()
    {
        GraphicsView noteToPlayGraphics = new GraphicsView
        {
            Drawable = new RhythmGraphics(NoteToPlay),
            WidthRequest = 600,
            HeightRequest = 100,
            HorizontalOptions = LayoutOptions.Start,
        };

        List<GraphicsView> elementsToRemove = new List<GraphicsView>();

        foreach (var child in layout.Children)
        {
            if (child is GraphicsView childGraphicsView && childGraphicsView.Drawable is RhythmGraphics)
            {
                elementsToRemove.Add(childGraphicsView);
            }
        }

        foreach (var element in elementsToRemove)
        {
            layout.Remove(element);
        }

        layout.Children.Add(noteToPlayGraphics);
    }
}

