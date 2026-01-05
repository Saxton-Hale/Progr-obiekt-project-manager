using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Services;
using TaskBoard.Tests.Domain.TestDoubles;

namespace TaskBoard.Tests.Domain.Services
{
    [TestClass]
    public class BoardServiceTests
    {
        [TestMethod]
        public void CreateBoard_WithWhiteSpaceName_throwsArgumentException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            Assert.ThrowsException<ArgumentException>(() =>
            service.CreateBoard(" "));
        }

        [TestMethod]
        public void CreateBoard_WithValidName_AddsBoardAndSavesChanges()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            var board = service.CreateBoard(" My Board ");

            Assert.IsNotNull(board);
            Assert.AreEqual(1, repo.AddCallCount);
            Assert.AreEqual(1, uow.SaveChangesCallCount);

            var nameProp = board.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic);
            if (nameProp != null)
            {
                var name = nameProp.GetValue(board) as string;
                Assert.AreEqual("My Board", name);
            }
        }

        [TestMethod]
        public void AddColumn_WhenBoardNotFound_ThrowsInvalidOperationOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            Assert.ThrowsException<InvalidOperationException>(() =>
            service.AddColumn(Guid.NewGuid(), "ToDo"));
        }

        [TestMethod]
        public void AddColumn_WhenBoardExists_UpdatesAndSavesChanges()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            var board = service.CreateBoard("Board");
            var beforeCount = GetColumnsCount(board);

            service.AddColumn(board.Id, "ToDo");

            var afterCount = GetColumnsCount(board);

            Assert.AreEqual(beforeCount + 1, afterCount);
            Assert.AreEqual(1, repo.UpdateCallCount);
            Assert.AreEqual(2, uow.SaveChangesCallCount);
        }

        [TestMethod]
        public void RemoveColumn_WhenBoardNotFound_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            Assert.ThrowsException<InvalidOperationException>(() =>
            service.RemoveColumn(Guid.NewGuid(), Guid.NewGuid()));
        }

        [TestMethod]
        public void RemoveColumn_WhenBoardExists_UpdatesAndSavesChanges()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            var board = service.CreateBoard("Board");
            service.AddColumn(board.Id, "Todo");

            var columnId = GetFirstColumnId(board);
            var beforeCount = GetColumnsCount(board);

            service.RemoveColumn(board.Id, columnId);

            var afterCount = GetColumnsCount(board);

            Assert.AreEqual(beforeCount - 1, afterCount);
            Assert.AreEqual(2, repo.UpdateCallCount);
            Assert.AreEqual(3, uow.SaveChangesCallCount);
        }

        [TestMethod]
        public void MoveTask_WhenBoardNotFound_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.MoveTask(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        }

        [TestMethod]
        public void MoveTask_WhenBoardColumnsAndTaskExist_MovesTaskBetweenColumns_AndSaves()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            var board = service.CreateBoard("Board");
            service.AddColumn(board.Id, "From");
            service.AddColumn(board.Id, "To");

            var fromColumn = board.Columns[0];
            var toColumn = board.Columns[1];

            var task = new TestTask("Task 1");
            fromColumn.AddTask(task);

            var updateBefore = repo.UpdateCallCount;
            var saveBefore = uow.SaveChangesCallCount;


            service.MoveTask(board.Id, task.Id, fromColumn.Id, toColumn.Id);


            Assert.IsFalse(fromColumn.Tasks.Any(t => t.Id == task.Id));
            Assert.IsTrue(toColumn.Tasks.Any(t => t.Id == task.Id));

            Assert.AreEqual(updateBefore + 1, repo.UpdateCallCount);
            Assert.AreEqual(saveBefore + 1, uow.SaveChangesCallCount);
        }

        [TestMethod]
        public void MoveTask_WhenSourceColumnNotFound_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            var board = service.CreateBoard("Board");
            service.AddColumn(board.Id, "To");

            var toColumn = board.Columns[0];

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.MoveTask(board.Id, Guid.NewGuid(), Guid.NewGuid(), toColumn.Id));
        }

        [TestMethod]
        public void MoveTask_WhenTaskNotInSourceColumn_ThrowsInvalidOperationException()
        {
            var repo = new InMemoryBoardRepository();
            var uow = new FakeUnitOfWork(repo);
            var service = new BoardService(uow);

            var board = service.CreateBoard("Board");
            service.AddColumn(board.Id, "From");
            service.AddColumn(board.Id, "To");

            var fromColumn = board.Columns[0];
            var toColumn = board.Columns[1];

            Assert.ThrowsException<InvalidOperationException>(() =>
                service.MoveTask(board.Id, Guid.NewGuid(), fromColumn.Id, toColumn.Id));
        }

        //helpersy
        private static int GetColumnsCount(object board)
        {
            var t = board.GetType();

            var prop = t.GetProperty("Columns", BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic);
            if (prop?.GetValue(board) is System.Collections.ICollection col1) return col1.Count;

            var field = t.GetField("_columns", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field?.GetValue(board) is System.Collections.ICollection col2) return col2.Count;

            throw new InvalidOperationException("Cannot read columns from Board");
        }

        private static Guid GetFirstColumnId(object board)
        {
            var t = board.GetType();

            object? columnsObj = null;

            var prop = t.GetProperty("Columns", BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic);
            if (prop != null) columnsObj = prop.GetValue(board);

            if (columnsObj is null)
            {
                var field = t.GetField("_columns", BindingFlags.Instance | BindingFlags.NonPublic);
                columnsObj = field?.GetValue(board);
            }

            if (columnsObj is not System.Collections.IEnumerable columnsEnum)
                throw new InvalidOperationException("Cannot enumerate columns from Board");

            var first = columnsEnum.Cast<object>().FirstOrDefault()
                        ?? throw new InvalidOperationException("Board has no columns");

            var idProp = first.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic)
                        ?? throw new InvalidOperationException("Column has no Id property");

            var idVal = idProp.GetValue(first);
            if (idVal is Guid id) return id;

            throw new InvalidOperationException("Column.Id is not Guid");
        }
    }
}
