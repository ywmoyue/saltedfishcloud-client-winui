using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SfcApplication.Extensions;
using SfcApplication.Models.Entities;
using SfcApplication.Models.Mappers;

namespace SfcApplication.ViewModels
{
    public class FileListPageViewModel : BaseViewModel
    {
        public ObservableCollection<DiskFileInfoMapper> DiskFileInfos { get; set; }

        public ObservableCollection<string> Paths { get; set; }

        public List<DiskFileInfoMapper> SelectedDiskFileInfos { get; set; }

        public ICommand SetSelectedDiskFileInfos
        {
            get
            {
                return new SelectedCommand((s) => 
                    SelectedDiskFileInfos=s.ToList()
                );
            }
        }
        public class SelectedCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            public Action<IEnumerable<DiskFileInfoMapper>> m_action;
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var source=parameter as IEnumerable<object>;
                m_action(source.Cast<DiskFileInfoMapper>());
            }

            public SelectedCommand(Action<IEnumerable<DiskFileInfoMapper>> action)
            {
                this.m_action = action;
            }
        }
    }
}
