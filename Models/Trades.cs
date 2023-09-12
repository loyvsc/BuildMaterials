using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Trade : ITable
    {
        private readonly bool UseBD;

        public int ID { get; set; }
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE trades SET Date='{date!.Value.Year}-{date!.Value.Month}-{date!.Value.Day}' WHERE ID = {ID};");
                }
            }
        }
        public string? DateInString
        {
            get=> Date?.ToShortDateString();
            set => Date = Convert.ToDateTime(value);
        }
        public string? SellerFio
        {
            get => sellerFio;
            set
            {
                sellerFio = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE trades SET SellerFio='{value}' WHERE ID = {ID};");
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
                    App.DBContext.Query($"UPDATE trades SET MaterialName='{value}' WHERE ID = {ID};");
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
                    App.DBContext.Query($"UPDATE trades SET Count='{value}' WHERE ID = {ID};");
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
                    App.DBContext.Query($"UPDATE trades SET Price='{value}' WHERE ID = {ID};");
                }
            }
        }

        public float Summ => Count * Price;
        
        private string? sellerFio = string.Empty;
        private string? materialName = string.Empty;
        private float count = 0;
        private float price = 0;
        private DateTime? date;

        public Trade()
        {
            UseBD = false;
        }

        public Trade(int iD, DateTime? date, string sellerFio, string materialName, float count, float price)
        {
            UseBD = true;
            this.ID = iD;
            this.date = date;
            this.sellerFio = sellerFio;
            this.materialName = materialName;
            this.count = count;
            this.price = price;
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