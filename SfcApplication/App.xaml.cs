using Microsoft.UI.Xaml;
using System;
using System.IO;
using Windows.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SfcApplication.Clients;
using SfcApplication.Models.Configs;
using SfcApplication.Services;
using AutoMapper;
using SfcApplication.Views.Pages;
using System.Threading.Tasks;
using SfcApplication.HostedServices;
using Downloader;
using Windows.Storage;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SfcApplication.Extensions;
using SfcApplication.Views.Components;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SfcApplication
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost m_host;
        private IConfiguration m_configuration;
        private string m_configurationPath;
        private Window m_window;
        public IntPtr WindowHandle;

        public App()
        {
            InitConfiguration();
            m_host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                //.Start();
                .Build();
            this.InitializeComponent();
        }

        private void InitConfiguration()
        {
            var packagePath = Package.Current.InstalledLocation.Path;
            var localConfigPath = ApplicationData.Current.LocalFolder.Path+"\\config";
            m_configurationPath= $"{localConfigPath}\\appsettings.json";
            if (!File.Exists(m_configurationPath))
            {
                if (!Directory.Exists(localConfigPath)) Directory.CreateDirectory(localConfigPath);
                File.Copy($"{packagePath}/appsettings.template.json",m_configurationPath);
            }
#if DEBUG
            File.Copy($"{packagePath}/appsettings.template.json", m_configurationPath, true);
#endif
            var builder = new ConfigurationBuilder()
                .SetBasePath(localConfigPath)
                .AddJsonFile("appsettings.json", optional: false);
            m_configuration = builder.Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var clientConfig = m_configuration.GetSection(nameof(ClientConfig)).Get<ClientConfig>();
            clientConfig.ConfigPath = m_configurationPath;
            var downloadConfig = m_configuration.GetSection(nameof(DownloadConfiguration)).Get<DownloadConfiguration>();
            var userConfig = m_configuration.GetSection(nameof(UserConfig)).Get<UserConfig>();
            ConfigureHttp();
            services.AddSingleton(m_configuration);
            services.AddSingleton(clientConfig);
            services.AddSingleton(downloadConfig);
            services.AddSingleton(userConfig);
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(expression =>
                expression.AddMaps(GetType().Assembly))));
            services.AddScoped<HelloClient>();
            services.AddScoped<UserClient>();
            services.AddScoped<DiskFileClient>();
            services.AddScoped<DiskFileService>();
            services.AddSingleton<RouteService>();
            services.AddSingleton<ToastService>();
            services.AddSingleton<ConfigService>();
            services.AddScoped<LocalFileIOService>();
            services.AddTransient<ConfirmDialog>();
            services.AddTransient<CreateFolderDialog>();
            services.AddTransient<RenameFileDialog>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<FileListPage>();
            services.AddSingleton<LoginPage>();
            services.AddSingleton<DownloadPage>();
            services.AddSingleton<UserSpacePage>();
            //services.AddScoped<DownloadingView>();
            services.AddSingleton<DownloadHostedService>();
            services.AddHostedService((serviceProvider) => serviceProvider.GetRequiredService<DownloadHostedService>());
            services.AddSingleton<UserHostedService>();
            services.AddHostedService((serviceProvider) => serviceProvider.GetRequiredService<UserHostedService>());
        }

        private void ConfigureHttp()
        {
            FlurlHttp.Configure(settings =>
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
                settings.UrlEncodedSerializer = new DataContractUrlEncodedSerializer();
            });
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            // start hostedServices
            StartHostedServices(m_host.Services).ContinueWith(task => Console.WriteLine(task.Exception));
            // start window
            m_window = m_host.Services.GetService<MainWindow>();
            WindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            m_window?.Activate();
        }

        private async Task StartHostedServices(IServiceProvider services)
        {
            await services.GetRequiredService<DownloadHostedService>().StartAsync(System.Threading.CancellationToken.None);
            await services.GetRequiredService<UserHostedService>().StartAsync(System.Threading.CancellationToken.None);
        }
    }
}
