using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskBoard.Domain.Entities
{
    internal class Board
    {
        private Guid _id;
        private string _name;
        private List<Column> _columns;

        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

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

        public List<Column> Columns
        {
            get => _columns;
            set => _columns = value ?? new List<Column>();
        }

        public Board()
        {
            _id = Guid.NewGuid();
            _columns = new List<Column>();
        }

        public Board(string name) : this()
        {
            Name = name;
        }

        public void AddColumn(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Column name cannot be null or whitespace");
                // custom exception later
            }

            var column = new Column
            {
                Name = name
            };

            _columns.Add(column);
        }

        public void RemoveColumn(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Column name cannot be null or whitespace");
            }

            var column = _columns.FirstOrDefault(c => c.Name == name);
            if (column == null)
            {
                // dodac pozniej exception
                throw new InvalidOperationException($"Column '{name}' not found on board.");
            }

            _columns.Remove(column);
        }

        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();

            foreach (var column in _columns)
            {
                if (column.Tasks != null)
                {
                    tasks.AddRange(column.Tasks);
                }
            }

            return tasks;
        }
    }
}
