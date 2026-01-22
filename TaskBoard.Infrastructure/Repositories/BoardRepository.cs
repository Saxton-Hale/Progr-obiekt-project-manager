using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Infrastructure.Repositories
{
    internal sealed class BoardRepository : IBoardRepository
    {
        private readonly List<Board> _boards;

        public BoardRepository(List<Board>? initial = null)
        {
            _boards = initial ?? new List<Board>();
        }

        public void ReplaceAll(IEnumerable<Board> boards)
        {
            _boards.Clear();
            _boards.AddRange(boards);
        }
        public void Add(Board board)
        {
            if (board is null) throw new ArgumentNullException(nameof(board));

            if (_boards.Any(b => b.Id == board.Id))
                throw new InvalidOperationException("Board with the same Id already exists");

            _boards.Add(board);
        }

        public IEnumerable<Board> GetAll()
        {
            return _boards.ToList();
        }

        public Board? GetById(Guid id)
        {
            return _boards.SingleOrDefault(b => b.Id == id);
        }

        public void Remove(Board board)
        {
            if (board is null) throw new ArgumentNullException(nameof(board));

            var existing = _boards.SingleOrDefault(b => b.Id == board.Id);
            if (existing is null)
                throw new InvalidOperationException("Board not found");

            _boards.Remove(existing);
        }

        public void Update(Board board)
        {
            if (board is null) throw new ArgumentNullException(nameof(board));

            var index = _boards.FindIndex(b => b.Id == board.Id);
            if (index < 0)
                throw new InvalidOperationException("Board not found");

            _boards[index] = board;
        }
    }
}
