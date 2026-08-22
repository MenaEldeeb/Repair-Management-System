using FinalProject.BusinessLayer;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Filters;
namespace FinalProject.Controllers
{
    [AdminOnly]
    public class PaymentsController : Controller
    {
        private readonly PaymentBL paymentBL;

        public PaymentsController()
        {
            paymentBL = new PaymentBL();
        }


        // =====================================================
        // ADMIN - INDEX
        // =====================================================

        public IActionResult Index()
        {
            var payments = paymentBL.GetAllPayments();

            return View(payments);
        }


        // =====================================================
        // CUSTOMER - CREATE PAYMENT - GET
        // =====================================================

        [HttpGet]
        public IActionResult Create(int repairOrderId)
        {
            if (repairOrderId <= 0)
                return NotFound();

            var repairOrder =
                paymentBL.GetRepairOrderById(repairOrderId);

            if (repairOrder == null)
                return NotFound();


     
            var existingPayment =
                paymentBL.GetPaymentByRepairOrderId(repairOrderId);

            if (existingPayment != null)
            {
   
                return RedirectToAction(
                    nameof(Edit),
                    new { id = existingPayment.PaymentId });
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
        // CUSTOMER - CREATE PAYMENT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Payment payment)
        {
 
            var repairOrder =
                paymentBL.GetRepairOrderById(
                    payment.RepairOrderId);


            if (repairOrder == null)
                return NotFound();



            var existingPayment =
                paymentBL.GetPaymentByRepairOrderId(
                    payment.RepairOrderId);


            if (existingPayment != null)
            {
                return RedirectToAction(
                    nameof(Edit),
                    new
                    {
                        id = existingPayment.PaymentId
                    });
            }


          
            payment.Amount = repairOrder.Cost;


            payment.PaymentDate = DateTime.Now;

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


            paymentBL.AddingPayment(payment);

           return RedirectToAction(
                "Track",
                "CustomerRepair");
        }


        // =====================================================
        // PAYMENT DETAILS
        // =====================================================

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();


            var payment =
                paymentBL.GetByID(id.Value);


            if (payment == null)
                return NotFound();


            return View(payment);
        }


        // =====================================================
        // CUSTOMER - EDIT PAYMENT - GET
        // =====================================================

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();


            var payment =
                paymentBL.GetByID(id.Value);


            if (payment == null)
                return NotFound();


            return View(payment);
        }


        // =====================================================
        // CUSTOMER - EDIT PAYMENT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Payment payment)
        {
            if (id != payment.PaymentId)
                return NotFound();



            var existingPayment =
                paymentBL.GetByID(id);


            if (existingPayment == null)
                return NotFound();


            payment.Amount =
                existingPayment.Amount;


            // نخلي RepairOrder الأصلي
            payment.RepairOrderId =
                existingPayment.RepairOrderId;



            payment.PaymentDate =
                existingPayment.PaymentDate;


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



            paymentBL.EditPayment(payment);


            // الرجوع للـ Track
            return RedirectToAction(
                "Track",
                "CustomerRepair");
        }


        // =====================================================
        // DELETE PAYMENT - GET
        // =====================================================

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();


            var payment =
                paymentBL.GetByID(id.Value);


            if (payment == null)
                return NotFound();


            return View(payment);
        }


        // =====================================================
        // DELETE PAYMENT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            paymentBL.Delete(id);

            return RedirectToAction(
                nameof(Index));
        }
    }
}