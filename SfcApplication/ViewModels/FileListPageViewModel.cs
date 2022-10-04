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
    internal class FileListPageViewModel : BaseViewModel
    {
        private ObservableCollection<DiskFileInfoMapper> m_diskFileInfos;
        private ObservableCollection<string> m_paths;
        private List<DiskFileInfoMapper> m_selectedDiskFileInfos;
        public ObservableCollection<DiskFileInfoMapper> DiskFileInfos
        {
            get => m_diskFileInfos;
            set => Set(ref m_diskFileInfos, value);
        }

        public ObservableCollection<string> Paths
        {
            get => m_paths;
            set => Set(ref m_paths, value);
        }

        public List<DiskFileInfoMapper> SelectedDiskFileInfos
        {
            get => m_selectedDiskFileInfos;
            set => Set(ref m_selectedDiskFileInfos, value);
        }

        public ICommand SetSelectedDiskFileInfos
        {
            get
            {
                return new SelectedCommand((s) => 
                this.SelectedDiskFileInfos=s.ToList()
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
