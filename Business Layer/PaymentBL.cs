using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.BusinessLayer
{
    public class PaymentBL
    {
        private MyContext db = new MyContext();

        // =========================
        // GET ALL PAYMENTS
        // =========================

        public List<Payment> GetAllPayments()
        {
            return db.Payments
                     .Include(p => p.RepairOrder)
                     .ToList();
        }


        // =========================
        // GET PAYMENT BY ID
        // =========================

        public Payment GetByID(int id)
        {
            return db.Payments
                     .Include(p => p.RepairOrder)
                     .FirstOrDefault(p => p.PaymentId == id);
        }


        // =========================
        // GET REPAIR ORDERS
        // =========================

        public List<RepairOrder> GetAllRepairOrders()
        {
            return db.RepairOrders.ToList();
        }


        // =========================
        // ADD PAYMENT
        // =========================

        public void AddingPayment(Payment payment)
        {
            db.Payments.Add(payment);

            db.SaveChanges();
        }


        // =========================
        // EDIT PAYMENT
        // =========================

        public void EditPayment(Payment payment)
        {
            db.Payments.Update(payment);

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