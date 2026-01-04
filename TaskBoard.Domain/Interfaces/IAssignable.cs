using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Interfaces
{
    internal interface IAssignable
    {
        void AssignTo(User user);
        void UnAssign(User user);
        void UnAssignAll();
    }
}
