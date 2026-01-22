using System.Windows;
using TaskBoard.App.Wpf.ViewModels;
using TaskBoard.Domain.Entities;

namespace TaskBoard.App.Wpf.Views
{
    public partial class TaskDetailsWindow : Window
    {
        public TaskDetailsViewModel ViewModel { get; }

        public TaskDetailsWindow(TaskItem task)
        {
            InitializeComponent();
            ViewModel = new TaskDetailsViewModel(task, Close);
            DataContext = ViewModel;
        }
    }
}
