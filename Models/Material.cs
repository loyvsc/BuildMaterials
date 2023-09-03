using BuildMaterials.BD;
using System;
using System.ComponentModel.DataAnnotations;

namespace BuildMaterials.Models
{
    public class Material : ITable
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public float Price { get; set; }
        public float Count { get; set; }
        public string? CountUnits { get; set; }
        public DateTime EnterDate { get; set; }
        public float EnterCount { get; set; }

        public string AsString()
        {
            return $"Материал #{ID}\nНаименование: {Name}\nПроизводитель: {Manufacturer}\nПоступление {EnterDate} в количестве {EnterCount}\nКоличетсво: {Count} {CountUnits}\nЦена: {Price}";
        }

        public Material()
        {
            Name = string.Empty;
            Manufacturer = string.Empty;
            Price = 0;
            Count = 0;
            CountUnits = string.Empty;
        }

        public Material(int id)
        {
            ID = id;
        }

        public Material(int id, string name, string manufacturer, float price, float count, string countUnits, DateTime enterDate, int enterCount)
        {
            ID = id;
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            Count = count;
            CountUnits = countUnits;
            EnterDate = enterDate;
            EnterCount = enterCount;
        }

        public bool IsValid => Name != string.Empty &&
            Manufacturer != string.Empty &&
            CountUnits != string.Empty;

        public override string ToString()
        {
            return Name!;
        }
    }
}