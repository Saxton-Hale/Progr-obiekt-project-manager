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
            set => _moduleName = value;
        }

        public FeatureTask(string moduleName) : base()
        {
            ModuleName = moduleName;
        }
    }
}
