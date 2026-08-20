




using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models;

public class MyContext : DbContext
{
    string connectionString =
       "Server=.\\SQLEXPRESS03;Database=FinalProject;Trusted_Connection=True;TrustServerCertificate=True;";

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Device> Devices { get; set; }

    public DbSet<Technician> Technicians { get; set; }

    public DbSet<RepairOrder> RepairOrders { get; set; }

    public DbSet<Payment> Payments { get; set; }
}