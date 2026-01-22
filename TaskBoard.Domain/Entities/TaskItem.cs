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
    public abstract class TaskItem : IIdentifiable, IAssignable, IComparable<TaskItem>, ICloneable
    {
        private Guid _id;
        private string _title;
        private string? _description;
        private TaskStatus _status;
        private TaskPriority _priority;
        private readonly DateTime _createdAt;
        private DateTime? _dueDate;
        private List<User> _assignedTo;

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
        public DateTime? DueDate => _dueDate;
        public List<User> AssignedTo => _assignedTo;

        protected TaskItem(string title, DateTime? dueDate = null)
        {
            _id = Guid.NewGuid();
            _assignedTo = new List<User>();

            _createdAt = DateTime.Now;
            SetTitle(title);
            SetDueDate(dueDate);
        }

        public void Rename(string newTitle) => SetTitle(newTitle);

        public void ChangeDueDate(DateTime? newDueDate) => SetDueDate(newDueDate);

        private void SetDueDate(DateTime? value)
        {
            if(value.HasValue)
            {
                if (value.Value < _createdAt)
                    throw new InvalidDeadlineException("Duedate cannot be earlier than CreatedAt");
            }
            
            _dueDate = value;
        }


        private void SetTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidTaskException("Task title cannot be null or whitespace");

            _title = value.Trim();
        }

        public virtual string GetSummary()
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

        public int CompareTo(TaskItem? other)
        {
            if (other is null) return 1;

            var thisDueDate = this.DueDate;
            var otherDueDate = other.DueDate;

            if (thisDueDate is null && otherDueDate is null) return 0;
            if (thisDueDate is null) return 1;
            if (otherDueDate is null) return -1;

            return DateTime.Compare(thisDueDate.Value, otherDueDate.Value);
        }

        public object Clone()
        {
            var copy = (TaskItem)this.MemberwiseClone();

            copy._assignedTo = new List<User>(this._assignedTo);

            return copy;
        }
    }
}
