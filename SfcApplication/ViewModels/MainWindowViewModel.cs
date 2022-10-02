using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SfcApplication.Models.Common;
using SfcApplication.Views;

namespace SfcApplication.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        NavMenuItem m_navMenuSelectedItem;
        ObservableCollection<NavMenuItem> m_navMenu;

        public NavMenuItem NavMenuSelectedItem
        {
            get => m_navMenuSelectedItem;
            set => Set(ref m_navMenuSelectedItem, value);
        }

        public ObservableCollection<NavMenuItem> NavMenu
        {
            get => m_navMenu;
            set => Set(ref m_navMenu, value);
        }

        public MainWindowViewModel()
        {
            m_navMenu = new ObservableCollection<NavMenuItem>();
            InitNavMenu();
        }

        private void InitNavMenu()
        {
            m_navMenu.Clear();
            m_navMenu.Add(new NavMenuItem("公共网盘","/fileList/public"));
            m_navMenu.Add(new NavMenuItem("私人网盘"));
            m_navMenu.Add(new NavMenuItem("登陆", "/login"));
            m_navMenu.Add(new NavMenuItem("注册"));
            m_navMenu.Add(new NavMenuItem("下载管理","/download"));
        }
    }
}
