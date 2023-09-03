using BuildMaterials.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.IO;
using System.Linq;

namespace BuildMaterials.BD
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Material> Materials { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Provider> Providers { get; set; } = null!;
        public DbSet<Trade> Trades { get; set; } = null!;
        public DbSet<TTN> TTNs { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public string[] AccessLevel => new string[4] { "Минимальный", "Низкий", "Средний", "Максимальный" };

        public readonly string ConnectionString = "server=localhost;user=root;database=buildmaterials;password=5469090;";

        public ApplicationContext()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS buildmaterials", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            if (Employees?.Count() == 0)
            {
                Employees.Add(new Employee("Имя", "Фамилия", "Отчество", "Директор", "+375259991234", 0));
                SaveChangesAsync();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 34)));
        }
    }
}