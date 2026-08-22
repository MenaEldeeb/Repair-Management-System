using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public class DashboardViewModel
    {
        // Statistics
        public int CustomersCount { get; set; }

        public int DevicesCount { get; set; }

        public int RepairOrdersCount { get; set; }

        public int TechniciansCount { get; set; }

        public int PendingOrdersCount { get; set; }

        public int InProgressOrdersCount { get; set; }

        public int CompletedOrdersCount { get; set; }

        public decimal TotalPayments { get; set; }

        // Recent repair orders
        public List<RecentRepairOrderViewModel> RecentRepairOrders { get; set; }
            = new List<RecentRepairOrderViewModel>();
    }


    public class RecentRepairOrderViewModel
    {
        public int RepairOrderId { get; set; }

        public string CustomerName { get; set; } = "";

        public string DeviceName { get; set; } = "";

        public string DeviceType { get; set; } = "";

        public string Problem { get; set; } = "";

        public string TechnicianName { get; set; } = "";

        public string Status { get; set; } = "";

        public decimal Cost { get; set; }

        public DateTime ReceiveDate { get; set; }
    }
}