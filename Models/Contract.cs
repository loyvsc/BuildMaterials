using BuildMaterials.BD;

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
        public int MaterialID
        {
            get => matid;
            set
            {
                matid = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Contracts SET MaterialID ={value} WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Material));
                OnPropertyChanged(nameof(MaterialID));
            }
        }
        public Material? Material
        {
            get => App.DBContext.Materials.ElementAt(MaterialID);
            set
            {
                if (value != null)
                {
                    MaterialID = value.ID;
                    OnPropertyChanged(nameof(Material));
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
        private string? seller = string.Empty;
        private string? buyer = string.Empty;
        private float count = 0;
        private float price = 0;
        private int matid = -1;

        public Contract()
        {
            UseBD = false;
        }

        public Contract(int iD, string? seller, string? buyer, int matid, float count,
            string? countUnits, float price, DateTime? date)
        {
            UseBD = false;
            ID = iD;
            Seller = seller;
            Buyer = buyer;
            MaterialID = matid;
            Count = count;
            CountUnits = countUnits;
            Price = price;
            Date = date;
            UseBD = true;
        }

        public override string ToString()
        {
            return $"Договор купли-продажи #{ID} от {DateInString}\nПокупатель: {Buyer}\nПродавец: {Seller}\nТовар \"{Material.Name}\" в количестве {Count} {CountUnits}.\nЦена за единицу ({CountUnits}): {Price}.\nСумма: {Summ}.\n\n               {Seller}\n\n               {Buyer}";
        }

        public bool IsValid => Date != null;
    }
}