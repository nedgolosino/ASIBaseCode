using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;

namespace ASI.Basecode.Data.Repositories
{
    public class AssignmentRepository : BaseRepository, IAssignmentRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public AssignmentRepository(AsiBasecodeDBContext dbContext, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Assignment> ViewAssignments()
        {
            return _dbContext.Assignments.ToList();
        }

        public void AddAssignment(Assignment assignment)
        {
            this.GetDbSet<Assignment>().Add(assignment);
            UnitOfWork.SaveChanges();
        }

        public void EditAssignment(Assignment assignment)
        {
            this.GetDbSet<Assignment>().Update(assignment);
            UnitOfWork.SaveChanges();
        }

        public void DeleteAssignment(Assignment assignment)
        {
            _dbContext.Assignments.Remove(assignment);
            UnitOfWork.SaveChanges();
        }
    }
}
