using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using SfcApplication.Models.Common;
using SfcApplication.Views;

namespace SfcApplication.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public NavMenuItem NavMenuSelectedItem { get; set; }

        public ObservableCollection<NavMenuItem> NavFullMenu { get; set; }

        [DependsOn(nameof(NavFullMenu))]
        public ObservableCollection<NavMenuItem> NavMenu => new ObservableCollection<NavMenuItem>(NavFullMenu.Where(x=>!x.Hidden));

        public MainWindowViewModel()
        {
            NavFullMenu = new ObservableCollection<NavMenuItem>();
            InitNavMenu();
        }

        public void UpdateNavMenu()
        {
            Set(nameof(NavMenu));
        }

        private void InitNavMenu()
        {
            NavFullMenu.Clear();
            NavFullMenu.Add(new NavMenuItem("公共网盘","/fileList/public"));
            NavFullMenu.Add(new NavMenuItem("私人网盘", "/fileList/private"));
            NavFullMenu.Add(new NavMenuItem("登陆", "/login"));
            NavFullMenu.Add(new NavMenuItem("注册"));
            NavFullMenu.Add(new NavMenuItem("用户中心","", hidden:true));
            NavFullMenu.Add(new NavMenuItem("下载管理","/download"));
        }
    }
}
