using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Filters;
namespace FinalProject.Controllers
{
    [AdminOnly]
    public class ReportsController : Controller
    {
        private readonly MyContext _context;

        public ReportsController(MyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // =========================
            // BASIC STATISTICS
            // =========================

            var totalCustomers =
                await _context.Customers.CountAsync();

            var totalDevices =
                await _context.Devices.CountAsync();

            var totalTechnicians =
                await _context.Technicians.CountAsync();

            var totalOrders =
                await _context.RepairOrders.CountAsync();


            // =========================
            // REPAIR STATUS
            // =========================

            var pending =
                await _context.RepairOrders
                    .CountAsync(x => x.Status == "Pending");

            var inProgress =
                await _context.RepairOrders
                    .CountAsync(x => x.Status == "In Progress");

            var ready =
                await _context.RepairOrders
                    .CountAsync(x => x.Status == "Ready for Collection");


            // =========================
            // PAYMENTS
            // =========================

            var totalPayments =
                await _context.Payments
                    .SumAsync(x => (decimal?)x.Amount) ?? 0;


            // =========================
            // REPAIR COST
            // =========================

            var totalRepairCost =
                await _context.RepairOrders
                    .SumAsync(x => (decimal?)x.Cost) ?? 0;


            // =========================
            // SEND DATA TO VIEW
            // =========================

            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalDevices = totalDevices;
            ViewBag.TotalTechnicians = totalTechnicians;
            ViewBag.TotalOrders = totalOrders;

            ViewBag.Pending = pending;
            ViewBag.InProgress = inProgress;
            ViewBag.Ready = ready;

            ViewBag.TotalPayments = totalPayments;
            ViewBag.TotalRepairCost = totalRepairCost;


            return View();
        }
    }
}
