using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Tests.Domain.TestDoubles
{
    internal class TestTask : TaskItem
    {
        public TestTask(string title, DateTime? dueDate = null) : base(title, dueDate) { }
    }
}
