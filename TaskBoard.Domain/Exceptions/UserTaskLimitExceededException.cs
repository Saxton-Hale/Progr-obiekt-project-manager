using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Exceptions
{
    internal class UserTaskLimitExceededException : DomainException
    {
        public UserTaskLimitExceededException(string message) : base(message) { }
    }
}
