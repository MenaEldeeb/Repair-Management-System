using FinalProject.Models;

namespace FinalProject.BusinessLayer
{
    public class CustomerBL
    {
        MyContext DBCON = new MyContext();

        public List<Customer> GetAllCustomers()
        {
            return DBCON.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return DBCON.Customers
                .Where(c => c.CustomerId == id)
                .FirstOrDefault();
        }

        public void AddCustomer(Customer customer)
        {
            DBCON.Customers.Add(customer);
            DBCON.SaveChanges();
        }

        public void EditCustomer(Customer customer)
        {
            DBCON.Customers.Update(customer);
            DBCON.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var customer = DBCON.Customers.Find(id);

            if (customer != null)
            {
                DBCON.Customers.Remove(customer);
                DBCON.SaveChanges();
            }
        }
    }
}
