using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;

namespace TaskBoard.Domain.Entities
{
    internal abstract class TaskItem
    {
        private Guid _id;
        private string _title;
        private string? _description;
        private TaskStatus _status;
        private TaskPriority _priority;
        private DateTime _createdAt;
        private DateTime _dueDate;
        private List<User>? _assignedTo;

        public Guid Id { get => _id; set => _id = value; }
        public string Title
        {
            get => _title;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tytul nie moze byc null or white space");
                } else
                {
                    _title = value;
                }
            }
        }
        public string Description { get => _description; set => _description = value; }
        public TaskStatus Status
        {
            get => _status;
            set => _status = value;
        }
        public TaskPriority Priority
        {
            get => _priority;
            set => _priority = value;
        }
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = System.DateTime.Now;
        }
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if(value < System.DateTime.Now)
                {
                    throw new ArgumentException("Due date nie moze byc w przeszlosci");
                } else
                {
                    _dueDate = value;
                }
            }
        }
        public List<User> AssignedTo
        {
            get => _assignedTo;
            set => _assignedTo = value;
        }

        public TaskItem()
        {
            _id = Guid.NewGuid();
        }

        public string GetSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Task id: {Id}");
            sb.AppendLine($"Tytul: {Title}");
            sb.AppendLine($"Opis: {Description}");
            sb.AppendLine($"Status: {Status}");
            sb.AppendLine($"Waznosc: {Priority}");
            sb.AppendLine($"Stworzone w: {CreatedAt}");
            sb.AppendLine($"Termin zrobienia {DueDate}");
            foreach(var user in AssignedTo)
            {
                sb.AppendLine(user.ToString());
            }

            return sb.ToString();
        }
    }
}
