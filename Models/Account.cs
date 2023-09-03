using BuildMaterials.BD;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaterials.Models
{
    [PrimaryKey("ID")]
    public class Account : ITable
    {
        public int ID { get; set; }
        public string? Seller { get; set; } = string.Empty;
        public string? ShipperName { get; set; } = string.Empty;
        public string? ShipperAdress { get; set; } = string.Empty;
        public string? ConsigneeName { get; set; } = string.Empty;
        public string? ConsigneeAdress { get; set; } = string.Empty;
        public string? Buyer { get; set; } = string.Empty;
        public string? CountUnits { get; set; } = string.Empty;
        public float Count { get; set; } = 0;
        public float Price { get; set; } = 0;
        public float Summ { get; set; } = 0;
        public float Tax { get; set; } = 0;
        public float TaxSumm { get; set; } = 0;
        public DateTime? Date { get; set; }
        [NotMapped]
        public string? DateInString => Date?.ToShortDateString();

        public Account() { }
        public Account(int iD, string? seller, string? shipperName, string? shipperAdress, string? consigneeName, string? consigneeAdress, string? buyer, string? countUnits, float count, float price, float summ, float tax, float taxSumm, DateTime? date)
        {
            ID = iD;
            Seller = seller;
            ShipperName = shipperName;
            ShipperAdress = shipperAdress;
            ConsigneeName = consigneeName;
            ConsigneeAdress = consigneeAdress;
            Buyer = buyer;
            CountUnits = countUnits;
            Count = count;
            Price = price;
            Summ = summ;
            Tax = tax;
            TaxSumm = taxSumm;
            Date = date;
        }

        public override string ToString()
        {
            return $"Счет #{ID} от {DateInString}\nПродавец: {Seller}\nПокупатель: {Buyer}\nГрузоотправитель: {ShipperName} (адрес: {ShipperAdress})\nГрузополучатель: {ConsigneeName} (адрес: {ConsigneeAdress})\nКоличество: {Count} {CountUnits}\nЦена: {Price}\nСумма: {Summ}\nНалоговый сбор: {TaxSumm}\nИтого: {Summ+TaxSumm}";
        }

        public bool IsValid => Date != null
            && Seller != string.Empty
            && ShipperName != string.Empty
            && ShipperAdress != string.Empty
            && ConsigneeName != string.Empty
            && ConsigneeAdress != string.Empty
            && Buyer != string.Empty
            && CountUnits != string.Empty;
    }
}