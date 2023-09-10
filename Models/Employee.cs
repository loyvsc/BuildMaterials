using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Employee : ITable
    {
        private readonly bool UseBD;
        public int ID { get; set; }
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET Name ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? SurName
        {
            get => surname;
            set
            {
                surname = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET SurName ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Pathnetic
        {
            get => pathnetic;
            set
            {
                pathnetic = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET Pathnetic ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Position
        {
            get => position;
            set
            {
                position = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET Position ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET PhoneNumber ='{value}' WHERE ID={ID};");
                }
            }
        }
        public int Password
        {
            get => password;
            set
            {
                password = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET Password ='{value}' WHERE ID={ID};");
                }
            }
        }
        public bool FinResponsible
        {
            get => finResponsible;
            set
            {
                finResponsible = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET FinResponsible = {value} WHERE ID={ID};");
                }
            }
        }
        public int AccessLevel
        {
            get => accessLevel;
            set
            {
                accessLevel = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Employees SET AccessLevel ={value} WHERE ID={ID};");
                }
            }
        }

        private string? name;
        private string? surname;
        private string? pathnetic;
        private string? position;
        private string? phoneNumber;
        private int password;
        private bool finResponsible = false;
        private int accessLevel;

        public string AccessLevelInString => App.DBContext.AccessLevel[AccessLevel];

        public Employee()
        {
            UseBD = false;
        }

        public Employee(int id, string name, string surName, string pathnetic, string position, string phoneNumber, int password = 0, int accessLevel = 3, bool finResp = false)
        {
            ID = id;
            UseBD = false;
            FinResponsible = finResp;
            Name = name;
            SurName = surName;
            Pathnetic = pathnetic;
            Position = position;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessLevel = accessLevel;
            UseBD = true;
        }

        public Employee(string position, int password, int accessLevel)
        {
            UseBD = false;
            Position = position;
            Password = password;
            AccessLevel = accessLevel;
        }

        public override string ToString()
        {
            string text = $"Сотрудник №{ID}\nФ.И.О.: {SurName} {Name} {Pathnetic}\nДолжность: {Position}\nНомер телефона: {PhoneNumber}\n";
            if (FinResponsible)
            {
                text += "Материально ответственный сотрудник";
            }
            else
            {
                text += "Не является материально ответственным";
            }
            return text;
        }

        public bool IsValid =>
            Name != string.Empty &&
            SurName != string.Empty &&
            Pathnetic != string.Empty &&
            Position != string.Empty &&
            PhoneNumber != string.Empty;
    }
}