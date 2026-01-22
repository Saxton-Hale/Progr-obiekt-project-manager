using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBoardRepository Boards { get; }
        void SaveChanges();
        void Reload();
    }
}
