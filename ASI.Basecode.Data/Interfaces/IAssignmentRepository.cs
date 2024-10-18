using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IAssignmentRepository
    {
        IEnumerable<Assignment> ViewAssignments();
        void AddAssignment(Assignment assignment);
        void EditAssignment(Assignment assignment);
        void DeleteAssignment(Assignment assignment);
    }
}
