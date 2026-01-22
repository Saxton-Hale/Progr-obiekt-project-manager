using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.Domain.Entities
{
    internal class BugTask : TaskItem
    {
        private TaskPriority _severity;
        private string? _stepsToReproduce;
        private string? _expectedResult;
        private string? _actualResult;

        public TaskPriority Severity
        {
            get => _severity;
            set => _severity = value;
        }
        public string? StepsToReproduce
        {
            get => _stepsToReproduce;
            set => _stepsToReproduce = value;
        }
        public string? ExpectedResult
        {
            get => _expectedResult;
            set => _expectedResult = value;
        }
        public string? ActualResult
        {
            get => _actualResult;
            set => _actualResult = value;
        }

        public BugTask(string title, DateTime dueDate) : base(title, dueDate)
        {
        }

        public BugTask(
            Guid id,
            string title,
            string? description,
            TaskStatus status,
            TaskPriority priority,
            DateTime? dueDate
        ) : base(title)
        {
            Id = id;
            Description = description;
            Status = status;
            Priority = priority;

            if (dueDate.HasValue)
                ChangeDueDate(dueDate);
        }


        public override string GetSummary()
        {
            return base.GetSummary() + "\nSeverity: " + Severity;
        }
    }
}
