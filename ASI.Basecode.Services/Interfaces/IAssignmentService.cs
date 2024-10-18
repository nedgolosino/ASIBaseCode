using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAssignmentService
    {
        (bool, IEnumerable<Assignment>) GetAssignments();
        void AddAssignment(Assignment assignment);
        void EditAssignment(Assignment assignment);
        void DeleteAssignment(Assignment assignment);
    }
}
