namespace RhythmYardstick;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }


    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        window.MinimumHeight = 500;
        window.MinimumWidth = 800;
        return window;
    }
}
