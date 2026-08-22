using FinalProject.Models;

namespace FinalProject.BusinessLayer
{
    public class TechnicianBL
    {
        private readonly MyContext DBCON;

        public TechnicianBL(MyContext context)
        {
            DBCON = context;
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
        // GET TECHNICIAN BY ID
        // =========================
        public Technician? GetByID(int id)
        {
            return DBCON.Technicians
                        .FirstOrDefault(t => t.TechnicianId == id);
        }

        // =========================
        // ADD TECHNICIAN
        // =========================
        public void AddingTechnician(Technician technician)
        {
            DBCON.Technicians.Add(technician);
            DBCON.SaveChanges();
        }

        // =========================
        // EDIT TECHNICIAN
        // =========================
        public void EditTechnician(Technician technician)
        {
            DBCON.Technicians.Update(technician);
            DBCON.SaveChanges();
        }

        // =========================
        // DELETE TECHNICIAN
        // =========================
        public void Delete(int id)
        {
            var technician = DBCON.Technicians.Find(id);

            if (technician != null)
            {
                DBCON.Technicians.Remove(technician);
                DBCON.SaveChanges();
            }
        }
    }
}