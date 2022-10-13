using Windows.Storage.Pickers;
using WinRT;

namespace SfcApplication.Extensions
{
    public static class FolderPickerExtension
    {
        public static void AttachWindow(this FolderPicker picker)
        {
            var windowHandle = (App.Current as App).WindowHandle;
            var pickerWindow = picker.As<IInitializeWithWindow>();
            pickerWindow.Initialize(windowHandle);
        }
    }
}
