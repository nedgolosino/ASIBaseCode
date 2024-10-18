using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IAnnouncementRepository
    {
        IEnumerable<Announcement> GetAnnouncements();
        public Announcement GetAnnouncementById(int id);
        public void addAnnouncement(Announcement announcement);
        public void updateAnnouncement(Announcement announcement);
        public void deleteAnnouncement(Announcement announcement);

    }
}
