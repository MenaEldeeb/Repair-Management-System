using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.BusinessLayer
{
    public class RepairOrderBL
    {
        private readonly MyContext DBCON;

        public RepairOrderBL(MyContext context)
        {
            DBCON = context;
        }

        // =========================
        // GET ALL REPAIR ORDERS
        // =========================
        public List<RepairOrder> GetAllRepairOrders()
        {
            return DBCON.RepairOrders
                        .Include(r => r.Device)
                        .Include(r => r.Technician)
                        .OrderByDescending(r => r.RepairOrderId)
                        .ToList();
        }

        // =========================
        // GET REPAIR ORDER BY ID
        // =========================
        public RepairOrder? GetByID(int id)
        {
            return DBCON.RepairOrders
                        .Include(r => r.Device)
                        .Include(r => r.Technician)
                        .FirstOrDefault(r => r.RepairOrderId == id);
        }

        // =========================
        // GET ALL DEVICES
        // =========================
        public List<Device> GetAllDevices()
        {
            return DBCON.Devices
                        .OrderBy(d => d.DeviceName)
                        .ToList();
        }

        // =========================
        // GET ALL TECHNICIANS
        // =========================
        public List<Technician> GetAllTechnicians()
        {
            return DBCON.Technicians
                        .OrderBy(t => t.Name)
                        .ToList();
        }

        // =========================
        // ADD REPAIR ORDER
        // =========================
        public void AddingRepairOrder(RepairOrder repairOrder)
        {
            DBCON.RepairOrders.Add(repairOrder);
            DBCON.SaveChanges();
        }

        // =========================
        // EDIT REPAIR ORDER
        // =========================
        public void EditRepairOrder(RepairOrder repairOrder)
        {
            DBCON.RepairOrders.Update(repairOrder);
            DBCON.SaveChanges();
        }

        // =========================
        // DELETE REPAIR ORDER
        // =========================
        public void Delete(int id)
        {
            var repairOrder = DBCON.RepairOrders.Find(id);

            if (repairOrder != null)
            {
                DBCON.RepairOrders.Remove(repairOrder);
                DBCON.SaveChanges();
            }
        }
    }
}