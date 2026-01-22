using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.App.Wpf.Commands;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.App.Wpf.ViewModels
{
    public class TaskDetailsViewModel : INotifyPropertyChanged
    {
        private readonly TaskItem _task;

        private string _title;
        public string Title { get => _title; set { if (_title == value) return; _title = value; OnPropertyChanged(); RelayCommand.RaiseCanExecuteChanged(); } }

        private string _description;
        public string Description { get => _description; set { if (_description == value) return; _description = value; OnPropertyChanged(); } }

        public List<TaskStatus> StatusOptions { get; } = new()
        {
        TaskStatus.ToDo, TaskStatus.InProgress, TaskStatus.Done, TaskStatus.ToVerify, TaskStatus.Blocked
        };

        public List<TaskPriority> PriorityOptions { get; } = new()
        {
        TaskPriority.Low, TaskPriority.Medium, TaskPriority.High, TaskPriority.Critical
        };

        private TaskStatus _selectedStatus;
        public TaskStatus SelectedStatus { get => _selectedStatus; set { if (_selectedStatus == value) return; _selectedStatus = value; OnPropertyChanged(); } }

        private TaskPriority _selectedPriority;
        public TaskPriority SelectedPriority { get => _selectedPriority; set { if (_selectedPriority == value) return; _selectedPriority = value; OnPropertyChanged(); } }

        private DateTime? _dueDate;
        public DateTime? DueDate { get => _dueDate; set { if (_dueDate == value) return; _dueDate = value; OnPropertyChanged(); } }

        public bool IsSaved { get; private set; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public TaskDetailsViewModel(TaskItem task, Action closeWindow)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));

            _title = task.Title;
            _description = task.Description ?? "";
            _selectedStatus = task.Status;
            _selectedPriority = task.Priority;
            _dueDate = task.DueDate;

            SaveCommand = new RelayCommand(_ =>
            {
                _task.Title = Title.Trim();
                _task.Description = Description;
                _task.Status = SelectedStatus;
                _task.Priority = SelectedPriority;

                _task.ChangeDueDate(DueDate);

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
