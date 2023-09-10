namespace BuildMaterials.Models
{
    public class Provider : BD.ITable
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
                    App.DBContext.Query($"UPDATE Providers SET CompanyName ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Providers SET Adress ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Providers SET CompanyPerson ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Providers SET CompanyPhone ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Providers SET Bank ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Providers SET BankProp ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Providers SET UNP ='{value}' WHERE ID={ID};");
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

        public Provider() { UseBD = false; }
        public Provider(int iD, string? companyName, string? adress, string? companyPerson, string? companyPhone, string? bank, string? bankProp, string? uNP)
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

        public string AsString()
        {
            return $"Поставщик №{ID}\nКомпания: {CompanyName} по адресу: {Adress}\nПредставитель: {CompanyPerson} (номер телефона: {CompanyPhone})\nУНП: {UNP}\nБанковские реквизиты: {Bank} {BankProp}";
        }

        public bool IsValid =>
            CompanyName != string.Empty &&
            CompanyPerson != string.Empty &&
            Adress != string.Empty &&
            CompanyPerson != string.Empty &&
            CompanyPhone != string.Empty &&
            Bank != string.Empty &&
            BankProp != string.Empty &&
            UNP != string.Empty;

        public override string ToString()
        {
            return CompanyName!;
        }
    }
}