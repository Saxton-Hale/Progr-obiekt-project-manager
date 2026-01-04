using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Exceptions
{
    internal class InvalidDeadlineException : DomainException 
    {
        public InvalidDeadlineException(string message) : base(message) { }
    }
}
