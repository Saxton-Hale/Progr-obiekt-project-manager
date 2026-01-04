using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Enums;
using TaskStatus = TaskBoard.Domain.Enums.TaskStatus;
using TaskBoard.Domain.Exceptions;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Domain.Entities
{
    internal abstract class TaskItem : IIdentifiable, IAssignable
    {
        private Guid _id;
        private string _title;
        private string? _description;
        private TaskStatus _status;
        private TaskPriority _priority;
        private readonly DateTime _createdAt;
        private DateTime _dueDate;
        private readonly List<User> _assignedTo;

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
        public DateTime CreatedAt => _createdAt;
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
        public List<User> AssignedTo => _assignedTo;

        protected TaskItem(string title, DateTime dueDate)
        {
            _id = Guid.NewGuid();
            _assignedTo = new List<User>();

            _createdAt = DateTime.Now;
            SetTitle(title);
            SetDueDate(dueDate);
        }

        public void Rename(string newTitle) => SetTitle(newTitle);

        public void ChangeDueDate(DateTime newDueDate) => SetDueDate(newDueDate);

        private void SetDueDate(DateTime value)
        {
            if (value < _createdAt)
                throw new InvalidDeadlineException("Duedate cannot be earlier than CreatedAt");

            _dueDate = value;
        }


        private void SetTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidTaskException("Task title cannot be null or whitespace");

            _title = value.Trim();
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

        public void AssignTo(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (!_assignedTo.Contains(user))
                _assignedTo.Add(user);
        }

        public void UnAssign(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            _assignedTo.Remove(user);
        }

        public void UnAssignAll()
        {
            _assignedTo.Clear();
        }
    }
}
