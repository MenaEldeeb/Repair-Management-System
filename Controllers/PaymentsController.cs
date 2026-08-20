
using FinalProject.BusinessLayer;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class PaymentsController : Controller
    {
        PaymentBL paymentBL = new PaymentBL();


        // =========================
        // INDEX
        // =========================

        public IActionResult Index()
        {
            var payments = paymentBL.GetAllPayments();

            return View(payments);
        }


        // =========================
        // DETAILS - GET
        // =========================

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = paymentBL.GetByID(id.Value);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }


        // =========================
        // CREATE - GET
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.RepairOrders =
                paymentBL.GetAllRepairOrders();

            return View();
        }


        // =========================
        // CREATE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    paymentBL.AddingPayment(payment);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(
                        "",
                        "Error while saving payment: " + ex.Message
                    );
                }
            }

            ViewBag.RepairOrders =
                paymentBL.GetAllRepairOrders();

            return View(payment);
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

            var payment = paymentBL.GetByID(id.Value);

            if (payment == null)
            {
                return NotFound();
            }

            ViewBag.RepairOrders =
                paymentBL.GetAllRepairOrders();

            return View(payment);
        }


        // =========================
        // EDIT - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    paymentBL.EditPayment(payment);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(
                        "",
                        "Error while updating payment: " + ex.Message
                    );
                }
            }

            ViewBag.RepairOrders =
                paymentBL.GetAllRepairOrders();

            return View(payment);
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

            var payment = paymentBL.GetByID(id.Value);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }


        // =========================
        // DELETE - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            paymentBL.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

