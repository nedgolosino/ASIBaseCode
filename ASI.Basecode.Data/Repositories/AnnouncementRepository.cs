using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class AnnouncementRepository : BaseRepository, IAnnouncementRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public AnnouncementRepository(AsiBasecodeDBContext dbContext, IUnitOfWork unitOfWork) : base(unitOfWork)
        { 
            this._dbContext = dbContext;
        }
        public IEnumerable<Announcement> GetAnnouncements()
        {
            return _dbContext.Announcements.ToList();
        }
        public Announcement GetAnnouncementById(int id)
        {
            var announcement = _dbContext.Announcements.FirstOrDefault(u => u.announcementId == id);

            if (announcement == null)
            {
                throw new Exception("Announcement not found!");
            }

            return announcement;
        }
        public void addAnnouncement(Announcement announcement)
        {
            this.GetDbSet<Announcement>().Add(announcement);
            UnitOfWork.SaveChanges();
        }

        public void deleteAnnouncement(Announcement announcement)
        {
            _dbContext.Remove(announcement);
            UnitOfWork.SaveChanges();
        }

        public void updateAnnouncement(Announcement announcement)
        {
            var updateAnnouncement = _dbContext.Announcements.FirstOrDefault(u => u.announcementId == announcement.announcementId);

            if (updateAnnouncement == null)
            {
                throw new Exception("Announcement not found!");
            }

            updateAnnouncement = announcement;
            UnitOfWork.SaveChanges();
        }
    }
}
