using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using FinalProject.Filters;
namespace FinalProject.Controllers {
[AdminOnly]
public class RepairOrdersController : Controller
    {
        private readonly MyContext _context;

        public RepairOrdersController(MyContext context)
        {
            _context = context;
        }


        // =====================================================
        // INDEX
        // =====================================================

        public async Task<IActionResult> Index()
        {
            var repairOrders = await _context.RepairOrders
                .Include(r => r.Device)
                    .ThenInclude(d => d.Customer)
                .Include(r => r.Technician)
                .Include(r => r.Payments)
                .OrderByDescending(r => r.RepairOrderId)
                .ToListAsync();

            return View(repairOrders);
        }


        // =====================================================
        // DETAILS
        // =====================================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairOrder = await _context.RepairOrders
                .Include(r => r.Device)
                    .ThenInclude(d => d.Customer)
                .Include(r => r.Technician)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.RepairOrderId == id);

            if (repairOrder == null)
            {
                return NotFound();
            }

            return View(repairOrder);
        }


        // =====================================================
        // CREATE - GET
        // =====================================================

        public async Task<IActionResult> Create()
        {
            ViewBag.Devices = await _context.Devices
                .Include(d => d.Customer)
                .ToListAsync();

            ViewBag.Technicians = await _context.Technicians
                .ToListAsync();

            return View();
        }


        // =====================================================
        // CREATE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairOrder repairOrder)
        {
            if (ModelState.IsValid)
            {
                repairOrder.ReceiveDate = DateTime.Now;

                repairOrder.Status = "Pending";

                _context.RepairOrders.Add(repairOrder);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            // لو فيه Error نرجع البيانات للـ View

            ViewBag.Devices = await _context.Devices
                .Include(d => d.Customer)
                .ToListAsync();

            ViewBag.Technicians = await _context.Technicians
                .ToListAsync();

            return View(repairOrder);
        }


        // =====================================================
        // EDIT - GET
        // =====================================================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairOrder = await _context.RepairOrders
                .Include(r => r.Device)
                    .ThenInclude(d => d.Customer)
                .Include(r => r.Technician)
                .FirstOrDefaultAsync(r => r.RepairOrderId == id);

            if (repairOrder == null)
            {
                return NotFound();
            }


            ViewBag.Devices = await _context.Devices
                .Include(d => d.Customer)
                .ToListAsync();

            ViewBag.Technicians = await _context.Technicians
                .ToListAsync();


            return View(repairOrder);
        }


        // =====================================================
        // EDIT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            RepairOrder repairOrder)
        {
            if (id != repairOrder.RepairOrderId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repairOrder);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairOrderExists(repairOrder.RepairOrderId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }


            ViewBag.Devices = await _context.Devices
                .Include(d => d.Customer)
                .ToListAsync();

            ViewBag.Technicians = await _context.Technicians
                .ToListAsync();


            return View(repairOrder);
        }


        // =====================================================
        // DELETE - GET
        // =====================================================

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairOrder = await _context.RepairOrders
                .Include(r => r.Device)
                    .ThenInclude(d => d.Customer)
                .Include(r => r.Technician)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.RepairOrderId == id);

            if (repairOrder == null)
            {
                return NotFound();
            }

            return View(repairOrder);
        }


        // =====================================================
        // DELETE - POST
        // =====================================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repairOrder = await _context.RepairOrders
                .FirstOrDefaultAsync(r => r.RepairOrderId == id);

            if (repairOrder == null)
            {
                return NotFound();
            }


            // حذف الـ Payments المرتبطة بالأوردر أولاً

            var payments = await _context.Payments
                .Where(p => p.RepairOrderId == id)
                .ToListAsync();

            if (payments.Any())
            {
                _context.Payments.RemoveRange(payments);
            }


            _context.RepairOrders.Remove(repairOrder);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // CHECK EXISTS
        // =====================================================

        private bool RepairOrderExists(int id)
        {
            return _context.RepairOrders
                .Any(e => e.RepairOrderId == id);
        }
    }
}