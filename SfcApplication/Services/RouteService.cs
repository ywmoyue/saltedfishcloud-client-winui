using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SfcApplication.Models.Common;
using SfcApplication.Views;

namespace SfcApplication.Services
{
    internal class RouteService
    {
        private Frame m_frame;
        private List<RouteItem> m_routes;
        public RouteItem CurrentRoute { get; private set; }
        private IServiceProvider m_serviceProvider;

        public RouteService(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }

        public void Init(Frame frame)
        {
            m_frame = frame;
            m_routes = new List<RouteItem>();
            m_routes.Add(new RouteItem() { Name = "login", PageType = typeof(LoginPage), Path = "/login" });
            m_routes.Add(new RouteItem() { Name = "publicFile", PageType = typeof(FileListPage), Path = "/fileList/public" });
        }

        public void Push(string path, object query = null)
        {
            var route = m_routes.FirstOrDefault(x => x.Path == path);
            var page = m_serviceProvider.GetRequiredService(route.PageType) as RoutePage;
            m_frame.Content = page;
            CurrentRoute = route;
            page.OnNavigated(query);
        }
    }
}
