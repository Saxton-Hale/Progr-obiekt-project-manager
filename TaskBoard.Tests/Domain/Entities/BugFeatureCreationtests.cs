using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Tests.Domain.Entities
{
    [TestClass]
    public class BugFeatureCreationTests
    {
        [TestMethod]
        public void BugTask_CreatesCorrectly()
        {
            var due = DateTime.Now.AddDays(2);
            var task = new BugTask("Fix auth", due);

            Assert.AreEqual("Fix auth", task.Title);
            Assert.AreEqual(task.DueDate, due);
        }
    }
}
