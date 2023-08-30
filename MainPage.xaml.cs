﻿using Microsoft.Maui.Platform;
using Plugin.Maui.Audio;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RhythmYardstick;

public partial class MainPage : ContentPage
{
    private Timer _timer;

    public MainPage()
    {
        BindingContext = new MainViewModel();
        InitializeComponent();
    }
}