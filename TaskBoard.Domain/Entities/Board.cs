using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskBoard.Domain.Entities
{
    public class Board
    {
        public Guid Id { get; private set; }
        private string _name;

        public List<Column> Columns { get; private set; } = new();

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name error, is null or whitespace");

                _name = value;
            }
        }

        private Board() { }

        public Board(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void AddColumn(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Column cannot be null or whitespace");

            Columns.Add(new Column(name));
        }

        public void RemoveColumn(Guid columnId)
        {
            var col = Columns.SingleOrDefault(c => c.Id == columnId);
            if (col is null)
                throw new InvalidOperationException("Column not found");

            Columns.Remove(col);
        }

        public IEnumerable<TaskItem> GetAllTasks() =>
            Columns.SelectMany(c => c.Tasks);

        public void MoveTask(Guid taskId, Guid fromColumnId, Guid toColumnId)
        {
            var from = Columns.SingleOrDefault(c => c.Id == fromColumnId)
                ?? throw new InvalidOperationException("Source column not found");

            var to = Columns.SingleOrDefault(c => c.Id == toColumnId)
                ?? throw new InvalidOperationException("Target column not found");

            var task = from.Tasks.SingleOrDefault(t => t.Id == taskId)
                ?? throw new InvalidOperationException("Task not found in source column");

            from.RemoveTask(taskId);
            to.AddTask(task);
        }
    }
}
