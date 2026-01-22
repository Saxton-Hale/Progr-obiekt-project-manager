using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TaskBoard.App.Wpf.Commands;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.App.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Board> Boards { get; } = new();
        public BoardViewModel BoardViewModel { get; } = new();

        private Board? _selectedBoard;
        public Board? SelectedBoard
        {
            get => _selectedBoard;
            set
            {
                if (_selectedBoard == value) return;
                _selectedBoard = value;
                OnPropertyChanged();

                BoardViewModel.SetBoard(_selectedBoard);
                RelayCommand.RaiseCanExecuteChanged();
            }
        }

        private string _newBoardName = "";
        public string NewBoardName
        {
            get => _newBoardName;
            set
            {
                if (_newBoardName == value) return;
                _newBoardName = value;
                OnPropertyChanged();
                RelayCommand.RaiseCanExecuteChanged();
            }
        }

        private readonly IUnitOfWork _unitOfWork;

        public RelayCommand AddBoardCommand { get; }
        public RelayCommand DeleteBoardCommand { get; }
        public RelayCommand SaveChangesCommand { get; }
        public RelayCommand LoadCommand { get; }

        public MainViewModel()
        {
            _unitOfWork = App.UnitOfWork;

            RefreshBoardsFromRepository();

            SelectedBoard = Boards.FirstOrDefault();

            AddBoardCommand = new RelayCommand(_ =>
            {
                System.Diagnostics.Debug.WriteLine("ADD CLICKED");
                var name = NewBoardName.Trim();

                var board = new Board(name);
                _unitOfWork.Boards.Add(board);

                RefreshBoardsFromRepository();

                NewBoardName = "";
                SelectedBoard = Boards.LastOrDefault();
            }, _ => !string.IsNullOrWhiteSpace(NewBoardName));

            DeleteBoardCommand = new RelayCommand(_ =>
            {
                if (SelectedBoard is null) return;

                _unitOfWork.Boards.Remove(SelectedBoard);
                RefreshBoardsFromRepository();

                SelectedBoard = Boards.FirstOrDefault();
            }, _ => SelectedBoard != null);


            SaveChangesCommand = new RelayCommand(_ => _unitOfWork.SaveChanges());

            LoadCommand = new RelayCommand(_ =>
            {
                _unitOfWork.Reload();
                RefreshBoardsFromRepository();
                SelectedBoard = Boards.FirstOrDefault();
            });
        }

        private void RefreshBoardsFromRepository()
        {
            Boards.Clear();
            foreach (var board in _unitOfWork.Boards.GetAll())
                Boards.Add(board);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
