using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Tests.Domain.TestDoubles;

namespace TaskBoard.Tests.Domain.Entities
{
    [TestClass]
    public class TaskItemComparableTests
    {
        [TestMethod]
        public void Sort_ByDueDate_NullIsLast()
        {
            var now = DateTime.Now;

            TaskItem a = new TestTask("A", now.AddDays(3));
            TaskItem b = new TestTask("B", now.AddDays(1));
            TaskItem c = new TestTask("C", null);
            TaskItem d = new TestTask("D", now.AddDays(2));

            var list = new List<TaskItem> { a, b, c, d };

            list.Sort();

            Assert.AreSame(b, list[0]);
            Assert.AreSame(d, list[1]);
            Assert.AreSame(a, list[2]);
            Assert.AreSame(c, list[3]);
        }
    }
}
