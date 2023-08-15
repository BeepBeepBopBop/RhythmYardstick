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


    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        rotatingLabel.Rotation = value;
        displayLabel.Text = String.Format("The Slider value is {0}", value);
    }
}

