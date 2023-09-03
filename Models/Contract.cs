using BuildMaterials.BD;
using Microsoft.EntityFrameworkCore;
using System;

namespace BuildMaterials.Models
{
    [PrimaryKey("ID")]
    public class Contract : ITable
    {
        public int ID { get; set; }
        public string? Seller { get; set; } = string.Empty;
        public string? Buyer { get; set; } = string.Empty;
        public string? MaterialName { get; set; } = string.Empty;
        public float Count { get; set; } = 0;
        public string? CountUnits { get; set; } = string.Empty;
        public string? Price { get; set; } = string.Empty;
        public string? Summ { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string? DateInString => Date?.ToShortDateString();

        public Contract() { }
        public Contract(int iD, string? seller, string? buyer, string? materialName, float count, string? countUnits, string? price, string? summ, DateTime? date)
        {
            ID = iD;
            Seller = seller;
            Buyer = buyer;
            MaterialName = materialName;
            Count = count;
            CountUnits = countUnits;
            Price = price;
            Summ = summ;
            Date = date;
        }

        public override string ToString()
        {
            return $"Договор купли-продажи #{ID} от {DateInString}\n\nПокупатель: {Buyer}\nПродавец: {Seller}\nТовар \"{MaterialName}\" в количестве {Count} {CountUnits}.\nЦена за единицу ({CountUnits}): {Price}.\nСумма: {Summ}.\n\n               {Seller}\n\n               {Buyer}";
        }

        public bool IsValid => Date != null;
    }
}