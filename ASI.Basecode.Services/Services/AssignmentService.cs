using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System.IO;

namespace ASI.Basecode.Services.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            this._assignmentRepository = assignmentRepository;
        }

        public (bool, IEnumerable<Assignment>) GetAssignments()
        {
            var assignments = _assignmentRepository.ViewAssignments();
            if (assignments != null)
            {
                return (true, assignments);
            }
            return (false, null);
        }

        public void AddAssignment(Assignment assignment)
        {
            try
            {
                var newAssignment = new Assignment
                {
                    Title = assignment.Title,
                    Description = assignment.Description,
                    DateTimeCreated = assignment.DateTimeCreated
                };
                _assignmentRepository.AddAssignment(newAssignment);
            }
            catch (Exception)
            {
                throw new InvalidDataException("Error Adding Assignment.");
            }
        }

        public void EditAssignment(Assignment assignment)
        {
            _assignmentRepository.EditAssignment(assignment);
        }

        public void DeleteAssignment(Assignment assignment)
        {
            _assignmentRepository.DeleteAssignment(assignment);
        }
    }
}
