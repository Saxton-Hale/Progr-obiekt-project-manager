using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Services;
using TaskBoard.Tests.Domain.TestDoubles;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.Tests.Domain.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        [TestMethod]
        public void ChangeStatus_WhenBoardNotFound_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new TaskService(uow);

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.ChangeStatus(Guid.NewGuid(), Guid.NewGuid(), (TaskStatus)1));
        }

        [TestMethod]
        public void ChangeStatus_WhenTaskNotFound_ThrowsInvalidOperationException()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.ChangeStatus(board.Id, Guid.NewGuid(), (TaskStatus)1));
        }

        [TestMethod]
        public void ChangeStatus_WhenTaskExists_UpdatesStatus_AndCallsUpdateAndSaveChanges()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            var task = new TestTask("T1");
            task.Status = (TaskStatus)0;

            column.AddTask(task);

            var updateBefore = repo.UpdateCallCount;
            var saveBefore = uow.SaveChangesCallCount;

            service.ChangeStatus(board.Id, task.Id, (TaskStatus)2);

            Assert.AreEqual((TaskStatus)2, task.Status);
            Assert.AreEqual(updateBefore + 1, repo.UpdateCallCount);
            Assert.AreEqual(saveBefore + 1, uow.SaveChangesCallCount);
        }

        [TestMethod]
        public void ChangePriority_WhenBoardNotFound_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new TaskService(uow);

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.ChangePriority(Guid.NewGuid(), Guid.NewGuid(), TaskPriority.High));
        }

        [TestMethod]
        public void ChangePriority_WhenTaskNotFound_ThrowsInvalidOperationException()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.ChangePriority(board.Id, Guid.NewGuid(), TaskPriority.Critical));
        }

        [TestMethod]
        public void ChangePriority_WhenTaskExists_UpdatesPriority_AndCallsUpdateAndSaveChanges()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            var task = new TestTask("T1");
            task.Priority = TaskPriority.Low;

            column.AddTask(task);

            var updateBefore = repo.UpdateCallCount;
            var saveBefore = uow.SaveChangesCallCount;

            service.ChangePriority(board.Id, task.Id, TaskPriority.Critical);

            Assert.AreEqual(TaskPriority.Critical, task.Priority);
            Assert.AreEqual(updateBefore + 1, repo.UpdateCallCount);
            Assert.AreEqual(saveBefore + 1, uow.SaveChangesCallCount);
        }

        [TestMethod]
        public void AssignTask_WhenUserIsNull_ThrowsArgumentNullException()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            Assert.ThrowsException<ArgumentNullException>(() =>
                service.AssignTask(board.Id, Guid.NewGuid(), null!));
        }

        [TestMethod]
        public void AssignTask_WhenBoardNotFound_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new TaskService(uow);

            var user = new User("A", "B", "a@b.com", UserRole.Member);

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.AssignTask(Guid.NewGuid(), Guid.NewGuid(), user));
        }

        [TestMethod]
        public void AssignTask_WhenTaskNotFound_ThrowsInvalidOperationException()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            var user = new User("A", "B", "a@b.com", UserRole.Member);

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.AssignTask(board.Id, Guid.NewGuid(), user));
        }

        [TestMethod]
        public void AssignTask_WhenTaskExists_AddsUserToTaskAssignedTo_AndCallsUpdateAndSaveChanges()
        {
            var (service, repo, uow, board, column) = CreateServiceWithBoardAndOneColumn();

            var task = new TestTask("T1");
            column.AddTask(task);

            var user = new User("A", "B", "a@b.com", UserRole.Member);

            var updateBefore = repo.UpdateCallCount;
            var saveBefore = uow.SaveChangesCallCount;

            service.AssignTask(board.Id, task.Id, user);

            Assert.IsTrue(task.AssignedTo.Any(u => u.Equals(user)));
            Assert.AreEqual(updateBefore + 1, repo.UpdateCallCount);
            Assert.AreEqual(saveBefore + 1, uow.SaveChangesCallCount);
        }

        [TestMethod]
        public void FilterByStatus_ReturnsOnlyMatchingTasks()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new TaskService(uow);

            var t1 = new TestTask("t1") { Status = (TaskStatus)1 };
            var t2 = new TestTask("t2") { Status = (TaskStatus)2 };
            var t3 = new TestTask("t3") { Status = (TaskStatus)1 };

            var result = service.FilterByStatus(new[] { t1, t2, t3 }, (TaskStatus)1).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(t1));
            Assert.IsTrue(result.Contains(t3));
            Assert.IsFalse(result.Contains(t2));
        }

        [TestMethod]
        public void FilterByPriority_ReturnsOnlyMatchingTasks()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new TaskService(uow);

            var t1 = new TestTask("t1") { Priority = TaskPriority.High };
            var t2 = new TestTask("t2") { Priority = TaskPriority.Low };
            var t3 = new TestTask("t3") { Priority = TaskPriority.High };

            var result = service.FilterByPriority(new[] { t1, t2, t3 }, TaskPriority.High).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(t1));
            Assert.IsTrue(result.Contains(t3));
            Assert.IsFalse(result.Contains(t2));
        }

        //helper
        private static (TaskService service, InMemoryBoardRepository repo, FakeUnitOfWork uow, Board board, Column column)
            CreateServiceWithBoardAndOneColumn()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new TaskService(uow);

            var board = new Board("B1");
            board.AddColumn("Todo");

            repo.Add(board);

            var column = board.Columns[0];
            return (service, repo, uow, board, column);
        }
    }
}
