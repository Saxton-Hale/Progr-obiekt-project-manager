using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TaskBoard.App.Wpf.Commands;
using TaskBoard.App.Wpf.Views;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;

namespace TaskBoard.App.Wpf.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        private Board? _board;
        public Board? Board
        {
            get => _board;
            private set
            {
                if (_board == value) return;
                _board = value;

                OnPropertyChanged();
                RelayCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Column> Columns { get; } = new();

        private Column? _selectedColumn;
        public Column? SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                if (_selectedColumn == value) return;
                _selectedColumn = value;

                OnPropertyChanged();
                RelayCommand.RaiseCanExecuteChanged();
            }
        }

        private string _newColumnName = "";
        private string _newTaskTitle;

        public string NewColumnName
        {
            get => _newColumnName;
            set
            {
                if (_newColumnName == value) return;
                _newColumnName = value;

                OnPropertyChanged();
                RelayCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set
            {
                if (_newTaskTitle == value) return;
                _newTaskTitle = value;

                OnPropertyChanged();
                RelayCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddColumnCommand { get; }
        public RelayCommand DeleteColumnCommand { get; }

        public RelayCommand AddTaskToColumnCommand { get; } 
        public RelayCommand OpenCreateTaskDialogCommand { get; }

        public RelayCommand OpenTaskDetailsCommand { get; }

        public RelayCommand MoveTaskCommand { get; }

        public BoardViewModel()
        {
            AddColumnCommand = new RelayCommand(_ =>
            {
                if (Board is null) return;

                var name = NewColumnName.Trim();
                Board.AddColumn(name);

                ReloadColumns();
                NewColumnName = "";
            }, _ => Board != null && !string.IsNullOrWhiteSpace(NewColumnName));

            DeleteColumnCommand = new RelayCommand(_ =>
            {
                if (Board is null || SelectedColumn is null) return;

                Board.RemoveColumn(SelectedColumn.Id);

                ReloadColumns();
                SelectedColumn = null;
            }, _ => Board != null && SelectedColumn != null);

            AddTaskToColumnCommand = new RelayCommand(param =>
            {
                if (Board is null) return;
                if (param is not Column column) return;

                var title = NewTaskTitle.Trim();
                if (string.IsNullOrWhiteSpace(title)) return;

                var task = new FeatureTask(title, null, "General");
                column.AddTask(task);

                ReloadColumns();
                NewTaskTitle = "";
            }, param => Board != null && param is Column && !string.IsNullOrWhiteSpace(NewTaskTitle));

            OpenCreateTaskDialogCommand = new RelayCommand(param =>
            {
                if (Board is null) return;
                if (param is not Column column) return;

                var window = new TaskCreateWindow
                {
                    Owner = Application.Current.MainWindow
                };

                window.ShowDialog();

                if (!window.ViewModel.IsSaved) return;

                var vm = window.ViewModel;

                var task = new FeatureTask(vm.Title.Trim(), vm.DueDate, "General")
                {
                    Description = vm.Description,
                    Status = vm.SelectedStatus,
                    Priority = vm.SelectedPriority
                };

                column.AddTask(task);

                ReloadColumns();
            });

            OpenTaskDetailsCommand = new RelayCommand(param =>
            {
                if (param is not TaskItem task) return;

                var window = new TaskDetailsWindow(task)
                {
                    Owner = Application.Current.MainWindow
                };

                window.ShowDialog();

                if (window.ViewModel.IsSaved)
                    ReloadColumns();
            });

            MoveTaskCommand = new RelayCommand(param =>
            {
                if (Board is null) return;
                if (param is not (TaskItem task, Column from, Column to)) return;

                Board.MoveTask(task.Id, from.Id, to.Id);
                ReloadColumns();
            });

        }

        public void SetBoard(Board? board)
        {
            Board = board;

            ReloadColumns();
            SelectedColumn = null;
        }

        private void ReloadColumns()
        {
            Columns.Clear();

            if (Board is null) return;

            foreach (var col in Board.Columns)
                Columns.Add(col);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
