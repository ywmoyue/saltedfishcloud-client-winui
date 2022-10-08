using AutoMapper;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;
using SfcApplication.Models.Common;
using SfcApplication.Models.Entities;
using SfcApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Components
{
    public sealed partial class DownloadingView : UserControl
    {
        public DownloadHostedService DownloadHostedService { get; set; }

        public DownloadingView()
        {
            this.InitializeComponent();
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            var view = sender as FrameworkElement;
            var item = view.DataContext as DownloadItemViewModel;
            DownloadHostedService.Resume(item.Id);
        }
    }
}
