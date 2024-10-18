using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            this._assignmentService = assignmentService;
        }

        public IActionResult Index()
        {
            (bool result, IEnumerable<Assignment> assignments) = _assignmentService.GetAssignments();
            if (result)
            {
                return View(assignments);
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var assignment = _assignmentService.GetAssignments().Item2.ToList().FirstOrDefault(x => x.Id == id);
            return View(assignment);
        }

        [HttpPost]
        public IActionResult Create(Assignment assignment)
        {
            try
            {
                _assignmentService.AddAssignment(assignment);
                return RedirectToAction("Index", "Assignment");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Assignment assignment)
        {
            _assignmentService.EditAssignment(assignment);
            TempData["SuccessMessage"] = "Assignment Edited Successfully";
            return RedirectToAction("Index", "Assignment");
        }

        public IActionResult Delete(int id)
        {
            var assignment = _assignmentService.GetAssignments().Item2.FirstOrDefault(x => x.Id == id);
            if (assignment != null)
            {
                _assignmentService.DeleteAssignment(assignment);
            }
            return RedirectToAction("Index", "Assignment");
        }
    }
}
