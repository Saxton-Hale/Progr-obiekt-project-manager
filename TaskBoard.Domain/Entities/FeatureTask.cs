using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoard.Domain.Entities
{
    internal class FeatureTask : TaskItem
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

        public FeatureTask(string title, DateTime dueDate, string moduleName) 
            : base(title, dueDate)
        {
            ModuleName = moduleName;
        }
    }
}
