using BuildMaterials.BD;
using System.ComponentModel;

namespace BuildMaterials.Models
{
    public class Account : ITable, INotifyPropertyChanged
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
                    App.DBContext.Query($"UPDATE Accounts SET Seller='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ShipperName
        {
            get => shipperName;
            set
            {
                shipperName = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Accounts SET ShipperName='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ShipperAdress
        {
            get => shipperAdress;
            set
            {
                shipperAdress = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Accounts SET ShipperAdress='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ConsigneeName
        {
            get => consigneeName;
            set
            {
                consigneeName = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Accounts SET ConsigneeName='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ConsigneeAdress
        {
            get => consigneeAdress;
            set
            {
                consigneeAdress = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Accounts SET ConsigneeAdress='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Accounts SET Buyer='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Accounts SET CountUnits='{value}' WHERE ID={ID};");
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
                    App.DBContext.Query($"UPDATE Accounts SET Count='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Summ));
                OnPropertyChanged(nameof(TaxSumm));
                OnPropertyChanged(nameof(FinalSumm));
                OnPropertyChanged(nameof(FinalSummAtString));
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
                    App.DBContext.Query($"UPDATE Accounts SET Price='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
                OnPropertyChanged(nameof(TaxSumm));
                OnPropertyChanged(nameof(FinalSumm));
                OnPropertyChanged(nameof(FinalSummAtString));
            }
        }
        public float Summ => Count * Price;
        public float Tax
        {
            get => tax;
            set
            {
                if (value > 100 || value < 0) return;
                tax = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Accounts SET Tax='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Tax));
                OnPropertyChanged(nameof(TaxSumm));
                OnPropertyChanged(nameof(FinalSumm));
                OnPropertyChanged(nameof(FinalSummAtString));
            }
        }
        public float TaxSumm => Summ * (Tax / 100);
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                if (UseBD)
                {
                    App.DBContext.Query($"UPDATE Date SET Date='{value}' WHERE ID={ID};");
                }
            }
        }
        public float FinalSumm => Summ + TaxSumm;
        public string FinalSummAtString => "Итого: " + (Summ + TaxSumm);
        public string? DateInString => Date?.ToShortDateString();

        private DateTime? date;
        private string? countUnits = string.Empty;
        private string? buyer = string.Empty;
        private string? shipperAdress = string.Empty;
        private string? consigneeAdress = string.Empty;
        private string? consigneeName = string.Empty;
        private string? seller = string.Empty;
        private string? shipperName = string.Empty;
        private float count = 0;
        private float price = 0;
        private float tax = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Account()
        {
            UseBD = false;
        }

        public Account(int iD, string? seller, string? shipperName, string? shipperAdress, string? consigneeName, string? consigneeAdress, string? buyer, string? countUnits, float count, float price, float tax, DateTime? date)
        {
            UseBD = false;
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
            Tax = tax;
            Date = date;
            UseBD = true;
        }
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public override string ToString()
        {
            return $"Счет №{ID} от {DateInString}\nПродавец: {Seller}\nПокупатель: {Buyer}\nГрузоотправитель: {ShipperName} (адрес: {ShipperAdress})\nГрузополучатель: {ConsigneeName} (адрес: {ConsigneeAdress})\nКоличество: {Count} {CountUnits}\nЦена: {Price}\nСумма: {Summ}\nНалоговый сбор: {TaxSumm}\nИтого: {Summ + TaxSumm}";
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