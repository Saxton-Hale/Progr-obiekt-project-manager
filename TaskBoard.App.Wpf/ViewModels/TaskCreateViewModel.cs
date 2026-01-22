using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.App.Wpf.Commands;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.App.Wpf.ViewModels
{
    public class TaskCreateViewModel : INotifyPropertyChanged
    {
        private string _title = "";
        public string Title
        {
            get => _title;
            set { if (_title == value) return; _title = value; OnPropertyChanged(); RelayCommand.RaiseCanExecuteChanged(); }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set { if (_description == value) return; _description = value; OnPropertyChanged(); }
        }

        public List<TaskStatus> StatusOptions { get; } = new()
        {
            TaskStatus.ToDo,
            TaskStatus.InProgress,
            TaskStatus.Done,
            TaskStatus.ToVerify,
            TaskStatus.Blocked
        };

        public List<TaskPriority> PriorityOptions { get; } = new()
        {
            TaskPriority.Low,
            TaskPriority.Medium,
            TaskPriority.High,
            TaskPriority.Critical
        };

        private TaskStatus _selectedStatus = TaskStatus.ToDo;
        public TaskStatus SelectedStatus
        {
            get => _selectedStatus;
            set { if (_selectedStatus == value) return; _selectedStatus = value; OnPropertyChanged(); }
        }

        private TaskPriority _selectedPriority = TaskPriority.Medium;
        public TaskPriority SelectedPriority
        {
            get => _selectedPriority;
            set { if (_selectedPriority == value) return; _selectedPriority = value; OnPropertyChanged(); }
        }

        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get => _dueDate;
            set { if (_dueDate == value) return; _dueDate = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        // Wynik okna
        public bool IsSaved { get; private set; }

        public TaskCreateViewModel(Action closeWindow)
        {
            SaveCommand = new RelayCommand(_ =>
            {
                IsSaved = true;
                closeWindow();
            }, _ => !string.IsNullOrWhiteSpace(Title));

            CancelCommand = new RelayCommand(_ =>
            {
                IsSaved = false;
                closeWindow();
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
