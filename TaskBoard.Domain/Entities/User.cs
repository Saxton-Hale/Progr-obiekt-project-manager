using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Domain.Entities
{
    internal class User : Person
    {
        private string _email;
        private UserRole _role;
        private List<TaskItem> _assignedTasks = new List<TaskItem>();

        public string Email
        {
            get => _email;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Email cannot be null or white space");
                    //Trzeba zrobic custom argument exception
                } else
                {
                    _email = value;
                }
            }
        }

        public UserRole Role
        {
            get => _role;
            set => _role = value;
        }

        public List<TaskItem> AssignedTasks => _assignedTasks;

        public User(string firstName, string lastName, string email, UserRole role) : base(firstName, lastName)
        {
            Email = email;
            Role = role;
        }

        public void AssignTask(TaskItem task)
        {
            if(task == null)
            {
                throw new ArgumentNullException(nameof(task));
                //custom do zrobienia
            }

            if(!_assignedTasks.Contains(task))
            {
                _assignedTasks.Add(task);
            }
        }

        public void UnAssignTask(TaskItem task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
                //custom do zrobienia
            }

            _assignedTasks.Remove(task);
        }

        public void ChangeUserRole(UserRole role)
        {
            if(role == Role)
            {
                throw new ArgumentException($"User juz ma role: {role}");
            } else
            {
                Role = role;
            }
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}, {Role}, {Email}";
        }
    }
}
