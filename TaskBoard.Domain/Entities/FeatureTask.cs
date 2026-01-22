using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.Domain.Entities
{
    public class FeatureTask : TaskItem
    {
        private string _moduleName;

        public string ModuleName
        {
            get => _moduleName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ModuleName cannot be empty.");
                _moduleName = value.Trim();
            }
        }

        public FeatureTask(string title, DateTime? dueDate, string moduleName) 
            : base(title, dueDate)
        {
            ModuleName = moduleName;
        }

        public FeatureTask(
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
            return base.GetSummary() + "\nModule name: " + ModuleName;
        }

    }
}
