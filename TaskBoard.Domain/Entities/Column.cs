using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Domain.Entities
{
    internal class Column
    {
        private Guid _id;
        private string _name;
        private readonly List<TaskItem> _tasks = new();
        public Guid Id { get => _id; set => _id = value; }
        public string Name
        {
            get => _name;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Imie kolumny nie moze byc null albo white space");
                    //trzba zrobic custom exception
                } else
                {
                    _name = value;
                }
            }
        }
        public IReadOnlyList<TaskItem> Tasks => _tasks;

        public Column()
        {
            _id = Guid.NewGuid();
        }

        public Column(string name)
        {
            this.Id = _id;
            _name = name;
            _tasks = new List<TaskItem>();
        }


        public void AddTask(TaskItem task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
                //custom do zrobienia
            }
            _tasks.Add(task);
        }

        public void RemoveTask(Guid taskId)
        {
            var task = _tasks.SingleOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
                //custom do zrobienia
            }
            _tasks.Remove(task);
        }
    }
}
