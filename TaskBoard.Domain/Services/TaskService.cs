using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;
using TaskBoard.Domain.Interfaces;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.Domain.Services
{
    internal class TaskService
    {
        private readonly IUnitOfWork _uow;
        public TaskService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public void ChangeStatus(Guid boardId, Guid taskId, TaskStatus newStatus)
        {
            var board = _uow.Boards.GetById(boardId)
                ?? throw new InvalidOperationException("Board not found");

            var task = board.GetAllTasks().SingleOrDefault(t => t.Id == taskId)
                ?? throw new InvalidOperationException("Task not found");

            task.Status = newStatus;

            _uow.Boards.Update(board);
            _uow.SaveChanges();
        }

        public void ChangePriority(Guid boardId, Guid taskId, TaskPriority newPriority)
        {
            var board = _uow.Boards.GetById(boardId)
                        ?? throw new InvalidOperationException("Board not found");

            var task = board.GetAllTasks().SingleOrDefault(t => t.Id == taskId)
                       ?? throw new InvalidOperationException("Task not found");

            task.Priority = newPriority;

            _uow.Boards.Update(board);
            _uow.SaveChanges();
        }

        public void AssignTask(Guid boardId, Guid taskId, User user/*, int maxTasksPerUser*/)
        {
            if (user is null) {
                throw new ArgumentNullException(nameof(user));
            }

            var board = _uow.Boards.GetById(boardId)
                ?? throw new InvalidOperationException("Board not found");

            var task = board.GetAllTasks().SingleOrDefault(t => t.Id == taskId)
                ?? throw new InvalidOperationException("Task not found");

            if (task is not IAssignable assignable)
                throw new InvalidOperationException("This taks is not assignable");

            //var currentAssignedCount = CountTasksAssingedToUser(user.Id);

            //if(currentAssignedCount >= maxTasksPerUser)
            //{
            //    throw new userTaskLimitExceededException(
            //        $"USer has reached the task limit ({maxTasksPerUser})")
            //}

            //Na pozniej jezeli bede chcial to dodac

            assignable.AssignTo(user);

            _uow.Boards.Update(board);
            _uow.SaveChanges();
        }

        private int CoutTaskAssignedToUser(Guid userId)
        {
            var boards = _uow.Boards.GetAll();
            return boards.SelectMany(b => b.GetAllTasks())
                .Count(t => t.AssignedTo.Any(u => u.Id == userId));
        }
    }
}
