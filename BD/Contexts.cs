using BuildMaterials.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildMaterials.BD
{
    public static class StaticValues
    {
        public static string ConnectionString = "server=localhost;user=root;database=buildmaterials;password=546909021Var;";
    }

    public interface DBSetBase<T> where T : class
    {
        List<T> Search(string text);
        List<T> ToList();
        void Add(T obj);
        void Remove(T obj);
        void Remove(int id);
        public List<T> Select(string query);
    }

    public class ApplicationContext : DbContext
    {
        public MaterialsTable Materials { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Provider> Providers { get; set; } = null!;
        public DbSet<Trade> Trades { get; set; } = null!;
        public DbSet<TTN> TTNs { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public string[] AccessLevel => new string[4] { "Минимальный", "Низкий", "Средний", "Максимальный" };

        public ApplicationContext()
        {
            InitializeDatabase();
            
            if (Employees?.Count() == 0)
            {
                Employees.Add(new Employee("Имя", "Фамилия", "Отчество", "Директор", "+375259991234", 0));
                SaveChangesAsync();
            }
        }

        private void InitializeDatabase()
        {
            _createDatabase();
            _initTables();
        }

        private void _initTables()
        {
            Materials = new MaterialsTable();
        }

        private void _createDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS buildmaterials;", connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Query(string query)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    command.ExecuteNonQuery();
                }
                _connection.Close();
            }
        }

        public List<Employee> EmployeeSearch(string text)
        {
            List<Employee> employees = new List<Employee>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employees WHERE " +
                    $"CONCAT(name,' ', surname,' ', pathnetic,' ',position,' ',phonenumber) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            SurName = reader.GetString(2),
                            Pathnetic = reader.GetString(3),
                            Position = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            Password = reader.GetInt32(6),
                            AccessLevel = reader.GetInt32(7)
                        };
                        employees.Add(employee);
                    }
                }
                _connection.Close();
            }
            return employees;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(StaticValues.ConnectionString, new MySqlServerVersion(new Version(8, 0, 34)));
        }
    }

    public class MaterialsTable : DBSetBase<Material>
    {
        private MySqlConnection _connection;

        public MaterialsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("CREATE TABLE IF NOT EXISTS materials (ID int NOT NULL AUTO_INCREMENT, Name varchar(300) not null, Manufacturer varchar(100) not null, Price float NOT NULL,Count float NOT NULL,CountUnits varchar(20) ,EnterDate datetime NOT NULL,EnterCount float NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public BuildMaterials.Models.Material ElementAt(int id)
        {
            BuildMaterials.Models.Material material = new BuildMaterials.Models.Material();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        material.ID = reader.GetInt32(0);
                        material.Name = reader.GetString(1);
                        material.Manufacturer = reader.GetString(2);
                        material.Price = reader.GetFloat(3);
                        material.Count = reader.GetFloat(4);
                        material.CountUnits = reader.GetString(5);
                        material.EnterDate = reader.GetDateTime(6);
                        material.EnterCount = reader.GetFloat(7);
                    }
                }
                _connection.Close();
            }
            return material;
        }

        public void Add(Material obj)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO materials " +
                "(Name, Manufacturer, Price, Count, CountUnits, EnterDate, EnterCount) VALUES" +
                $"('{obj.Name}','{obj.Manufacturer}',{obj.Price},{obj.Count},'{obj.CountUnits}','{obj.EnterDate}',{obj.EnterCount}", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Remove(Material obj)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Material> Search(string text)
        {
            List<Material> materials = new List<Material>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE " +
                    $"CONCAT(name,' ', manufacturer,' ', price,' ',count,' ',enterdate,' ',entercount) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Material employee = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetFloat(3),
                            Count = reader.GetFloat(4),
                            CountUnits = reader.GetString(5),
                            EnterDate = reader.GetDateTime(6),
                            EnterCount = reader.GetFloat(7),
                        };
                        materials.Add(employee);
                    }
                }
                _connection.Close();
            }
            return materials;
        }

        public List<Material> Select(string query)
        {
            List<Material> materials = new List<Material>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Material material = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetFloat(3),
                            Count = reader.GetFloat(4),
                            CountUnits = reader.GetString(5),
                            EnterDate = reader.GetDateTime(6),
                            EnterCount = reader.GetFloat(7),
                        };
                        materials.Add(material);
                    }
                }
                _connection.Close();
            }
            return materials;
        }        

        public List<Material> ToList()
        {
            List<Material> materials = new List<Material>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Materials;", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Material employee = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetFloat(3),
                            Count = reader.GetFloat(4),
                            CountUnits = reader.GetString(5),
                            EnterDate = reader.GetDateTime(6),
                            EnterCount = reader.GetFloat(7),
                        };
                        materials.Add(employee);
                    }
                }
                _connection.Close();
            }
            return materials;
        }
    }
    public class EmployeesTable : DBSetBase<Employee>
    {
        private MySqlConnection _connection;

        public EmployeesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("CREATE TABLE IF NOT EXISTS materials (ID int NOT NULL AUTO_INCREMENT, Name varchar(300) not null, Manufacturer varchar(100) not null, Price float NOT NULL,Count float NOT NULL,CountUnits varchar(20) ,EnterDate datetime NOT NULL,EnterCount float NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public BuildMaterials.Models.Material ElementAt(int id)
        {
            BuildMaterials.Models.Material material = new BuildMaterials.Models.Material();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        material.ID = reader.GetInt32(0);
                        material.Name = reader.GetString(1);
                        material.Manufacturer = reader.GetString(2);
                        material.Price = reader.GetFloat(3);
                        material.Count = reader.GetFloat(4);
                        material.CountUnits = reader.GetString(5);
                        material.EnterDate = reader.GetDateTime(6);
                        material.EnterCount = reader.GetFloat(7);
                    }
                }
                _connection.Close();
            }
            return material;
        }

        public void Add(Material obj)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO materials " +
                "(Name, Manufacturer, Price, Count, CountUnits, EnterDate, EnterCount) VALUES" +
                $"('{obj.Name}','{obj.Manufacturer}',{obj.Price},{obj.Count},'{obj.CountUnits}','{obj.EnterDate}',{obj.EnterCount}", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Remove(Material obj)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Material> Search(string text)
        {
            List<Material> materials = new List<Material>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE " +
                    $"CONCAT(name,' ', manufacturer,' ', price,' ',count,' ',enterdate,' ',entercount) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Material employee = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetFloat(3),
                            Count = reader.GetFloat(4),
                            CountUnits = reader.GetString(5),
                            EnterDate = reader.GetDateTime(6),
                            EnterCount = reader.GetFloat(7),
                        };
                        materials.Add(employee);
                    }
                }
                _connection.Close();
            }
            return materials;
        }

        public List<Material> Select(string query)
        {
            List<Material> materials = new List<Material>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Material material = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetFloat(3),
                            Count = reader.GetFloat(4),
                            CountUnits = reader.GetString(5),
                            EnterDate = reader.GetDateTime(6),
                            EnterCount = reader.GetFloat(7),
                        };
                        materials.Add(material);
                    }
                }
                _connection.Close();
            }
            return materials;
        }

        public List<Material> ToList()
        {
            List<Material> materials = new List<Material>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Materials;", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Material employee = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetFloat(3),
                            Count = reader.GetFloat(4),
                            CountUnits = reader.GetString(5),
                            EnterDate = reader.GetDateTime(6),
                            EnterCount = reader.GetFloat(7),
                        };
                        materials.Add(employee);
                    }
                }
                _connection.Close();
            }
            return materials;
        }
    }
}