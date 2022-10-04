using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SfcApplication.Models.Common;
using SfcApplication.Models.Entities;
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
        public DownloadingView()
        {
            this.InitializeComponent();
            //ViewModel.DownloadItemList = new ObservableCollection<DownloadItem>();
            //ViewModel.DownloadItemList.Add(new DownloadItem()
            //{
            //    Id=0,
            //    DiskFileInfo=new DiskFileInfo()
            //    {
            //        Name="test",
            //        Size=50000,
            //        Suffix="txt"
            //    }
            //});
        }
    }
}
