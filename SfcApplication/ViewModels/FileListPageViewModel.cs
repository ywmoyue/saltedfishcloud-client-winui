using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using SfcApplication.Models.Mappers;
using PropertyChanged;

namespace SfcApplication.ViewModels
{
    public class FileListPageViewModel : BaseViewModel
    {
        public ObservableCollection<DiskFileInfoMapperViewModel> DiskFileInfos { get; set; }

        public ObservableCollection<string> Paths { get; set; }

        public ObservableCollection<DiskFileInfoMapperViewModel> SelectedDiskFileInfos { get; set; }
        public List<DiskFileInfoMapperViewModel> DraggingDiskFileInfos { get; set; }

        public bool IsSelectionBorderShow { get; set; }
        public Point SelectionBorderMouseSourcePoint { get; set; }
        public Point SelectionBorderMouseTargetPoint { get; set; }=new Point(0,0);

        [DependsOn(nameof(SelectionBorderMouseTargetPoint))]
        public double SelectionBorderWidth =>
            Math.Abs(SelectionBorderMouseTargetPoint.X - SelectionBorderMouseSourcePoint.X);

        [DependsOn(nameof(SelectionBorderMouseTargetPoint))]
        public double SelectionBorderHeight =>
            Math.Abs(SelectionBorderMouseTargetPoint.Y - SelectionBorderMouseSourcePoint.Y);

        [DependsOn(nameof(SelectionBorderMouseTargetPoint))]
        public double SelectionBorderPosX => SelectionBorderMouseTargetPoint.X < SelectionBorderMouseSourcePoint.X
            ? SelectionBorderMouseTargetPoint.X
            : SelectionBorderMouseSourcePoint.X;

        [DependsOn(nameof(SelectionBorderMouseTargetPoint))]
        public double SelectionBorderPosY => SelectionBorderMouseTargetPoint.Y < SelectionBorderMouseSourcePoint.Y
            ? SelectionBorderMouseTargetPoint.Y
            : SelectionBorderMouseSourcePoint.Y;
        public int UserId { get; set; }

        public ICommand SetSelectedDiskFileInfos
        {
            get
            {
                return new SelectedCommand((s) => 
                    SelectedDiskFileInfos=new ObservableCollection<DiskFileInfoMapperViewModel>(s)
                );
            }
        }
        public class SelectedCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            public Action<IEnumerable<DiskFileInfoMapperViewModel>> m_action;
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var source=parameter as IEnumerable<object>;
                m_action(source.Cast<DiskFileInfoMapperViewModel>());
            }

            public SelectedCommand(Action<IEnumerable<DiskFileInfoMapperViewModel>> action)
            {
                this.m_action = action;
            }
        }
    }
}
