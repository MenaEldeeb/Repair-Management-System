using FinalProject.BusinessLayer;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class TechniciansController : Controller
    {
        TechnicianBL technicianBL = new TechnicianBL();

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            var technicians = technicianBL.GetAllTechnicians();

            return View(technicians);
        }

        // =========================
        // DETAILS
        // =========================
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = technicianBL.GetByID(id.Value);

            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // =========================
        // CREATE - GET
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Technician technician)
        {
            if (!ModelState.IsValid)
            {
                return View(technician);
            }

            technicianBL.AddingTechnician(technician);

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT - GET
        // =========================
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = technicianBL.GetByID(id.Value);

            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // =========================
        // EDIT - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Technician technician)
        {
            if (id != technician.TechnicianId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(technician);
            }

            technicianBL.EditTechnician(technician);

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE - GET
        // =========================
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = technicianBL.GetByID(id.Value);

            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // =========================
        // DELETE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            technicianBL.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}