﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SfcApplication.Models.Entities;
using SfcApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SfcApplication.Extensions;
using SfcApplication.Models.Configs;
using SfcApplication.Views.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Components
{
    public sealed partial class DownloadedView : UserControl
    {
        public event EventHandler<DownloadItemViewModel> OpenFolderToFile; 

        public DownloadedView()
        {
            this.InitializeComponent();
        }

        private void OpenFolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var view = sender as FrameworkElement;
            var item = view.DataContext as DownloadItemViewModel;
            OpenFolderToFile?.Invoke(this, item);
        }
    }
}
