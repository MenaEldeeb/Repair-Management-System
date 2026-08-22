using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.BusinessLayer
{
    public class PaymentBL
    {
        private readonly MyContext db;

        public PaymentBL(MyContext context)
        {
            db = context;
        }

        // =========================
        // GET ALL PAYMENTS
        // =========================

        public List<Payment> GetAllPayments()
        {
            return db.Payments
                .Include(p => p.RepairOrder)
                    .ThenInclude(r => r.Device)
                        .ThenInclude(d => d.Customer)
                .ToList();
        }

        // =========================
        // GET PAYMENT BY ID
        // =========================

        public Payment? GetByID(int id)
        {
            return db.Payments
                .Include(p => p.RepairOrder)
                    .ThenInclude(r => r.Device)
                        .ThenInclude(d => d.Customer)
                .FirstOrDefault(p => p.PaymentId == id);
        }

        // =========================
        // GET REPAIR ORDER
        // =========================

        public RepairOrder? GetRepairOrderById(int id)
        {
            return db.RepairOrders
                .Include(r => r.Device)
                    .ThenInclude(d => d.Customer)
                .FirstOrDefault(r => r.RepairOrderId == id);
        }

        // =========================
        // GET PAYMENT BY REPAIR ORDER
        // =========================

        public Payment? GetPaymentByRepairOrderId(int repairOrderId)
        {
            return db.Payments
                .FirstOrDefault(p => p.RepairOrderId == repairOrderId);
        }

        // =========================
        // ADD PAYMENT
        // =========================

        public void AddingPayment(Payment payment)
        {
            var repairOrder = db.RepairOrders
                .FirstOrDefault(r =>
                    r.RepairOrderId == payment.RepairOrderId);

            if (repairOrder == null)
                return;

            payment.RepairOrder = null;

            db.Payments.Add(payment);

            db.SaveChanges();
        }

        // =========================
        // EDIT PAYMENT
        // =========================

        public void EditPayment(Payment payment)
        {
            var existingPayment = db.Payments
                .FirstOrDefault(p =>
                    p.PaymentId == payment.PaymentId);

            if (existingPayment == null)
                return;

            existingPayment.PaymentMethod =
                payment.PaymentMethod;

            db.SaveChanges();
        }

        // =========================
        // DELETE PAYMENT
        // =========================

        public void Delete(int id)
        {
            var payment = db.Payments.Find(id);

            if (payment != null)
            {
                db.Payments.Remove(payment);
                db.SaveChanges();
            }
        }
    }
}