using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }


        // ==========================================
        // CUSTOMER HOME PAGE
        // ==========================================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        // ==========================================
        // ADMIN LOGIN - GET
        // ==========================================

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }


        // ==========================================
        // ADMIN LOGIN - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(string username, string password)
        {
            if (username == "admin" && password == "123456")
            {
                HttpContext.Session.SetString("IsAdmin", "true");

                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Error = "Invalid username or password.";

            return View();
        }


        // ==========================================
        // ADMIN DASHBOARD
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // منع الدخول بدون Login
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction(nameof(AdminLogin));
            }


            // ==========================================
            // STATISTICS
            // ==========================================

            var totalCustomers =
                await _context.Customers.CountAsync();

            var totalDevices =
                await _context.Devices.CountAsync();

            var totalTechnicians =
                await _context.Technicians.CountAsync();

            var totalRepairOrders =
                await _context.RepairOrders.CountAsync();


            var pendingRepairs =
                await _context.RepairOrders
                    .CountAsync(x => x.Status == "Pending");


            var inProgressRepairs =
                await _context.RepairOrders
                    .CountAsync(x => x.Status == "In Progress");


            var completedRepairs =
                await _context.RepairOrders
                    .CountAsync(x =>
                        x.Status == "Completed" ||
                        x.Status == "Ready for Collection");


            var totalPayments =
                await _context.Payments
                    .SumAsync(x => (decimal?)x.Amount) ?? 0;


            // ==========================================
            // VIEWBAG
            // ==========================================

            ViewBag.TotalCustomers =
                totalCustomers;

            ViewBag.TotalDevices =
                totalDevices;

            ViewBag.TotalTechnicians =
                totalTechnicians;

            ViewBag.TotalRepairOrders =
                totalRepairOrders;

            ViewBag.PendingRepairs =
                pendingRepairs;

            ViewBag.InProgressRepairs =
                inProgressRepairs;

            ViewBag.CompletedRepairs =
                completedRepairs;

            ViewBag.TotalPayments =
                totalPayments;


            // ==========================================
            // RECENT REPAIR ORDERS
            // ==========================================

            var recentOrders =
                await _context.RepairOrders
                    .Include(x => x.Device)
                    .Include(x => x.Technician)
                    .OrderByDescending(x => x.ReceiveDate)
                    .Take(5)
                    .ToListAsync();


         
            return View(
                "~/Views/Dashboard/Index.cshtml",
                recentOrders
            );
        }


        // ==========================================
        // ADMIN LOGOUT
        // ==========================================

        [HttpGet]
        public IActionResult AdminLogout()
        {
            HttpContext.Session.Remove("IsAdmin");

            return RedirectToAction(nameof(Index));
        }
    }
}