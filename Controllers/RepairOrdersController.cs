using FinalProject.BusinessLayer;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class RepairOrdersController : Controller
    {
        RepairOrderBL repairOrderBL = new RepairOrderBL();


        // =========================
        // INDEX
        // =========================

        public IActionResult Index()
        {
            var repairOrders = repairOrderBL.GetAllRepairOrders();

            return View(repairOrders);
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

            var repairOrder = repairOrderBL.GetByID(id.Value);

            if (repairOrder == null)
            {
                return NotFound();
            }

            return View(repairOrder);
        }


        // =========================
        // CREATE - GET
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Devices = repairOrderBL.GetAllDevices();
            ViewBag.Technicians = repairOrderBL.GetAllTechnicians();

            return View();
        }


        // =========================
        // CREATE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RepairOrder repairOrder)
        {
            if (ModelState.IsValid)
            {
                repairOrderBL.AddingRepairOrder(repairOrder);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Devices = repairOrderBL.GetAllDevices();
            ViewBag.Technicians = repairOrderBL.GetAllTechnicians();

            return View(repairOrder);
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

            var repairOrder = repairOrderBL.GetByID(id.Value);

            if (repairOrder == null)
            {
                return NotFound();
            }

            ViewBag.Devices = repairOrderBL.GetAllDevices();
            ViewBag.Technicians = repairOrderBL.GetAllTechnicians();

            return View(repairOrder);
        }


        // =========================
        // EDIT - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RepairOrder repairOrder)
        {
            if (id != repairOrder.RepairOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                repairOrderBL.EditRepairOrder(repairOrder);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Devices = repairOrderBL.GetAllDevices();
            ViewBag.Technicians = repairOrderBL.GetAllTechnicians();

            return View(repairOrder);
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

            var repairOrder = repairOrderBL.GetByID(id.Value);

            if (repairOrder == null)
            {
                return NotFound();
            }

            return View(repairOrder);
        }


        // =========================
        // DELETE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repairOrderBL.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}