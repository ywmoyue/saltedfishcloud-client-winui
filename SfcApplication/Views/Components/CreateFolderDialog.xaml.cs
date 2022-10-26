using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using SfcApplication.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Components
{
    public sealed partial class CreateFolderDialog : ContentDialog
    {
        private readonly DiskFileService m_diskFileService;
        public List<string> Paths { get; set; }
        public int UserId { get; set; }
        public bool IsCreated { get; private set; }

        public CreateFolderDialog(DiskFileService diskFileService)
        {
            m_diskFileService = diskFileService;
            this.InitializeComponent();
        }

        private async void CreateFolderDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await m_diskFileService.CreateFolder(ViewModel.Name, Paths, UserId);
            IsCreated = true;
        }
    }
}
