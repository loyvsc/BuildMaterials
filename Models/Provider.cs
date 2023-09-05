namespace BuildMaterials.Models
{
    public class Provider : BD.ITable
    {
        public int ID { get; set; }
        public string? CompanyName { get; set; } = string.Empty;
        public string? Adress { get; set; } = string.Empty;
        public string? CompanyPerson { get; set; } = string.Empty;
        public string? CompanyPhone { get; set; } = string.Empty;
        public string? Bank { get; set; } = string.Empty;
        public string? BankProp { get; set; } = string.Empty;
        public string? UNP { get; set; } = string.Empty;

        public Provider() { }
        public Provider(int iD, string? companyName, string? adress, string? companyPerson, string? companyPhone, string? bank, string? bankProp, string? uNP)
        {
            ID = iD;
            CompanyName = companyName;
            Adress = adress;
            CompanyPerson = companyPerson;
            CompanyPhone = companyPhone;
            Bank = bank;
            BankProp = bankProp;
            UNP = uNP;
        }

        public string AsString()
        {
            return $"Поставщик #{ID}\nКомпания: {CompanyName} по адресу: {Adress}\nПредставитель: {CompanyPerson} (номер телефона: {CompanyPhone})\nУНП: {UNP}\nБанковские реквизиты: {Bank} {BankProp}";
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