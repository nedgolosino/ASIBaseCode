using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            this._announcementRepository = announcementRepository;
        }

        public void AddAnnouncement(Announcement announcement)
        {
            try
            {
                var newAnnouncement = new Announcement();
                newAnnouncement.Title = announcement.Title;
                newAnnouncement.Description = announcement.Description;
                newAnnouncement.Date = announcement.Date;

                _announcementRepository.addAnnouncement(newAnnouncement);
            }
            catch (Exception)
            {
                throw new InvalidDataException("Error adding announcement");
            }
        }

        public void UpdateAnnouncement(Announcement announcement)
        {
            var currentAnnouncement = _announcementRepository.GetAnnouncementById(announcement.announcementId);

            if (currentAnnouncement == null)
            {
                throw new InvalidDataException("Announcement not found!");
            }

            currentAnnouncement.Title = announcement.Title;
            currentAnnouncement.Description = announcement.Description;
            currentAnnouncement.Date = announcement.Date;

            _announcementRepository.updateAnnouncement(currentAnnouncement);
        }

        public void DeleteAnnouncement(int id)
        {
            var announcement = _announcementRepository.GetAnnouncementById(id);

            _announcementRepository.deleteAnnouncement(announcement);
        }

        public Announcement GetAnnouncementById(int id)
        {
            return _announcementRepository.GetAnnouncementById(id);
        }

        public (bool, IEnumerable<Announcement>) GetAnnouncements()
        { 
            var announcements = _announcementRepository.GetAnnouncements();

            if (announcements != null)
            { 
                return (true, announcements);
            }
            return (false, null);
        }
    }
}
