namespace Homework2
{
    public class Journal
    {
        public int Id { get; set; }
        public int PublisherId { get; set; }
        public string Name { get; set; }
        private int _circulationK;
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
        public Journal(int id, int publisherId, string name, int circulationK)
        {
            Id = id;
            PublisherId = publisherId;
            Name = name;
            CirculationK = circulationK;  
        }
        public Journal() : this(0, 0, "", 0) { }
        public override string ToString() => $"[{Id}] {Name}, изд-во #{PublisherId}, тираж: {CirculationK} тыс. экз.";
    }
}
