using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Interfaces
{
    public interface IBoardRepository
    {
        IEnumerable<Board> GetAll();
        Board? GetById(Guid id);

        void Add(Board board);
        void Update(Board board);
        void Remove(Board board);
    }
}
