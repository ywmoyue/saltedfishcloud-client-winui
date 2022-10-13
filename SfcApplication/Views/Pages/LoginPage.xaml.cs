using Microsoft.UI.Xaml;
using SfcApplication.Clients;
using SfcApplication.Services;
using SfcApplication.Extensions;
using SfcApplication.HostedServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : RoutePage
    {
        private readonly UserHostedService m_userHostedService;
        private readonly ToastService m_toastService;
        private RouteService m_routeService;
        public LoginPage(UserClient userClient,RouteService routeService, UserHostedService userHostedService, ToastService toastService)
        {
            m_routeService = routeService;
            m_userHostedService = userHostedService;
            m_toastService = toastService;
            this.InitializeComponent();
        }
        
        private async void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var (success,msg) = await m_userHostedService.Login(ViewModel.UserName, ViewModel.Password);
            if (success)
            {
                m_toastService.Show("登录成功");
                m_routeService.Push("/fileList/public");
            }
            else
            {
                m_toastService.Show(msg);
            }
        }
    }
}
