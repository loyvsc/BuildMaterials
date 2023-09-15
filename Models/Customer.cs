using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class MaterialResponse : ITable
    {
        public bool UseBD;
        public int ID { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET Name = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public string CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET CountUnits = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float BalanceAtStart
        {
            get => balStart;
            set
            {
                balStart = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET BalanceAtStart = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float Prihod
        {
            get => prihod;
            set
            {
                prihod = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET Prihod = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float Rashod
        {
            get => rashod;
            set
            {
                rashod = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET Rashod = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float BalanceAtEnd
        {
            get => balEnd;
            set
            {
                balEnd = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET BalanceAtEnd = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public int FinResponseEmployeeID
        {
            get => finRespEmpID;
            set
            {
                finRespEmpID = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE MaterialResponses SET FinResponseEmployeeID = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public Employee FinReponseEmployee
        {
            get=> App.DBContext.Employees.ElementAt(FinResponseEmployeeID);
            set => FinResponseEmployeeID = value.ID;
        }

        private int finRespEmpID = 0;
        private float balEnd = 0;
        private float rashod = 0;
        private float prihod = 0;
        private float balStart = 0;
        private string countUnits = string.Empty;
        private string name = string.Empty;

        public MaterialResponse() { }
        public MaterialResponse(int iD, string name, string countUnits, float balStart, float prihod,
            float rashod, float balEnd, int finRespId)
        {
            ID = iD;
            CountUnits = countUnits;
            BalanceAtStart = balStart;
            BalanceAtEnd = balEnd;
            Prihod = prihod;
            Rashod = rashod;
            BalanceAtEnd = balEnd;
            FinResponseEmployeeID = finRespId;
            Name = name;
        }

        public override string ToString()
        {
            return $"Материально-ответственный отчет №{ID}\nМатериально-ответственный сотрудник: {FinReponseEmployee.SurName} {FinReponseEmployee.Name} {FinReponseEmployee.Pathnetic}\nПриход: {prihod}\nРасход: {rashod}\nБаланс на начало: {balStart}\nБаланс на конец: {balEnd}\nНаименование материала: {name}\nЕд. измерения: {countUnits}";
        }

        public bool IsValid => BalanceAtStart >= 0 &&
             Prihod >= 0 && Rashod >= 0;
    }

    public class Customer : ITable
    {
        public bool UseBD;
        public int ID { get; set; }
        public string? CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET CompanyName ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Adress
        {
            get => adress;
            set
            {
                adress = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET Adress ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? CompanyPerson
        {
            get => companyPerson;
            set
            {
                companyPerson = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET CompanyPerson ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? CompanyPhone
        {
            get => companyPhone;
            set
            {
                companyPhone = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET CompanyPhone ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Bank
        {
            get => bank;
            set
            {
                bank = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET Bank ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? BankProp
        {
            get => bankProp;
            set
            {
                bankProp = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET BankProp ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? UNP
        {
            get => unp;
            set
            {
                unp = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Customers SET UNP ='{value}' WHERE ID={ID};");
                }
            }
        }

        private string? companyName = string.Empty;
        private string? adress = string.Empty;
        private string? companyPerson = string.Empty;
        private string? companyPhone = string.Empty;
        private string? bank = string.Empty;
        private string? bankProp = string.Empty;
        private string? unp = string.Empty;

        public Customer()
        {
            UseBD = false;
        }

        public Customer(int iD, string? companyName, string? adress, string? companyPerson, string? companyPhone, string? bank, string? bankProp, string? uNP)
        {
            UseBD = false;
            ID = iD;
            CompanyName = companyName;
            Adress = adress;
            CompanyPerson = companyPerson;
            CompanyPhone = companyPhone;
            Bank = bank;
            BankProp = bankProp;
            UNP = uNP;
            UseBD = true;
        }

        public bool IsValid =>
            CompanyName != string.Empty &&
            CompanyPerson != string.Empty &&
            Adress != string.Empty &&
            CompanyPhone != string.Empty &&
            Bank != string.Empty &&
            BankProp != string.Empty &&
            UNP != string.Empty;

        public override string ToString()
        {
            return CompanyName!;
        }

        public string AsString()
        {
            return $"Заказчик №{ID}\nКомпания: {CompanyName}\nАдрес расположения: {Adress}\nПредставитель: {CompanyPerson} (контактный номер телефона: {CompanyPhone})\nУНП: {UNP}\nБанковские реквизиты: {Bank} {BankProp}";
        }

        public static explicit operator Customer(Provider v)
        {
            return new Customer(0, v.CompanyName, v.Adress, v.CompanyPerson, v.CompanyPhone, v.Bank, v.BankProp, v.UNP);
        }

        public static explicit operator Customer(List<Provider> v)
        {
            throw new NotImplementedException();
        }
    }
}