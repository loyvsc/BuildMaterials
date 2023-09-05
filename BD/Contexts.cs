using BuildMaterials.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace BuildMaterials.BD
{
    public static class StaticValues
    {
        public const string ConnectionString = "server=localhost;user=root;database=buildmaterials;password=546909021Var;";
    }

    public interface IDBSetBase<T> where T : class
    {
        List<T> Search(string text);
        List<T> ToList();
        void Add(T obj);
        void Remove(T obj);
        void Remove(int id);
        public List<T> Select(string query);
    }

    public class ApplicationContext
    {
        public MaterialsTable Materials { get; set; } = null!;
        public EmployeesTable Employees { get; set; } = null!;
        public CustomersTable Customers { get; set; } = null!;
        public ProvidersTable Providers { get; set; } = null!;
        public TradesTable Trades { get; set; } = null!;
        public TTNSTable TTNs { get; set; } = null!;
        public AccountsTable Accounts { get; set; } = null!;
        public ContractsTable Contracts { get; set; } = null!;
        public string[] AccessLevel => new string[4] { "Минимальный", "Низкий", "Средний", "Максимальный" };

        public ApplicationContext()
        {
            InitializeDatabase();
            if (Employees?.Count() == 0)
            {
                Employees.Add(new Employee("Имя", "Фамилия", "Отчество", "Директор", "+375259991234", 0));
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
            Employees = new EmployeesTable();
            Customers = new CustomersTable();
            Providers = new ProvidersTable();
            Trades = new TradesTable();
            TTNs = new TTNSTable();
            Accounts = new AccountsTable();
            Contracts = new ContractsTable();
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

    }

    public class MaterialsTable : IDBSetBase<Material>
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
                $"('{obj.Name}','{obj.Manufacturer}',{obj.Price},{obj.Count},'{obj.CountUnits}','{obj.EnterDate.ToShortDateString()}',{obj.EnterCount});", _connection))
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
    public class EmployeesTable : IDBSetBase<Employee>
    {
        private MySqlConnection _connection;

        public EmployeesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("CREATE TABLE IF NOT EXISTS employees (ID int NOT NULL AUTO_INCREMENT, Name varchar(50), Surname varchar(50), Pathnetic varchar(70), Position varchar(100), PhoneNumber varchar(14),Password int NOT NULL, AccessLevel int NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM employees;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.Employee ElementAt(int id)
        {
            Employee employee = new Employee();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employee WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        employee.ID = reader.GetInt32(0);
                        employee.Name = reader.GetString(1);
                        employee.SurName = reader.GetString(2);
                        employee.Pathnetic = reader.GetString(3);
                        employee.Position = reader.GetString(4);
                        employee.PhoneNumber = reader.GetString(5);
                        employee.Password = reader.GetInt32(6);
                        employee.AccessLevel = reader.GetInt32(7);
                    }
                }
                _connection.Close();
            }
            return employee;
        }

        public void Add(Employee obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO employees " +
                "(Name, Surname, pathnetic, position, phonenumber, password, AccessLevel) VALUES" +
                $"('{obj.Name}','{obj.SurName}'," +
                $"'{obj.Pathnetic}','{obj.Position}','{obj.PhoneNumber}',{obj.Password},{obj.AccessLevel});", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(Employee obj)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM employees WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM employees WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Employee> Search(string text)
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

        public List<Employee> ToList()
        {
            return Select("SELECT * FROM employees;");
        }

        public List<Employee> Select(string query)
        {
            List<Employee> employees = new List<Employee>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
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
    }
    public class CustomersTable : IDBSetBase<Customer>
    {
        private MySqlConnection _connection;

        public CustomersTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS customers " +
                "(ID int NOT NULL AUTO_INCREMENT, CompanyName varchar(100), Adress varchar(50)," +
                "CompanyPerson varchar(70), CompanyPhone varchar(20), Bank varchar(100)," +
                "BankProp varchar(100) NOT NULL, UNP varchar(100) NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM customers;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.Customer ElementAt(int id)
        {
            Customer obj = new Customer();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM customers WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        obj.ID = reader.GetInt32(0);
                        obj.CompanyName = reader.GetString(1);
                        obj.Adress = reader.GetString(2);
                        obj.CompanyPerson = reader.GetString(3);
                        obj.CompanyPhone = reader.GetString(4);
                        obj.Bank = reader.GetString(5);
                        obj.BankProp = reader.GetString(6);
                        obj.UNP = reader.GetString(7);
                    }
                }
                _connection.Close();
            }
            return obj;
        }

        public void Add(Customer obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO customers " +
                "(companyname, adress, CompanyPerson, CompanyPhone, Bank, BankProp, UNP) VALUES" +
                $"('{obj.CompanyName}','{obj.Adress}'," +
                $"'{obj.CompanyPerson}','{obj.CompanyPhone}','{obj.Bank}','{obj.BankProp}','{obj.UNP}');", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(Customer obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM customers WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Customer> Search(string text)
        {
            List<Customer> customers = new List<Customer>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employees WHERE " +
                    $"CONCAT(name,' ', surname,' ', pathnetic,' ',position,' ',phonenumber) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Customer customer = new Customer()
                        {
                            ID = reader.GetInt32(0),
                            CompanyName = reader.GetString(1),
                            Adress = reader.GetString(2),
                            CompanyPerson = reader.GetString(3),
                            CompanyPhone = reader.GetString(4),
                            Bank = reader.GetString(5),
                            BankProp = reader.GetString(6),
                            UNP = reader.GetString(7)
                        };
                        customers.Add(customer);
                    }
                }
                _connection.Close();
            }
            return customers;
        }

        public List<Customer> ToList()
        {
            return Select("SELECT * FROM customers;");
        }

        public List<Customer> Select(string query)
        {
            List<Customer> customers = new List<Customer>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Customer customer = new Customer()
                        {
                            ID = reader.GetInt32(0),
                            CompanyName = reader.GetString(1),
                            Adress = reader.GetString(2),
                            CompanyPerson = reader.GetString(3),
                            CompanyPhone = reader.GetString(4),
                            Bank = reader.GetString(5),
                            BankProp = reader.GetString(6),
                            UNP = reader.GetString(7)
                        };
                        customers.Add(customer);
                    }
                }
                _connection.Close();
            }
            return customers;
        }
    }
    public class ProvidersTable : IDBSetBase<Provider>
    {
        private MySqlConnection _connection;

        public ProvidersTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS providers " +
                "(ID int NOT NULL AUTO_INCREMENT, CompanyName varchar(100), Adress varchar(50)," +
                "CompanyPerson varchar(70), CompanyPhone varchar(20), Bank varchar(100)," +
                "BankProp varchar(100) NOT NULL, UNP varchar(100) NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM providers;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.Provider ElementAt(int id)
        {
            Provider obj = new Provider();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM provider WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        obj.ID = reader.GetInt32(0);
                        obj.CompanyName = reader.GetString(1);
                        obj.Adress = reader.GetString(2);
                        obj.CompanyPerson = reader.GetString(3);
                        obj.CompanyPhone = reader.GetString(4);
                        obj.Bank = reader.GetString(5);
                        obj.BankProp = reader.GetString(6);
                        obj.UNP = reader.GetString(7);
                    }
                }
                _connection.Close();
            }
            return obj;
        }

        public void Add(Provider obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO providers " +
                "(companyname, adress, CompanyPerson, CompanyPhone, Bank, BankProp, UNP) VALUES" +
                $"('{obj.CompanyName}','{obj.Adress}'," +
                $"'{obj.CompanyPerson}','{obj.CompanyPhone}','{obj.Bank}','{obj.BankProp}','{obj.UNP}');", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(Provider obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM provider WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Provider> Search(string text)
        {
            List<Provider> providers = new List<Provider>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM providers WHERE " +
                    $"CONCAT(name,' ', surname,' ', pathnetic,' ',position,' ',phonenumber) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Provider provider = new Provider()
                        {
                            ID = reader.GetInt32(0),
                            CompanyName = reader.GetString(1),
                            Adress = reader.GetString(2),
                            CompanyPerson = reader.GetString(3),
                            CompanyPhone = reader.GetString(4),
                            Bank = reader.GetString(5),
                            BankProp = reader.GetString(6),
                            UNP = reader.GetString(7)
                        };
                        providers.Add(provider);
                    }
                }
                _connection.Close();
            }
            return providers;
        }

        public List<Provider> ToList()
        {
            return Select("SELECT * FROM customers;");
        }

        public List<Provider> Select(string query)
        {
            List<Provider> providers = new List<Provider>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Provider provider = new Provider()
                        {
                            ID = reader.GetInt32(0),
                            CompanyName = reader.GetString(1),
                            Adress = reader.GetString(2),
                            CompanyPerson = reader.GetString(3),
                            CompanyPhone = reader.GetString(4),
                            Bank = reader.GetString(5),
                            BankProp = reader.GetString(6),
                            UNP = reader.GetString(7)
                        };
                        providers.Add(provider);
                    }
                }
                _connection.Close();
            }
            return providers;
        }
    }
    public class TradesTable : IDBSetBase<Trade>
    {
        private MySqlConnection _connection;

        public TradesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS trades " +
                "(ID int NOT NULL AUTO_INCREMENT, Date date not null, SellerFio varchar(50) not null," +
                "materialname varchar(100) not null, count float, price float, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM trades;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.Trade ElementAt(int id)
        {
            Trade obj = new Trade();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM trades WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        obj.ID = reader.GetInt32(0);
                        obj.Date = reader.GetDateTime(1);
                        obj.SellerFio = reader.GetString(2);
                        obj.MaterialName = reader.GetString(3);
                        obj.Count = reader.GetFloat(4);
                        obj.Price = reader.GetFloat(5);
                    }
                }
                _connection.Close();
            }
            return obj;
        }

        public void Add(Trade obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO trades " +
                "(Date, SellerFio, MaterialName, Count, Price) VALUES" +
                $"('{obj.Date!.Value.ToShortDateString()}','{obj.SellerFio}'," +
                $"'{obj.MaterialName}',{obj.Count},{obj.Price});", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(Trade obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM trades WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Trade> Search(string text)
        {
            List<Trade> trades = new List<Trade>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM trades" +
                    $" WHERE CONCAT(SellerFio,' ', MaterialName) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Trade trade = new Trade()
                        {
                            ID = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            SellerFio = reader.GetString(2),
                            MaterialName = reader.GetString(3),
                            Count = reader.GetFloat(4),
                            Price = reader.GetFloat(5),
                        };
                        trades.Add(trade);
                    }
                }
                _connection.Close();
            }
            return trades;
        }

        public List<Trade> ToList()
        {
            return Select("SELECT * FROM trades;");
        }

        public List<Trade> Select(string query)
        {
            List<Trade> trades = new List<Trade>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Trade trade = new Trade()
                        {
                            ID = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            SellerFio = reader.GetString(2),
                            MaterialName = reader.GetString(3),
                            Count = reader.GetFloat(4),
                            Price = reader.GetFloat(5),
                        };
                        trades.Add(trade);
                    }
                }
                _connection.Close();
            }
            return trades;
        }
    }
    public class TTNSTable : IDBSetBase<TTN>
    {
        private MySqlConnection _connection;

        public TTNSTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS ttns " +
                "(ID int NOT NULL AUTO_INCREMENT, Shipper varchar(100), " +
                "Consignee varchar(50) not null," +
                "Payer varchar(100) not null, count float not null," +
                "price float, MaterialName varchar(100) not null," +
                "CountUnits varchar(20), weight float not null," +
                "summ float not null, date date not null, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM ttns;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.TTN ElementAt(int id)
        {
            TTN obj = new TTN();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM ttns WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        obj.ID = reader.GetInt32(0);
                        obj.Shipper = reader.GetString(1);
                        obj.Consignee = reader.GetString(2);
                        obj.Payer = reader.GetString(3);
                        obj.Count = reader.GetFloat(4);
                        obj.Price = reader.GetFloat(5);
                        obj.MaterialName = reader.GetString(6);
                        obj.CountUnits = reader.GetString(7);
                        obj.Weight = reader.GetFloat(8);
                        obj.Summ = reader.GetFloat(9);
                        obj.Date = reader.GetDateTime(10);
                    }
                }
                _connection.Close();
            }
            return obj;
        }

        public void Add(TTN obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO ttns " +
                "(shpper, Consignee, Payer, Count, Price,Weight,Summ, Date) VALUES" +
                $"('{obj.Shipper}','{obj.Consignee}'," +
                $"'{obj.Payer}',{obj.Count},{obj.Price},{obj.Weight},{obj.Summ},'{obj.DateInString}');", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(TTN obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM ttns WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<TTN> Search(string text)
        {
            List<TTN> ttns = new List<TTN>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM ttns" +
                    $" WHERE CONCAT(Shipper,' ', Consignee,' ',Payer,' ',MaterialName) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        TTN obj = new TTN()
                        {
                            ID = reader.GetInt32(0),
                            Shipper = reader.GetString(1),
                            Consignee = reader.GetString(2),
                            Payer = reader.GetString(3),
                            Count = reader.GetFloat(4),
                            Price = reader.GetFloat(5),
                            MaterialName = reader.GetString(6),
                            CountUnits = reader.GetString(7),
                            Weight = reader.GetFloat(8),
                            Summ = reader.GetFloat(9),
                            Date = reader.GetDateTime(10)
                        };
                        ttns.Add(obj);
                    }
                }
                _connection.Close();
            }
            return ttns;
        }

        public List<TTN> ToList()
        {
            return Select("SELECT * FROM ttns;");
        }

        public List<TTN> Select(string query)
        {
            List<TTN> ttns = new List<TTN>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        TTN obj = new TTN()
                        {
                            ID = reader.GetInt32(0),
                            Shipper = reader.GetString(1),
                            Consignee = reader.GetString(2),
                            Payer = reader.GetString(3),
                            Count = reader.GetFloat(4),
                            Price = reader.GetFloat(5),
                            MaterialName = reader.GetString(6),
                            CountUnits = reader.GetString(7),
                            Weight = reader.GetFloat(8),
                            Summ = reader.GetFloat(9),
                            Date = reader.GetDateTime(10)
                        };
                        ttns.Add(obj);
                    }
                }
                _connection.Close();
            }
            return ttns;
        }
    }
    public class AccountsTable : IDBSetBase<Account>
    {
        private MySqlConnection _connection;

        public AccountsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS accounts " +
                "(ID int NOT NULL AUTO_INCREMENT, Seller varchar(100), " +
                "ShipperName varchar(50) not null," +
                "ShipperAdress varchar(100) not null, ConsigneeName varchar(50) not null," +
                "ConsigneeAdress varchar(100) not null, Buyer varchar(100) not null," +
                "CountUnits varchar(20), Count float not null," +
                "summ float not null, Price float not null, Tax float not null, TaxSumm float, date date not null," +
                " PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM accounts;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.Account ElementAt(int id)
        {
            Account obj = new Account();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        obj.ID = reader.GetInt32(0);
                        obj.Seller = reader.GetString(1);
                        obj.ShipperName = reader.GetString(2);
                        obj.ShipperAdress = reader.GetString(3);
                        obj.ConsigneeName = reader.GetString(4);
                        obj.ConsigneeAdress = reader.GetString(5);
                        obj.Buyer = reader.GetString(6);
                        obj.CountUnits = reader.GetString(7);
                        obj.Count = reader.GetFloat(8);
                        obj.Price = reader.GetFloat(9);
                        obj.Summ = reader.GetFloat(10);
                        obj.Tax = reader.GetFloat(11);
                        obj.TaxSumm = reader.GetFloat(12);
                        obj.Date = reader.GetDateTime(13);
                    }
                }
                _connection.Close();
            }
            return obj;
        }

        public void Add(Account obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO accounts " +
                "(Seller, ShipperName, ShipperAdress, ConsigneeName, ConsigneeAdress," +
                "Buyer,CountUnits, Count,Price,Summ,Tax,TaxSumm,Date) VALUES" +
                $"('{obj.Seller}','{obj.ShipperName}','{obj.ShipperAdress}','{obj.ConsigneeName}'," +
                $"'{obj.ConsigneeAdress}',{obj.Buyer},{obj.CountUnits},{obj.Count},{obj.Price}," +
                $"{obj.Summ},{obj.Tax},{obj.TaxSumm},'{obj.DateInString}');", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(Account obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM accounts WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Account> Search(string text)
        {
            List<Account> ttns = new List<Account>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts" +
                    $" WHERE CONCAT(Seller,' ', ShipperName,' ',ShipperAdress,' ',ConsigneeName," +
                    $"' ',ConsigneeAdress,' ',Buyer) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Account obj = new Account()
                        {
                            ID = reader.GetInt32(0),
                            Seller = reader.GetString(1),
                            ShipperName = reader.GetString(2),
                            ShipperAdress = reader.GetString(3),
                            ConsigneeName = reader.GetString(4),
                            ConsigneeAdress = reader.GetString(5),
                            Buyer = reader.GetString(6),
                            CountUnits = reader.GetString(7),
                            Count = reader.GetFloat(8),
                            Price = reader.GetFloat(9),
                            Summ = reader.GetFloat(10),
                            Tax = reader.GetFloat(11),
                            TaxSumm = reader.GetFloat(12),
                            Date = reader.GetDateTime(13),
                        };
                        ttns.Add(obj);
                    }
                }
                _connection.Close();
            }
            return ttns;
        }

        public List<Account> ToList()
        {
            return Select("SELECT * FROM accounts;");
        }

        public List<Account> Select(string query)
        {
            List<Account> ttns = new List<Account>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Account obj = new Account()
                        {
                            ID = reader.GetInt32(0),
                            Seller = reader.GetString(1),
                            ShipperName = reader.GetString(2),
                            ShipperAdress = reader.GetString(3),
                            ConsigneeName = reader.GetString(4),
                            ConsigneeAdress = reader.GetString(5),
                            Buyer = reader.GetString(6),
                            CountUnits = reader.GetString(7),
                            Count = reader.GetFloat(8),
                            Price = reader.GetFloat(9),
                            Summ = reader.GetFloat(10),
                            Tax = reader.GetFloat(11),
                            TaxSumm = reader.GetFloat(12),
                            Date = reader.GetDateTime(13),
                        };
                        ttns.Add(obj);
                    }
                }
                _connection.Close();
            }
            return ttns;
        }
    }
    public class ContractsTable : IDBSetBase<Contract>
    {
        private MySqlConnection _connection;

        public ContractsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS contracts " +
                "(seller varchar(100), buyer varchar(100)," +
                "materialname varchar(100), count float not null," +
                "countunits varchar(30), price varchar(20)," +
                "summ varchar(30), date date," +
                " PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM contracts;", _connection))
            {
                _connection.Open();
                string? bdValue = _command.ExecuteScalar() as string;
                _connection.Close();
                if (bdValue != null)
                {
                    return Convert.ToInt32(bdValue);
                }
                return 0;
            }
        }

        public BuildMaterials.Models.Contract ElementAt(int id)
        {
            Contract obj = new Contract();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contracts WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        obj.ID = reader.GetInt32(0);
                        obj.Seller = reader.GetString(1);
                        obj.Buyer = reader.GetString(2);
                        obj.MaterialName = reader.GetString(3);
                        obj.Count = reader.GetFloat(4);
                        obj.CountUnits = reader.GetString(5);
                        obj.Price = reader.GetString(6);
                        obj.Summ = reader.GetString(7);
                        obj.Date = reader.GetDateTime(8);
                    }
                }
                _connection.Close();
            }
            return obj;
        }

        public void Add(Contract obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO contracts " +
                "(Seller, Buyer, MaterialName, Count, CountUnits," +
                "Price,Summ, Date) VALUES" +
                $"('{obj.Seller}','{obj.Buyer}','{obj.MaterialName}','{obj.Count}'," +
                $"'{obj.CountUnits}','{obj.Price}','{obj.Summ}','{obj.Date}');", _connection))
            {
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Remove(Contract obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM contracts WHERE id={id};", _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public List<Contract> Search(string text)
        {
            List<Contract> contracts = new List<Contract>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contracts" +
                    $" WHERE CONCAT(Seller,' ', Buyer,' ',MaterialName,' ',ConsigneeName," +
                    $"' ',ConsigneeAdress,' ',Buyer) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Contract obj = new Contract();

                        obj.ID = reader.GetInt32(0);
                        obj.Seller = reader.GetString(1);
                        obj.Buyer = reader.GetString(2);
                        obj.MaterialName = reader.GetString(3);
                        obj.Count = reader.GetFloat(4);
                        obj.CountUnits = reader.GetString(5);
                        obj.Price = reader.GetString(6);
                        obj.Summ = reader.GetString(7);
                        obj.Date = reader.GetDateTime(8);
                        contracts.Add(obj);
                    }
                }
                _connection.Close();
            }
            return contracts;
        }

        public List<Contract> ToList()
        {
            return Select("SELECT * FROM contracts;");
        }

        public List<Contract> Select(string query)
        {
            List<Contract> ttns = new List<Contract>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteReaderAsync().Result;
                    while (reader.Read())
                    {
                        Contract obj = new Contract();

                        obj.ID = reader.GetInt32(0);
                        obj.Seller = reader.GetString(1);
                        obj.Buyer = reader.GetString(2);
                        obj.MaterialName = reader.GetString(3);
                        obj.Count = reader.GetFloat(4);
                        obj.CountUnits = reader.GetString(5);
                        obj.Price = reader.GetString(6);
                        obj.Summ = reader.GetString(7);
                        obj.Date = reader.GetDateTime(8);
                        ttns.Add(obj);
                    }
                }
                _connection.Close();
            }
            return ttns;
        }
    }
}