using BuildMaterials.BD;
using System;
using System.Collections.Generic;

namespace BuildMaterials.Models
{
    public class MaterialResponse : ITable
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;
        public string CountUnits { get; set; } = string.Empty;
        public float BalanceAtStart { get; set; }
        public float Prihod { get; set; }
        public float Rashod { get; set; }
        public float BalanceAtEnd { get; set; }
        public int FinResponseEmployeeID { get; set; }
        public Employee FinReponseEmployee => App.DBContext.Employees.ElementAt(FinResponseEmployeeID);

        public MaterialResponse() { }
        public MaterialResponse(int iD, string name, string countUnits, float balStart, float prihod, float rashod, float balEnd, int finRespId)
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

        public bool IsValid => BalanceAtStart >= 0 &&
             Prihod >= 0 && Rashod >= 0;
    }

    public class Customer : ITable
    {
        public int ID { get; set; }
        public string? CompanyName { get; set; } = string.Empty;
        public string? Adress { get; set; } = string.Empty;
        public string? CompanyPerson { get; set; } = string.Empty;
        public string? CompanyPhone { get; set; } = string.Empty;
        public string? Bank { get; set; } = string.Empty;
        public string? BankProp { get; set; } = string.Empty;
        public string? UNP { get; set; } = string.Empty;

        public Customer() { }
        public Customer(int iD, string? companyName, string? adress, string? companyPerson, string? companyPhone, string? bank, string? bankProp, string? uNP)
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
            return $"Покупатель #{ID}\nКомпания: {CompanyName} по адресу: {Adress}\nПредставитель: {CompanyPerson} (номер телефона: {CompanyPhone})\nУНП: {UNP}\nБанковские реквизиты: {Bank} {BankProp}";
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