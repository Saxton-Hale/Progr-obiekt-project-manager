using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Exceptions;
using TaskBoard.Tests.Domain.TestDoubles;

namespace TaskBoard.Tests.Domain.Entities
{
    [TestClass]
    public class TaskItemValidationTests
    {
        [TestMethod]
        public void CreatingTask_WithEmptyTitle_ThrowsInvalidTaskException()
        {
            Assert.ThrowsException<InvalidTaskException>(() =>
            {
                _ = new TestTask(" ");
            });
        }

        [TestMethod]
        public void CreatingTask_WithDueDateEarlierThanCreatedAt_ThrowsInvalidDeadlineException()
        {
            Assert.ThrowsException<InvalidDeadlineException>(() =>
            {
                _ = new TestTask("Title", DateTime.Now.AddDays(-6));
            });
        }
    }
}
