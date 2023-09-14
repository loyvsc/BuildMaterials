using BuildMaterials.BD;
using System.ComponentModel;

namespace BuildMaterials.Models
{
    public class Contract : NotifyPropertyChangedBase, ITable
    {
        private readonly bool UseBD;
        public int ID { get; set; }
        public string? Seller
        {
            get => seller;
            set
            {
                seller = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Contract SET Seller ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Buyer
        {
            get => buyer;
            set
            {
                buyer = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Contract SET Buyer ='{value}' WHERE ID={ID};");
                }
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
                    App.DBContext.Query($"UPDATE Contract SET MaterialName ='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Contract SET Count ='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Summ));
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
                    App.DBContext.Query($"UPDATE Contract SET CountUnits ='{value}' WHERE ID={ID};");
                }
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
                    App.DBContext.Query($"UPDATE Contract SET Price ='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
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
                    App.DBContext.Query($"UPDATE Contract SET Date ='{value!.Value.Year}-{value!.Value.Month}-{value!.Value.Day}' WHERE ID={ID};");
                }
            }
        }
        public string? DateInString => Date?.ToShortDateString();

        private DateTime? date;
        private string? countUnits = string.Empty;
        private string? materialName = string.Empty;
        private string? seller = string.Empty;
        private string? buyer = string.Empty;
        private float count = 0;
        private float price = 0;        

        public Contract()
        {
            UseBD = false;
        }

        public Contract(int iD, string? seller, string? buyer, string? materialName, float count,
            string? countUnits, float price, DateTime? date)
        {
            UseBD = false;
            ID = iD;
            Seller = seller;
            Buyer = buyer;
            MaterialName = materialName;
            Count = count;
            CountUnits = countUnits;
            Price = price;
            Date = date;
            UseBD = true;
        }

        public override string ToString()
        {
            return $"Договор купли-продажи #{ID} от {DateInString}\n\nПокупатель: {Buyer}\nПродавец: {Seller}\nТовар \"{MaterialName}\" в количестве {Count} {CountUnits}.\nЦена за единицу ({CountUnits}): {Price}.\nСумма: {Summ}.\n\n               {Seller}\n\n               {Buyer}";
        }

        public bool IsValid => Date != null;
    }
}