using System.ComponentModel.DataAnnotations;

namespace Homework3.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Journal> Journals { get; set; } = new List<Journal>();

        public Publisher() { }

        public Publisher(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"{Name}";
    }
}