using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Enums
{
    public enum TaskStatus
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2,
        ToVerify = 3,
        Blocked = 4
    }
}
