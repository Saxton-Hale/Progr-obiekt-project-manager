using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Interfaces
{
    public interface IBoardDataStore
    {
        List<Board> LoadBoards(string path);
        void SaveBoards(IEnumerable<Board> boards, string path);
    }
}
