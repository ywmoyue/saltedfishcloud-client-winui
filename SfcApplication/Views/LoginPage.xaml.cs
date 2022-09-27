using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SfcApplication.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class LoginPage : Page
    {
        private UserClient m_userClient;
        private IServiceProvider m_serviceProvider;
        public LoginPage(UserClient userClient,IServiceProvider serviceProvider)
        {
            m_userClient = userClient;
            m_serviceProvider = serviceProvider;
            this.InitializeComponent();
        }

        private async void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var token=await m_userClient.GetToken(ViewModel.UserName, ViewModel.Password);
            return;
        }
    }
}
