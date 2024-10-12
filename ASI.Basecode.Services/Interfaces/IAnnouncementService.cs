using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IAnnouncementService
    {
        public (bool, IEnumerable<Announcement>) GetAnnouncements();
        public Announcement GetAnnouncementById(int id);
        public void AddAnnouncement(Announcement announcement);
        public void UpdateAnnouncement(Announcement announcement);
        public void DeleteAnnouncement(int id);
    }
}
