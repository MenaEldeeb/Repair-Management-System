using FinalProject.Models;

namespace FinalProject.BusinessLayer
{
    public class CustomerBL
    {
        private readonly MyContext DBCON;

        public CustomerBL(MyContext context)
        {
            DBCON = context;
        }

        public List<Customer> GetAllCustomers()
        {
            return DBCON.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return DBCON.Customers
                .FirstOrDefault(c => c.CustomerId == id);
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