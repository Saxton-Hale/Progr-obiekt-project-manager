using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Comparers;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskBoard.Tests.Domain.TestDoubles;

namespace TaskBoard.Tests.Domain.Comparers
{
    [TestClass]
    public class TaskPriorityComparerTests
    {
        [TestMethod]
        public void Sort_ByPriority_Descending()
        {
            var comparer = new TaskPriorityComparer();

            var low = new TestTask("low") { Priority = TaskPriority.Low };
            var critical = new TestTask("critical") { Priority = TaskPriority.Critical };
            var high = new TestTask("high") { Priority = TaskPriority.High };
            var medium = new TestTask("medium") { Priority = TaskPriority.Medium };

            var list = new List<TaskItem> { low, critical, high, medium };

            list.Sort(comparer);

            Assert.AreSame(critical, list[0]);
            Assert.AreSame(high, list[1]);
            Assert.AreSame(medium, list[2]);
            Assert.AreSame(low, list[3]);
        }

        [TestMethod]
        public void Sort_SamePriority_UsesDueDateAscending_NullLast()
        {
            var comparer = new TaskPriorityComparer();
            var now = DateTime.Now;

            var a = new TestTask("a", now.AddDays(3)) { Priority = TaskPriority.High };
            var b = new TestTask("b", now.AddDays(1)) { Priority = TaskPriority.High };
            var c = new TestTask("c", null) { Priority = TaskPriority.High };
            var d = new TestTask("d", now.AddDays(2)) { Priority = TaskPriority.High };

            var list = new List<TaskItem> { a, b, c, d };

            list.Sort(comparer);

            Assert.AreSame(b, list[0]);
            Assert.AreSame(d, list[1]);
            Assert.AreSame(a, list[2]);
            Assert.AreSame(c, list[3]);
        }

        [TestMethod]
        public void Compare_WithNullTask_PutsNullLast()
        {
            var comparer = new TaskPriorityComparer();

            var x = new TestTask("x") { Priority = TaskPriority.Critical };
            TaskItem? y = null;

            var list = new List<TaskItem?> { y, x };

            Assert.IsTrue(comparer.Compare(x, null) < 0);
            Assert.IsTrue(comparer.Compare(null, x) > 0);
            Assert.AreEqual(0, comparer.Compare(null, null));
        }
    }
}
