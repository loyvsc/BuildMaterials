using BuildMaterials.BD;
using System;

namespace BuildMaterials.Models
{

    public class Trade : ITable
    {
        public int ID { get; set; }
        public DateTime? Date { get; set; } = null;

        public string? DateInString
        {
            get=> Date?.ToShortDateString();
            set => Date = Convert.ToDateTime(value);
        }
        public string? SellerFio { get; set; }
        public string? MaterialName { get; set; }
        public float Count { get; set; } = 0;
        public float Price { get; set; } = 0;

        public float Summ => Count * Price;

        public Trade() { }

        public Trade(int iD, DateTime? date, string sellerFio, string materialName, float count, float price)
        {
            ID = iD;
            Date = date;
            SellerFio = sellerFio;
            MaterialName = materialName;
            Count = count;
            Price = price;
        }

        public bool IsValid => Date != null
            && SellerFio != string.Empty
            && MaterialName != string.Empty;

        public override string ToString()
        {
            return $"Товарооборот №{ID} от {DateInString}\nПродавец: {SellerFio}\nМатериал: {MaterialName}\nКоличество: {Count}\nЦена: {Price}";
        }
    }
}