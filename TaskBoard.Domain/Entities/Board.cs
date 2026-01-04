using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskBoard.Domain.Entities
{
    internal class Board
    {
        private readonly Guid _id;
        private string _name;
        private readonly List<Column> _columns;

        public Guid Id => _id;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name error, is null or whitespace");
                    //custom exception later
                }
                else
                {
                    _name = value;
                }
            }
        }
        public IReadOnlyList<Column> Columns => _columns;

        public Board(string name)
        {
            _id = Guid.NewGuid();
            _name = name;
            _columns = new List<Column>();
        }

        public void AddColumn(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Column cannot be null or whitespace");
            //custom exception

            _columns.Add(new Column(name));
        }

        public void RemoveColumn(Guid columnId)
        {
            var col = _columns.SingleOrDefault(c => c.Id == columnId);
            if (col is null)
                throw new InvalidOperationException("Column not found");
            //custom exception

            _columns.Remove(col);
        }

        public IEnumerable<TaskItem> GetAllTasks() =>
            _columns.SelectMany(c => c.Tasks);

        public void MoveTask(Guid taskId, Guid fromColumnId, Guid toColumnId)
        {
            var from = _columns.SingleOrDefault(c => c.Id == fromColumnId)
                ?? throw new InvalidOperationException("Source column not found");

            var to = _columns.SingleOrDefault(c => c.Id == toColumnId)
                ?? throw new InvalidOperationException("Target column not found");

            var task = from.Tasks.SingleOrDefault(t => t.Id == taskId)
                ?? throw new InvalidOperationException("Task not found in source column");

            from.RemoveTask(taskId);
            to.AddTask(task);
        }
    }
}
