using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Tests.Domain.TestDoubles
{
    internal sealed class InMemoryBoardRepository : IBoardRepository
    {
        private readonly Dictionary<Guid, Board> _store = new();

        public int AddCallCount { get; private set; }
        public int UpdateCallCount { get; private set; }
        public int RemoveCallCount { get; private set; }
        public int GetByIdCallCount { get; private set; }
        public int GetAllCallCount { get; private set; }

        public IEnumerable<Board> GetAll()
        {
            GetAllCallCount++;
            return _store.Values.ToList();
        }

        public Board? GetById(Guid id)
        {
            GetByIdCallCount++;
            _store.TryGetValue(id, out var board);
            return board;
        }

        public void Add(Board board)
        {
            AddCallCount++;
            _store[board.Id] = board;
        }

        public void Update(Board board)
        {
            UpdateCallCount++;
            _store[board.Id] = board;
        }

        public void Remove(Board board)
        {
            RemoveCallCount++;
            _store.Remove(board.Id);
        }
    }
}
