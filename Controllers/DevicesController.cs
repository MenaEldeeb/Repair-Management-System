using FinalProject.BusinessLayer;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [AdminOnly]
    public class DevicesController : Controller
    {
        private readonly DeviceBL deviceBL;

        public DevicesController(DeviceBL deviceBL)
        {
            this.deviceBL = deviceBL;
        }

        // =========================
        // INDEX
        // =========================

        public IActionResult Index()
        {
            var devices = deviceBL.GetAllDevices();

            return View(devices);
        }

        // =========================
        // DETAILS - GET
        // =========================

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = deviceBL.GetByID(id.Value);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // =========================
        // CREATE - GET
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Customers = deviceBL.GetAllCustomers();

            return View();
        }

        // =========================
        // CREATE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Device device)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = deviceBL.GetAllCustomers();

                return View(device);
            }

            try
            {
                deviceBL.AddingDevice(device);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to save the device: " + ex.Message
                );

                ViewBag.Customers = deviceBL.GetAllCustomers();

                return View(device);
            }
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

            var device = deviceBL.GetByID(id.Value);

            if (device == null)
            {
                return NotFound();
            }

            ViewBag.Customers = deviceBL.GetAllCustomers();

            return View(device);
        }

        // =========================
        // EDIT - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = deviceBL.GetAllCustomers();

                return View(device);
            }

            try
            {
                deviceBL.EditDevice(device);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to update the device: " + ex.Message
                );

                ViewBag.Customers = deviceBL.GetAllCustomers();

                return View(device);
            }
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

            var device = deviceBL.GetByID(id.Value);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // =========================
        // DELETE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            deviceBL.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}