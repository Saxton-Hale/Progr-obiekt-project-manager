using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Tests.Domain.TestDoubles
{
    internal class FakeUnitOfWork : IUnitOfWork
    {
        public IBoardRepository Boards { get; }

        public int SaveChangesCallCount { get; private set; }

        public FakeUnitOfWork(IBoardRepository boards)
        {
            Boards = boards ?? throw new ArgumentNullException(nameof(boards));
        }

        public void SaveChanges()
        {
            SaveChangesCallCount++;
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }
    }
}
