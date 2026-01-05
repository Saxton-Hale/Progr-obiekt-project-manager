using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Infrastructure.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        public IBoardRepository Boards { get; }

        public UnitOfWork()
        {
            Boards = new BoardRepository();
        }

        public void SaveChanges()
        {
            //in memory wiec zostaje tutaj puste
        }
    }
}
