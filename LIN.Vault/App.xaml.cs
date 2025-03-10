﻿using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace LIN.Vault;

public partial class App : Microsoft.Maui.Controls.Application
{
    public App()
    {
        InitializeComponent();
        

        var page = new MainPage()
        {
        };
        NavigationPage.SetHasNavigationBar(page, false);
        MainPage = new NavigationPage(page);

        // Ajustar pantalla al teclado.
        Current?.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

    }
}