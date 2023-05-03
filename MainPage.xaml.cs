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

        YardstickGraphics yardstickGraphics = new YardstickGraphics();
        GraphicsView graphicsView = new GraphicsView() { HeightRequest = 500, WidthRequest = 700 };
        graphicsView.Drawable = yardstickGraphics;
        layout.Children.Add(graphicsView);
        _isStarted = true;
    }


    private void TimerCallback(object state)
    {
        if (_isStarted)
        {
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

        GraphicsView noteToPlayGraphics = new GraphicsView
        {
            Drawable = new RhythmGraphics(NoteToPlay),
            WidthRequest = YardstickGraphics.CanvasWidth,
            HeightRequest = YardstickGraphics.CanvasHeight * YardstickGraphics.VerticalIndentation
        };

        List<GraphicsView> elementsToRemove = new List<GraphicsView>();

        //foreach (var child in layout.Children)
        //{
        //    if (child is GraphicsView childGraphicsView && noteToPlayGraphics.Drawable is YardstickGraphics.RhythmGraphics)
        //    {
        //        elementsToRemove.Add(childGraphicsView);
        //    }
        //}

        //foreach (var element in elementsToRemove)
        //{
        //    layout.Children.Remove(element);
        //}

        layout.Children.Add(noteToPlayGraphics);
    }

    private Tuple<int, int> GetNoteToPlay()
    {
        int beatNumber;
        int subDivisionNumber;

        beatNumber = _randomBeat.Next(1, Configuration.BeatCount);
        subDivisionNumber = _randomSubdivision.Next(1, Configuration.SubdivisionCount * Configuration.SubdivisionCount);

        return new Tuple<int, int>(beatNumber, subDivisionNumber);
    }
}

