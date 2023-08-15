namespace RhythmYardstick;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = new SettingsViewModel();
    }

    void OnBpmSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        ((SettingsViewModel)BindingContext).BPM = (int)value;
    }

    void OnSubdivisionsSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        ((SettingsViewModel)BindingContext).SubdivisionCount = (int)value;
    }

    void OnBeatCountSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        ((SettingsViewModel)BindingContext).BeatCount = (int)value;
    }
}

