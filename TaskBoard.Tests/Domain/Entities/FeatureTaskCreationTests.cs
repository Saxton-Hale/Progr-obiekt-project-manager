using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Tests.Domain.Entities
{
    [TestClass]
    public class FeatureTaskCreationTests
    {
        [TestMethod]
        public void FeatureTask_CreatesCorrectly()
        {
            var due = DateTime.Now.AddDays(2);
            var task = new FeatureTask("Add export", due, "Module name");

            Assert.IsNotNull(task);
            Assert.AreEqual("Add export", task.Title);
            Assert.AreEqual(task.DueDate, due);
            Assert.AreEqual(task.ModuleName, "Module name");
        }
    }
}
