using Microsoft.AspNetCore.Mvc;
using FinalProject.BusinessLayer;
using FinalProject.Models;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [AdminOnly]
    public class CustomersController : Controller
    {
        private readonly CustomerBL customerBL;

        public CustomersController(CustomerBL customerBL)
        {
            this.customerBL = customerBL;
        }

        public IActionResult Index()
        {
            var customers = customerBL.GetAllCustomers();

            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerBL.AddCustomer(customer);

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public IActionResult Details(int id)
        {
            var customer = customerBL.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public IActionResult Edit(int id)
        {
            var customer = customerBL.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerBL.EditCustomer(customer);

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public IActionResult Delete(int id)
        {
            var customer = customerBL.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            customerBL.DeleteCustomer(id);

            return RedirectToAction("Index");
        }
    }
}