using BuildMaterials.BD;
using System;

namespace BuildMaterials.Models
{
    public class TTN : ITable
    {
        public int ID { get; set; }
        public string? Shipper { get; set; } = string.Empty;
        public string? Consignee { get; set; } = string.Empty;
        public string? Payer { get; set; } = string.Empty;
        public float Count { get; set; } = 0;
        public float Price { get; set; } = 0;
        public string? MaterialName { get; set; } = string.Empty;
        public string? CountUnits { get; set; } = string.Empty;
        public float Weight { get; set; } = 0;
        public float Summ { get; set; } = 0;
        public DateTime? Date { get; set; }

        public string? DateInString => Date?.ToShortDateString();

        public TTN() { }
        public TTN(int iD, string? shipper, string? consignee, string? payer, float count, float price, string materialName, string countUnit, float weight, float summ, DateTime? date)
        {
            ID = iD;
            Shipper = shipper;
            Consignee = consignee;
            Payer = payer;
            Count = count;
            Price = price;
            MaterialName = materialName;
            CountUnits = countUnit;
            Weight = weight;
            Summ = summ;
            Date = date;
        }

        public override string ToString()
        {
            return $"ТТН от {DateInString}\nГрузоотправитель: {Shipper}\nГрузополучатель: {Consignee}\nПлательщик: {Payer}\nМатериал: {MaterialName}\nКоличество: {Count} {CountUnits}\nЦена: {Price}\nСумма: {Summ}";
        }

        public bool IsValid =>
            Shipper != string.Empty &&
            Consignee != string.Empty &&
            Payer != string.Empty &&
            MaterialName != string.Empty &&
            CountUnits != string.Empty &&
            Date != null;
    }
}