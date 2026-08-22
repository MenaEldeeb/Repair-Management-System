



using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<Payment> Payments { get; set; }
}