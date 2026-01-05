using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Comparer
{
    internal class TaskPriorityComparer : IComparer<TaskItem>
    {
        public int Compare(TaskItem? x, TaskItem? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return 1;
            if (y is null) return -1;

            int cmp = y.Priority.CompareTo(x.Priority);
            if (cmp != 0) return cmp;

            cmp = CompareDueDateNullLast(x.DueDate, y.DueDate);
            if (cmp != 0) return cmp;

            cmp = x.CreatedAt.CompareTo(y.CreatedAt);
            if (cmp != 0) return cmp;

            return x.Id.CompareTo(y.Id);
        }

        private static int CompareDueDateNullLast(DateTime? a, DateTime? b)
        {
            if (a is null && b is null) return 0;
            if (a is null) return 1;
            if (b is null) return -1;

            return DateTime.Compare(a.Value, b.Value);
        }
    }
}
