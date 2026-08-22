using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class CustomerPaymentController : Controller
    {
        private readonly MyContext _context;

        public CustomerPaymentController(MyContext context)
        {
            _context = context;
        }


        // =====================================================
        // CREATE PAYMENT - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Create(int repairOrderId)
        {
            if (repairOrderId <= 0)
                return NotFound();

            var repairOrder = await _context.RepairOrders
                .Include(r => r.Device)
                    .ThenInclude(d => d!.Customer)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r =>
                    r.RepairOrderId == repairOrderId);

            if (repairOrder == null)
                return NotFound();

            // لا يوجد سعر حتى الآن
            if (repairOrder.Cost <= 0)
            {
                TempData["Error"] =
                    "The repair cost has not been confirmed yet.";

                return RedirectToAction(
                    "TrackResult",
                    "CustomerRepair",
                    new
                    {
                        id = repairOrderId,
                        phone = repairOrder.Device?.Customer?.Phone
                    });
            }

            // لو الدفع موجود بالفعل
            if (repairOrder.Payments != null &&
                repairOrder.Payments.Any())
            {
                var existingPayment = repairOrder.Payments.First();

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = existingPayment.PaymentId,
                        repairOrderId = repairOrderId,
                        phone = repairOrder.Device?.Customer?.Phone
                    });
            }

            var payment = new Payment
            {
                RepairOrderId = repairOrder.RepairOrderId,
                Amount = repairOrder.Cost,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Cash"
            };

            return View(payment);
        }


        // =====================================================
        // CREATE PAYMENT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            var repairOrder = await _context.RepairOrders
                .Include(r => r.Payments)
                .Include(r => r.Device)
                    .ThenInclude(d => d!.Customer)
                .FirstOrDefaultAsync(r =>
                    r.RepairOrderId == payment.RepairOrderId);

            if (repairOrder == null)
                return NotFound();


            // منع الدفع أكثر من مرة
            if (repairOrder.Payments != null &&
                repairOrder.Payments.Any())
            {
                var existingPayment =
                    repairOrder.Payments.First();

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = existingPayment.PaymentId,
                        repairOrderId = repairOrder.RepairOrderId,
                        phone = repairOrder.Device?.Customer?.Phone
                    });
            }


            // السعر الحقيقي من RepairOrder
            payment.Amount = repairOrder.Cost;

            // التاريخ الحقيقي
            payment.PaymentDate = DateTime.Now;


            // السماح فقط بـ Cash أو Card
            if (payment.PaymentMethod != "Cash" &&
                payment.PaymentMethod != "Card")
            {
                ModelState.AddModelError(
                    "PaymentMethod",
                    "Please select Cash or Card.");
            }


            if (!ModelState.IsValid)
            {
                return View(payment);
            }


            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();


            TempData["Success"] =
                "Payment has been completed successfully.";


            return RedirectToAction(
                "TrackResult",
                "CustomerRepair",
                new
                {
                    id = repairOrder.RepairOrderId,
                    phone = repairOrder.Device?.Customer?.Phone
                });
        }


        // =====================================================
        // EDIT PAYMENT - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            int repairOrderId,
            string phone)
        {
            if (id <= 0 ||
                repairOrderId <= 0 ||
                string.IsNullOrWhiteSpace(phone))
            {
                return NotFound();
            }


            var payment = await _context.Payments
                .Include(p => p.RepairOrder)
                    .ThenInclude(r => r!.Device)
                        .ThenInclude(d => d!.Customer)
                .FirstOrDefaultAsync(p =>
                    p.PaymentId == id &&
                    p.RepairOrderId == repairOrderId &&
                    p.RepairOrder != null &&
                    p.RepairOrder.Device != null &&
                    p.RepairOrder.Device.Customer != null &&
                    p.RepairOrder.Device.Customer.Phone == phone);


            if (payment == null)
                return NotFound();


            return View(payment);
        }


        // =====================================================
        // EDIT PAYMENT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            int repairOrderId,
            string phone,
            Payment payment)
        {
            if (id != payment.PaymentId)
                return NotFound();


            var existingPayment = await _context.Payments
                .Include(p => p.RepairOrder)
                    .ThenInclude(r => r!.Device)
                        .ThenInclude(d => d!.Customer)
                .FirstOrDefaultAsync(p =>
                    p.PaymentId == id &&
                    p.RepairOrderId == repairOrderId &&
                    p.RepairOrder != null &&
                    p.RepairOrder.Device != null &&
                    p.RepairOrder.Device.Customer != null &&
                    p.RepairOrder.Device.Customer.Phone == phone);


            if (existingPayment == null)
                return NotFound();


            // العميل يقدر يغير طريقة الدفع فقط
            if (payment.PaymentMethod != "Cash" &&
                payment.PaymentMethod != "Card")
            {
                ModelState.AddModelError(
                    "PaymentMethod",
                    "Please select Cash or Card.");
            }


            if (!ModelState.IsValid)
            {
                payment.Amount =
                    existingPayment.Amount;

                payment.PaymentDate =
                    existingPayment.PaymentDate;

                payment.RepairOrderId =
                    existingPayment.RepairOrderId;

                return View(payment);
            }


            existingPayment.PaymentMethod =
                payment.PaymentMethod;


            await _context.SaveChangesAsync();


            TempData["Success"] =
                "Your payment has been updated successfully.";


            return RedirectToAction(
                "TrackResult",
                "CustomerRepair",
                new
                {
                    id = existingPayment.RepairOrderId,
                    phone = existingPayment
                        .RepairOrder?
                        .Device?
                        .Customer?
                        .Phone
                });
        }


        // =====================================================
        // PAYMENT DETAILS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Details(
            int id,
            int repairOrderId,
            string phone)
        {
            if (id <= 0 ||
                repairOrderId <= 0 ||
                string.IsNullOrWhiteSpace(phone))
            {
                return NotFound();
            }


            var payment = await _context.Payments
                .Include(p => p.RepairOrder)
                    .ThenInclude(r => r!.Device)
                        .ThenInclude(d => d!.Customer)
                .FirstOrDefaultAsync(p =>
                    p.PaymentId == id &&
                    p.RepairOrderId == repairOrderId &&
                    p.RepairOrder != null &&
                    p.RepairOrder.Device != null &&
                    p.RepairOrder.Device.Customer != null &&
                    p.RepairOrder.Device.Customer.Phone == phone);


            if (payment == null)
                return NotFound();


            return View(payment);
        }
    }
}