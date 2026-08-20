

using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.BusinessLayer
{
    public class DeviceBL
    {
        MyContext DBCON = new MyContext();


        public List<Device> GetAllDevices()
        {
            var res = DBCON.Devices
                           .Include(d => d.Customer)
                           .OrderBy(d => d.DeviceId)
                           .ToList();

            return res;
        }


        public Device GetByID(int id)
        {
            var res = DBCON.Devices
                           .Include(d => d.Customer)
                           .FirstOrDefault(d => d.DeviceId == id);

            return res;
        }


        public void AddingDevice(Device d)
        {
            DBCON.Devices.Add(d);
            DBCON.SaveChanges();
        }


        public void EditDevice(Device d)
        {
            DBCON.Devices.Update(d);
            DBCON.SaveChanges();
        }


        public void Delete(int id)
        {
            var device = DBCON.Devices.Find(id);

            if (device != null)
            {
                DBCON.Devices.Remove(device);
                DBCON.SaveChanges();
            }
        }


        public List<Customer> GetAllCustomers()
        {
            var res = DBCON.Customers
                           .OrderBy(c => c.Fname)
                           .ToList();

            return res;
        }
    }
}