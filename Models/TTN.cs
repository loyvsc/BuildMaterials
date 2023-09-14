using BuildMaterials.BD;
using System;
using System.ComponentModel;

namespace BuildMaterials.Models
{
    public class TTN : NotifyPropertyChangedBase, ITable
    {
        private readonly bool UseBD;
        public int ID { get; set; }
        public string? Shipper
        {
            get => shipper;
            set
            {
                shipper = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Shipper = '{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Consignee
        {
            get => consignee;
            set
            {
                consignee = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Consignee = '{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Payer
        {
            get => payer;
            set
            {
                payer = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Payer = '{value}' WHERE ID={ID};");
                }
            }
        }
        public float Count
        {
            get => count;
            set
            {
                count = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Count={value} WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Summ));
            }
        }
        public float Price
        {
            get => price;
            set
            {
                price = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Price={value} WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
            }
        }
        public string? MaterialName
        {
            get => materialName;
            set
            {
                materialName = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET MaterialName = '{value}' WHERE ID={ID};");
                }
            }
        }
        public string? CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET CountUnits = '{value}' WHERE ID={ID};");
                }
            }
        }
        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Weight = {value} WHERE ID={ID};");
                }
            }
        }
        public float Summ => Count * Price;
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE TTNS SET Date = {value} WHERE ID={ID};");
                }
            }
        }

        private string? shipper = string.Empty;
        private string? consignee = string.Empty;
        private string? payer = string.Empty;
        private string? materialName = string.Empty;
        private string? countUnits = string.Empty;
        private float weight = 0;
        private float count = 0;
        private float price = 0;
        private DateTime? date;

        public string? DateInString => Date?.ToShortDateString();

        public TTN() { UseBD = false; }
        public TTN(int iD, string? shipper, string? consignee, string? payer, float count, float price, string materialName, string countUnit, float weight, DateTime? date)
        {
            UseBD = false;
            ID = iD;
            Shipper = shipper;
            Consignee = consignee;
            Payer = payer;
            Count = count;
            Price = price;
            MaterialName = materialName;
            CountUnits = countUnit;
            Weight = weight;
            Date = date;
            UseBD = false;
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