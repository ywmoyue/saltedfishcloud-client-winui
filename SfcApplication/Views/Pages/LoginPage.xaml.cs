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
using SfcApplication.Services;
using SfcApplication.Extensions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class LoginPage : RoutePage
    {
        private UserClient m_userClient;
        private RouteService m_routeService;
        public LoginPage(UserClient userClient,RouteService routeService)
        {
            m_userClient = userClient;
            m_routeService = routeService;
            this.InitializeComponent();
        }
        
        private async void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var token=await m_userClient.GetToken(ViewModel.UserName, ViewModel.Password);
            // save token
            m_routeService.Push("/fileList/public");
        }
    }
}
