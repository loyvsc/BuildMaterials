using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.BD
{
    public static class StaticValues
    {
        public const string ConnectionString = "server=localhost;user=root;database=buildmaterials;password=546909021Var;";
        public const string CreateDatabaseConnectionString = "server=localhost;user=root;password=546909021Var;";
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
        public MaterialResponsesTable MaterialResponse { get; set; } = null!;
        public readonly string[] AccessLevel = new string[4] { "Минимальный", "Низкий", "Средний", "Максимальный" };

        public ApplicationContext()
        {
            InitializeDatabase();
            if (Employees?.Count() == 0)
            {
                Employees.Add(new Employee(-1, "Имя", "Фамилия", "Отчество", "Администратор", "+375259991234", 0, 3, false));
            }
        }

        public void InitializeDatabase()
        {
            CreateDatabase();
            InitTables();
        }

        private void InitTables()
        {
            Employees = new EmployeesTable();
            Customers = new CustomersTable();
            Providers = new ProvidersTable();
            Materials = new MaterialsTable();
            Trades = new TradesTable();
            TTNs = new TTNSTable();
            Accounts = new AccountsTable();
            Contracts = new ContractsTable();
            MaterialResponse = new MaterialResponsesTable();
        }

        private void CreateDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(StaticValues.CreateDatabaseConnectionString))
            {
                connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS buildmaterials;", connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                connection.CloseAsync().Wait();
            }
        }

        public void Query(string query)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }
    }

    public class MaterialResponsesTable : IDBSetBase<MaterialResponse>
    {
        private readonly MySqlConnection _connection;

        public MaterialResponsesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("CREATE TABLE IF NOT EXISTS materialresponses " +
                "(ID int not null, Name varchar(100), CountUnits varchar(100), BalanceAtStart float not null, " +
                "Prihod float not null, Rashod float not null, BalanceAtEnd float not null," +
                "FinResposeEmployeeID int not null, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public MaterialResponse ElementAt(int id)
        {
            MaterialResponse material = new MaterialResponse();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM materialresponses WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        material.ID = reader.GetInt32(0);
                        material.Name = reader.GetString(1);
                        material.BalanceAtStart = reader.GetFloat(2);
                        material.BalanceAtStart = reader.GetFloat(3);
                        material.Prihod = reader.GetFloat(4);
                        material.Rashod = reader.GetFloat(5);
                        material.BalanceAtEnd = reader.GetFloat(6);
                        material.FinResponseEmployeeID = reader.GetInt32(7);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return material;
        }

        public void Add(MaterialResponse obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO materialresponses " +
                "(Name, CountUnits, BalanceAtStart, Prihod, Rashod, BalanceAtEnd, FinResponseEmployeeID) VALUES" +
                $"('{obj.Name}','{obj.CountUnits}',{obj.BalanceAtStart},{obj.Prihod}," +
                $"{obj.Rashod},{obj.BalanceAtEnd},{obj.FinResponseEmployeeID});", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(MaterialResponse obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materialresponses WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<MaterialResponse> Search(string text)
        {
            List<MaterialResponse> materials = new List<MaterialResponse>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM materialresponses WHERE " +
                    $"Name like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetMaterialResponse(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        public List<MaterialResponse> Select(string query)
        {
            List<MaterialResponse> materials = new List<MaterialResponse>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetMaterialResponse(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        private MaterialResponse GetMaterialResponse(MySqlDataReader reader)
        {
            MaterialResponse material = new MaterialResponse();
            material.ID = reader.GetInt32(0);
            material.Name = reader.GetString(1);
            material.BalanceAtStart = reader.GetFloat(2);
            material.BalanceAtStart = reader.GetFloat(3);
            material.Prihod = reader.GetFloat(4);
            material.Rashod = reader.GetFloat(5);
            material.BalanceAtEnd = reader.GetFloat(6);
            material.FinResponseEmployeeID = reader.GetInt32(7);
            material.UseBD = true;
            return material;
        }

        public List<MaterialResponse> ToList()
        {
            List<MaterialResponse> materials = new List<MaterialResponse>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM materialresponses;", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetMaterialResponse(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }
    }
    public class MaterialsTable : IDBSetBase<Material>
    {
        private readonly MySqlConnection _connection;

        public MaterialsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("CREATE TABLE IF NOT EXISTS materials (ID int NOT NULL AUTO_INCREMENT, Name varchar(300) not null, Manufacturer varchar(100) not null, Price float NOT NULL,Count float NOT NULL,CountUnits varchar(20) ,EnterDate datetime NOT NULL,EnterCount float NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public BuildMaterials.Models.Material ElementAt(int id)
        {
            BuildMaterials.Models.Material material = new BuildMaterials.Models.Material();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        return GetMaterial(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return material;
        }

        public void Add(Material obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO materials " +
                "(Name, Manufacturer, Price, Count, CountUnits, EnterDate, EnterCount) VALUES" +
                $"('{obj.Name}','{obj.Manufacturer}',{obj.Price},{obj.Count},'{obj.CountUnits}','{obj.EnterDate.Year}-{obj.EnterDate.Month}-{obj.EnterDate.Day}',{obj.EnterCount});", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(Material obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Material> Search(string text)
        {
            List<Material> materials = new List<Material>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE " +
                    $"CONCAT(name,' ', manufacturer,' ', price,' ',count,' ',enterdate,' ',entercount) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        Material employee = new Material(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetFloat(3), reader.GetFloat(4), reader.GetString(5), reader.GetDateTime(6),
                            reader.GetFloat(7));
                        materials.Add(employee);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        public List<Material> Select(string query)
        {
            List<Material> materials = new List<Material>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        Material material = new Material()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Count = reader.GetFloat(2),
                        };
                        materials.Add(material);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        private Material GetMaterial(MySqlDataReader reader)
        {
            return new Material(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetFloat(3), reader.GetFloat(4), reader.GetString(5), reader.GetDateTime(6), reader.GetFloat(7));
        }

        public List<Material> ToList()
        {
            List<Material> materials = new List<Material>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Materials;", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetMaterial(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }
    }
    public class EmployeesTable : IDBSetBase<Employee>
    {
        private readonly MySqlConnection _connection;

        public EmployeesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("CREATE TABLE IF NOT EXISTS employees" +
                "(ID int NOT NULL AUTO_INCREMENT, Name varchar(50), Surname varchar(50)," +
                "Pathnetic varchar(70), Position varchar(100), PhoneNumber varchar(14)," +
                "Password int NOT NULL, AccessLevel int NOT NULL, FinResponsible boolean, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM employees;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Employee GetEmployee(MySqlDataReader reader)
        {
            return new Employee(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                reader.GetString(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetBoolean(8));
        }

        public Employee ElementAt(int id)
        {
            Employee employee = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employees WHERE id={id};", _connection))
                {
                    using (MySqlDataReader reader = command.ExecuteMySqlReaderAsync())
                        while (reader.Read())
                        {
                            employee = GetEmployee(reader);
                        }
                }
                _connection.CloseAsync().Wait();
            }
            return employee;
        }

        public void Add(Employee obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO employees " +
                "(Name, Surname, pathnetic, position, phonenumber, password, AccessLevel, FinResponsible) VALUES" +
                $"('{obj.Name}','{obj.SurName}'," +
                $"'{obj.Pathnetic}','{obj.Position}','{obj.PhoneNumber}',{obj.Password},{obj.AccessLevel},{obj.FinResponsible});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Employee obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM employees WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM employees WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Employee> Search(string text)
        {
            List<Employee> employees = new List<Employee>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employees WHERE " +
                    $"CONCAT(name,' ', surname,' ', pathnetic,' ',position,' ',phonenumber) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        employees.Add(GetEmployee(reader));
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        employees.Add(GetEmployee(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return employees;
        }
    }
    public class CustomersTable : IDBSetBase<Customer>
    {
        private readonly MySqlConnection _connection;

        public CustomersTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS customers " +
                "(ID int NOT NULL AUTO_INCREMENT, CompanyName varchar(100), Adress varchar(50)," +
                "CompanyPerson varchar(70), CompanyPhone varchar(20), Bank varchar(100)," +
                "BankProp varchar(100) NOT NULL, UNP varchar(100) NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM customers;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Customer GetCustomer(MySqlDataReader reader)
        {
            return new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                reader.GetString(5), reader.GetString(6), reader.GetString(7));
        }

        public Customer ElementAt(int id)
        {
            Customer obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM customers WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetCustomer(reader);
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Customer obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM customers WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Customer> Search(string text)
        {
            List<Customer> customers = new List<Customer>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM customers WHERE " +
                    $"CONCAT(CompanyName,' ', Adress,' ', CompanyPerson,' ',CompanyPhone,' '," +
                    $"Bank,' ',Bankprop,' ', UNP) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        customers.Add(GetCustomer(reader));
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        customers.Add(GetCustomer(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return customers;
        }
    }
    public class ProvidersTable : IDBSetBase<Provider>
    {
        private readonly MySqlConnection _connection;

        public ProvidersTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS providers " +
                "(ID int NOT NULL AUTO_INCREMENT, CompanyName varchar(100), Adress varchar(50)," +
                "CompanyPerson varchar(70), CompanyPhone varchar(20), Bank varchar(100)," +
                "BankProp varchar(100) NOT NULL, UNP varchar(100) NOT NULL, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM providers;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Provider GetProvider(MySqlDataReader reader)
        {
            return new Provider(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                reader.GetString(5), reader.GetString(6), reader.GetString(7));
        }

        public BuildMaterials.Models.Provider ElementAt(int id)
        {
            Provider obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM providers WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetProvider(reader);
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Provider obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM providers WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Provider> Search(string text)
        {
            List<Provider> providers = new List<Provider>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM providers WHERE " +
                    $"CONCAT(companyname,' ', adress,' ', companyperson,' ',bank,' ',bankprop,' ', unp) like '%{text}%';", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        providers.Add(GetProvider(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return providers;
        }

        public List<Provider> ToList()
        {
            return Select("SELECT * FROM providers;");
        }

        public List<Provider> Select(string query)
        {
            List<Provider> providers = new List<Provider>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        providers.Add(GetProvider(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return providers;
        }
    }
    public class TradesTable : IDBSetBase<Trade>
    {
        private readonly MySqlConnection _connection;

        public TradesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS trades " +
                "(ID int NOT NULL AUTO_INCREMENT, Date date not null, SellerFio varchar(50) not null," +
                "materialname varchar(100) not null, count float, price float, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM trades;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Trade GetTrade(MySqlDataReader reader)
        {
            return new Trade(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2), reader.GetString(3), reader.GetFloat(4), reader.GetFloat(5));
        }

        public BuildMaterials.Models.Trade ElementAt(int id)
        {
            Trade obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM trades WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetTrade(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Add(Trade obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO trades " +
                "(Date, SellerFio, MaterialName, Count, Price) VALUES" +
                $"('{obj.Date!.Value.Year}-{obj.Date!.Value.Month}-{obj.Date!.Value.Day}','{obj.SellerFio}'," +
                $"'{obj.MaterialName}',{obj.Count},{obj.Price});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Trade obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM trades WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Trade> Search(string text)
        {
            List<Trade> trades = new List<Trade>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM trades" +
                    $" WHERE CONCAT(SellerFio,' ', MaterialName) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        trades.Add(GetTrade(reader));
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        trades.Add(GetTrade(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return trades;
        }
    }
    public class TTNSTable : IDBSetBase<TTN>
    {
        private readonly MySqlConnection _connection;

        public TTNSTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS ttns " +
                "(ID int NOT NULL AUTO_INCREMENT, Shipper varchar(100), " +
                "Consignee varchar(50) not null," +
                "Payer varchar(100) not null, count float not null," +
                "price float, MaterialName varchar(100) not null," +
                "CountUnits varchar(20), weight float not null," +
                "date date not null, PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM ttns;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private TTN GetTTN(MySqlDataReader reader)
        {
            return new TTN(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetFloat(4),
            reader.GetFloat(5), reader.GetString(6), reader.GetString(7), reader.GetFloat(8), reader.GetDateTime(9));
        }

        public TTN ElementAt(int id)
        {
            TTN obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM ttns WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetTTN(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Add(TTN obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO ttns " +
                "(shipper, Consignee, Payer, Count, Price,Weight, Date, MaterialName,CountUnits) VALUES" +
                $"('{obj.Shipper}','{obj.Consignee}'," +
                $"'{obj.Payer}',{obj.Count},{obj.Price},{obj.Weight},'{obj.Date!.Value.Year}-{obj.Date!.Value.Month}-{obj.Date!.Value.Day}'," +
                $"'{obj.MaterialName}','{obj.CountUnits}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(TTN obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM ttns WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<TTN> Search(string text)
        {
            List<TTN> ttns = new List<TTN>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM ttns" +
                    $" WHERE CONCAT(Shipper,' ', Consignee,' ',Payer,' ',MaterialName) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetTTN(reader));
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetTTN(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }
    }
    public class AccountsTable : IDBSetBase<Account>
    {
        private readonly MySqlConnection _connection;

        public AccountsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS accounts " +
                "(ID int NOT NULL AUTO_INCREMENT, Seller varchar(100), " +
                "ShipperName varchar(50) not null," +
                "ShipperAdress varchar(100) not null, ConsigneeName varchar(50) not null," +
                "ConsigneeAdress varchar(100) not null, Buyer varchar(100) not null," +
                "CountUnits varchar(20), Count float not null," +
                "Price float not null, Tax float not null, date date not null," +
                " PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM accounts;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Account GetAccount(MySqlDataReader reader)
        {
            return new Account(reader.GetInt32(0),reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                reader.GetString(5), reader.GetString(6), reader.GetString(7),
                reader.GetFloat(8), reader.GetFloat(9), reader.GetFloat(10), reader.GetDateTime(11));
        }

        public Account ElementAt(int id)
        {
            Account obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetAccount(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Add(Account obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO accounts " +
                "(Seller, ShipperName, ShipperAdress, ConsigneeName, ConsigneeAdress," +
                "Buyer,CountUnits, Count,Price,Tax,Date) VALUES" +
                $"('{obj.Seller}','{obj.ShipperName}','{obj.ShipperAdress}','{obj.ConsigneeName}'," +
                $"'{obj.ConsigneeAdress}','{obj.Buyer}','{obj.CountUnits}',{obj.Count},{obj.Price}," +
                $"{obj.Tax},'{obj.Date!.Value.Year}-{obj.Date!.Value.Month}-{obj.Date!.Value.Day}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Account obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM accounts WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Account> Search(string text)
        {
            List<Account> ttns = new List<Account>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts" +
                    $" WHERE CONCAT(Seller,' ', ShipperName,' ',ShipperAdress,' ',ConsigneeName," +
                    $"' ',ConsigneeAdress,' ',Buyer) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetAccount(reader));
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetAccount(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }
    }
    public class ContractsTable : IDBSetBase<Contract>
    {
        private readonly MySqlConnection _connection;

        public ContractsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
            Init();
        }

        private void Init()
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand
                ("CREATE TABLE IF NOT EXISTS contracts " +
                "(id int not null auto_increment, seller varchar(100), buyer varchar(100)," +
                "materialname varchar(100), count float not null," +
                "countunits varchar(30), price float," +
                "date date," +
                " PRIMARY KEY (ID));", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM contracts;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Contract GetContract(MySqlDataReader reader)
        {
            return new Contract(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetFloat(4),
                reader.GetString(5), reader.GetFloat(6), reader.GetDateTime(7));
        }

        public BuildMaterials.Models.Contract ElementAt(int id)
        {
            Contract obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contracts WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetContract(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Add(Contract obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO contracts " +
                "(Seller, Buyer, MaterialName, Count, CountUnits," +
                "Price, Date) VALUES" +
                $"('{obj.Seller}','{obj.Buyer}','{obj.MaterialName}',{obj.Count}," +
                $"'{obj.CountUnits}',{obj.Price},'{obj.Date!.Value.Year}-{obj.Date!.Value.Month}-{obj.Date!.Value.Day}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Contract obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM contracts WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Contract> Search(string text)
        {
            List<Contract> contracts = new List<Contract>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contracts" +
                    $" WHERE CONCAT(Seller,' ', Buyer,' ',MaterialName,' ',ConsigneeName," +
                    $"' ',ConsigneeAdress,' ',Buyer) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        contracts.Add(GetContract(reader));
                    }
                }
                _connection.CloseAsync().Wait();
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
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetContract(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }
    }
}