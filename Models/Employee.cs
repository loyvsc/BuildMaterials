using BuildMaterials.BD;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaterials.Models
{
    public class Employee : ITable
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Pathnetic { get; set; }
        public string? Position { get; set; }
        public string? PhoneNumber { get; set; }
        public int Password { get; set; }
        public int AccessLevel { get; set; }
        [NotMapped]
        public string AccessLevelInString => App.DBContext.AccessLevel[AccessLevel];

        public Employee() { }

        public Employee(string name, string surName, string pathnetic, string position, string phoneNumber, int password = 0, int accessLevel = 3)
        {
            Name = name;
            SurName = surName;
            Pathnetic = pathnetic;
            Position = position;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessLevel = accessLevel;
        }

        public override string ToString()
        {
            return $"Сотрудник #{ID}\nФ.И.О.: {SurName} {Name} {Pathnetic}\nДолжность: {Position}\nНомер телефона: {PhoneNumber}";
        }

        public bool IsValid =>
            Name != string.Empty &&
            SurName != string.Empty &&
            Pathnetic != string.Empty &&
            Position != string.Empty &&
            PhoneNumber != string.Empty;
    }
}