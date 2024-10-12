using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService announcementService)
        {
            this._announcementService = announcementService;
        }

        public IActionResult Index()
        {
            (bool result, IEnumerable<Announcement> announcements) = _announcementService.GetAnnouncements();
            if (result)
            {
                return View(announcements);
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Announcement announcement)
        {
            _announcementService.AddAnnouncement(announcement);
            return RedirectToAction("Index", "Announcement");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var announcement = _announcementService.GetAnnouncementById(id);

            if (announcement == null)
            {
                return NotFound();
            }

            var editAnnouncement = new Announcement
            {
                announcementId = id,
                Title = announcement.Title,
                Description = announcement.Description
            };

            return View(editAnnouncement);
        }

        [HttpPost]
        public IActionResult Edit(Announcement announcement)
        {
            _announcementService.UpdateAnnouncement(announcement);

            return RedirectToAction("Index", "Announcement");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _announcementService.DeleteAnnouncement(id);

            return RedirectToAction("Index");
        }

    }
}
