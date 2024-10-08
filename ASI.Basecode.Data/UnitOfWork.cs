using System;
using ASI.Basecode.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Data
{

    /// <summary>
    /// Unit of Work Implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
   
        /// </summary>
        public DbContext Database { get; private set; }

        /// <summary>
   
     
     
        public UnitOfWork(AsiBasecodeDBContext serviceContext)
        {
            Database = serviceContext;
        }

        /// <summary>
   
        /// </summary>
        public void SaveChanges()
        {
            Database.SaveChanges();
        }

        /// <summary>
   
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
