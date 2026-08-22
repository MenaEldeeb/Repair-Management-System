using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class CustomerRepairController : Controller
    {
        private readonly MyContext _context;

        public CustomerRepairController(MyContext context)
        {
            _context = context;
        }

        // =====================================================
        // REQUEST REPAIR - GET
        // =====================================================

        [HttpGet]
        public IActionResult RequestRepair()
        {
            return View();
        }


        // =====================================================
        // REQUEST REPAIR - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestRepair(
            string fname,
            string lname,
            string phone,
            string email,
            string address,
            string deviceName,
            string deviceType,
            string serialNumber,
            string problem)
        {
            if (string.IsNullOrWhiteSpace(fname) ||
                string.IsNullOrWhiteSpace(lname) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(deviceName) ||
                string.IsNullOrWhiteSpace(deviceType) ||
                string.IsNullOrWhiteSpace(problem))
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View();
            }

            // FIND CUSTOMER
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Phone == phone);

            // CREATE CUSTOMER
            if (customer == null)
            {
                customer = new Customer
                {
                    Fname = fname.Trim(),
                    Lname = lname.Trim(),
                    Phone = phone.Trim(),
                    Email = email?.Trim() ?? "",
                    Address = address?.Trim() ?? ""
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            // CREATE DEVICE
            var device = new Device
            {
                DeviceName = deviceName.Trim(),
                DeviceType = deviceType.Trim(),
                SerialNumber = serialNumber?.Trim() ?? "",
                Problem = problem.Trim(),
                CustomerId = customer.CustomerId
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            // FIND TECHNICIAN
            var technician = await _context.Technicians
                .OrderBy(x => x.TechnicianId)
                .FirstOrDefaultAsync();

            if (technician == null)
            {
                ViewBag.Error = "No technician is currently available.";
                return View();
            }

            // CREATE REPAIR ORDER
            var repairOrder = new RepairOrder
            {
                ReceiveDate = DateTime.Now,
                Status = "Pending",
                Description = problem.Trim(),
                Cost = 0,
                DeviceId = device.DeviceId,
                TechnicianId = technician.TechnicianId
            };

            _context.RepairOrders.Add(repairOrder);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Success),
                new { id = repairOrder.RepairOrderId });
        }


        // =====================================================
        // SUCCESS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var repairOrder = await _context.RepairOrders
                .Include(x => x.Device)
                .ThenInclude(x => x!.Customer)
                .Include(x => x.Technician)
                .FirstOrDefaultAsync(x => x.RepairOrderId == id);

            if (repairOrder == null)
            {
                return NotFound();
            }

            return View(repairOrder);
        }


        // =====================================================
        // TRACK - GET
        // =====================================================

        [HttpGet]
        public IActionResult Track()
        {
            return View();
        }


        // =====================================================
        // TRACK - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(
            int repairOrderId,
            string phone)
        {
            if (repairOrderId <= 0 ||
                string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.Error =
                    "Please enter the repair order number and phone number.";

                return View();
            }

            var order = await _context.RepairOrders

                .Include(x => x.Device)
                    .ThenInclude(x => x!.Customer)

                .Include(x => x.Technician)

                .Include(x => x.Payments)

                .FirstOrDefaultAsync(x =>
                    x.RepairOrderId == repairOrderId &&
                    x.Device != null &&
                    x.Device.Customer != null &&
                    x.Device.Customer.Phone == phone);

            if (order == null)
            {
                ViewBag.Error =
                    "No repair order was found. Please check your information.";

                return View();
            }

            return View("TrackResult", order);
        }


        // =====================================================
        // EDIT - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            string phone)
        {
            var order = await _context.RepairOrders

                .Include(x => x.Device)
                    .ThenInclude(x => x!.Customer)

                .FirstOrDefaultAsync(x =>
                    x.RepairOrderId == id &&
                    x.Device != null &&
                    x.Device.Customer != null &&
                    x.Device.Customer.Phone == phone);

            if (order == null)
            {
                TempData["Error"] =
                    "Repair order not found.";

                return RedirectToAction(nameof(Track));
            }

            // Customer can edit only while Pending
            if (order.Status != "Pending")
            {
                TempData["Error"] =
                    "You can only edit a pending repair request.";

                return RedirectToAction(
                    nameof(Track));
            }

            return View(order);
        }


        // =====================================================
        // EDIT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string phone,
            string deviceName,
            string deviceType,
            string serialNumber,
            string problem)
        {
            var order = await _context.RepairOrders

                .Include(x => x.Device)
                    .ThenInclude(x => x!.Customer)

                .FirstOrDefaultAsync(x =>
                    x.RepairOrderId == id &&
                    x.Device != null &&
                    x.Device.Customer != null &&
                    x.Device.Customer.Phone == phone);

            if (order == null)
            {
                TempData["Error"] =
                    "Repair order not found.";

                return RedirectToAction(nameof(Track));
            }

            if (order.Status != "Pending")
            {
                TempData["Error"] =
                    "You can only edit a pending repair request.";

                return RedirectToAction(nameof(Track));
            }

            // UPDATE DEVICE
            if (order.Device != null)
            {
                order.Device.DeviceName =
                    deviceName.Trim();

                order.Device.DeviceType =
                    deviceType.Trim();

                order.Device.SerialNumber =
                    serialNumber?.Trim() ?? "";

                order.Device.Problem =
                    problem.Trim();
            }

            // UPDATE REPAIR ORDER
            order.Description =
                problem.Trim();

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Your repair request has been updated successfully.";

            return RedirectToAction(
                nameof(TrackResult),
                new
                {
                    id = order.RepairOrderId,
                    phone = phone
                });
        }


        // =====================================================
        // TRACK RESULT
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> TrackResult(
            int id,
            string phone)
        {
            var order = await _context.RepairOrders

                .Include(x => x.Device)
                    .ThenInclude(x => x!.Customer)

                .Include(x => x.Technician)

                .Include(x => x.Payments)

                .FirstOrDefaultAsync(x =>
                    x.RepairOrderId == id &&
                    x.Device != null &&
                    x.Device.Customer != null &&
                    x.Device.Customer.Phone == phone);

            if (order == null)
            {
                TempData["Error"] =
                    "No repair order was found.";

                return RedirectToAction(nameof(Track));
            }

            return View(order);
        }


        // =====================================================
        // DELETE - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            string phone)
        {
            var order = await _context.RepairOrders

                .Include(x => x.Device)
                    .ThenInclude(x => x!.Customer)

                .FirstOrDefaultAsync(x =>
                    x.RepairOrderId == id &&
                    x.Device != null &&
                    x.Device.Customer != null &&
                    x.Device.Customer.Phone == phone);

            if (order == null)
            {
                TempData["Error"] =
                    "Repair order not found.";

                return RedirectToAction(nameof(Track));
            }

            if (order.Status != "Pending")
            {
                TempData["Error"] =
                    "You can only cancel a pending repair request.";

                return RedirectToAction(nameof(Track));
            }

            return View(order);
        }


        // =====================================================
        // DELETE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id,
            string phone)
        {
            var order = await _context.RepairOrders

                .Include(x => x.Device)
                    .ThenInclude(x => x!.Customer)

                .FirstOrDefaultAsync(x =>
                    x.RepairOrderId == id &&
                    x.Device != null &&
                    x.Device.Customer != null &&
                    x.Device.Customer.Phone == phone);

            if (order == null)
            {
                TempData["Error"] =
                    "Repair order not found.";

                return RedirectToAction(nameof(Track));
            }

            if (order.Status != "Pending")
            {
                TempData["Error"] =
                    "You can only cancel a pending repair request.";

                return RedirectToAction(nameof(Track));
            }

            // DELETE ORDER
            _context.RepairOrders.Remove(order);

            // DELETE DEVICE
            if (order.Device != null)
            {
                _context.Devices.Remove(order.Device);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Your repair request has been cancelled successfully.";

            return RedirectToAction(nameof(Track));
        }
    }
}