using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Domain.Services
{
    internal class BoardService
    {
        private readonly IUnitOfWork _uow;
        public BoardService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public Board CreateBoard(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty");

            var board = new Board(name.Trim());
            _uow.Boards.Add(board);
            _uow.SaveChanges();
            return board;
        }

        public void AddColumn(Guid boardId, string columnName)
        {
            var board = _uow.Boards.GetById(boardId)
                ?? throw new InvalidOperationException("Board not found");

            board.AddColumn(columnName);
            _uow.Boards.Update(board);
            _uow.SaveChanges();
        }
        
        public void RemoveColumn(Guid boardId, Guid columnId)
        {
            var board = _uow.Boards.GetById(boardId)
                ?? throw new InvalidOperationException("Board not found");

            board.RemoveColumn(columnId);
            _uow.Boards.Update(board);
            _uow.SaveChanges();
        }

        public void MoveTask(Guid boardId, Guid taskId, Guid fromColumnId, Guid toColumnId)
        {
            var board = _uow.Boards.GetById(boardId)
                        ?? throw new InvalidOperationException("Board not found.");

            board.MoveTask(taskId, fromColumnId, toColumnId);
            _uow.Boards.Update(board);
            _uow.SaveChanges();
        }
    }
}
