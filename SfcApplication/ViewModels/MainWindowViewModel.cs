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
    public class MainWindowViewModel : BaseViewModel
    {
        public NavMenuItem NavMenuSelectedItem { get; set; }

        public ObservableCollection<NavMenuItem> NavMenu { get; set; }

        public MainWindowViewModel()
        {
            NavMenu = new ObservableCollection<NavMenuItem>();
            InitNavMenu();
        }

        private void InitNavMenu()
        {
            NavMenu.Clear();
            NavMenu.Add(new NavMenuItem("公共网盘","/fileList/public"));
            NavMenu.Add(new NavMenuItem("私人网盘", "/fileList/private"));
            NavMenu.Add(new NavMenuItem("登陆", "/login"));
            NavMenu.Add(new NavMenuItem("注册"));
            NavMenu.Add(new NavMenuItem("下载管理","/download"));
        }
    }
}
