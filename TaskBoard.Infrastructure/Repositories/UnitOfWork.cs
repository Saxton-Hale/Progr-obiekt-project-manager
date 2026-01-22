using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IBoardRepository Boards { get; }

        private readonly IBoardDataStore _dataStore;
        private readonly string _path;

        public UnitOfWork(IBoardDataStore dataStore, string path)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentException("Path cannot be empty", nameof(path)) : path;

            var loaded = _dataStore.LoadBoards(_path);
            Boards = new BoardRepository(loaded);
        }

        public void SaveChanges()
        {
            var boards = Boards.GetAll();
            _dataStore.SaveBoards(boards, _path);
        }

        public void Reload()
        {
            var loaded = _dataStore.LoadBoards(_path);

            if (Boards is BoardRepository repo)
                repo.ReplaceAll(loaded);
            else
                throw new InvalidOperationException("Boards repo does not support ReplaceAll.");
        }

    }
}
