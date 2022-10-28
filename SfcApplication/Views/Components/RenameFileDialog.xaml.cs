using Microsoft.UI.Xaml.Controls;
using SfcApplication.Services;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Components
{
    public sealed partial class RenameFileDialog : ContentDialog
    {
        private readonly DiskFileService m_diskFileService;
        public List<string> Paths { get; set; }
        public int UserId { get; set; }

        public string NewName => ViewModel.Name;

        public string OldName
        {
            set { 
                ViewModel.OldName = value;
                ViewModel.Name = value;
            }
        }

        public bool IsOk { get; private set; }

        public RenameFileDialog(DiskFileService diskFileService)
        {
            m_diskFileService = diskFileService;
            this.InitializeComponent();
        }

        private async void RenameFileDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await m_diskFileService.RenameFile(ViewModel.OldName, ViewModel.Name, Paths, UserId);
            IsOk = true;
        }
    }
}
