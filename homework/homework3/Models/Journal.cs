using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework3.Models
{
    /// <summary>Журнал (основная таблица)</summary>
    public class Journal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }

        public string Name { get; set; } = string.Empty;

        private int _circulationK;

        /// <summary>Тираж (тыс. экз.) - не может быть отрицательным</summary>
        public int CirculationK
        {
            get => _circulationK;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Тираж не может быть отрицательным!");
                _circulationK = value;
            }
        }

        /// <summary>Навигационное свойство: издательство журнала</summary>
        public virtual Publisher? Publisher { get; set; }

        public Journal() { }

        public Journal(int id, int publisherId, string name, int circulationK)
        {
            Id = id;
            PublisherId = publisherId;
            Name = name;
            CirculationK = circulationK;
        }

        public override string ToString() => $"{Name} (тираж: {CirculationK} тыс. экз.)";
    }
}