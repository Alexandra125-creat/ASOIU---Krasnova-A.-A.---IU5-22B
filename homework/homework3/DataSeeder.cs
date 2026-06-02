namespace Homework3.Models
{
    /// <summary>Заполнение БД начальными данными из CSV</summary>
    public static class DataSeeder
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Publishers.Any() || context.Journals.Any())
                return;

            string publishersPath = Path.Combine(AppContext.BaseDirectory, "Data", "publishers.csv");
            string journalsPath = Path.Combine(AppContext.BaseDirectory, "Data", "journals.csv");

            LoadPublishersFromCsv(context, publishersPath);
            LoadJournalsFromCsv(context, journalsPath);
        }

        private static void LoadPublishersFromCsv(AppDbContext context, string filePath)
        {
            if (!File.Exists(filePath))
            {
                AddDefaultPublishers(context);
                return;
            }

            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var parts = lines[i].Split(';');
                if (parts.Length < 2) continue;

                if (int.TryParse(parts[0], out int id) && !string.IsNullOrWhiteSpace(parts[1]))
                {
                    context.Publishers.Add(new Publisher { Id = id, Name = parts[1].Trim() });
                }
            }
            context.SaveChanges();
        }

        private static void LoadJournalsFromCsv(AppDbContext context, string filePath)
        {
            if (!File.Exists(filePath))
            {
                AddDefaultJournals(context);
                return;
            }

            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var parts = lines[i].Split(';');
                if (parts.Length < 4) continue;

                if (int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[1], out int pubId) &&
                    int.TryParse(parts[3], out int circulation) &&
                    !string.IsNullOrWhiteSpace(parts[2]))
                {
                    context.Journals.Add(new Journal
                    {
                        Id = id,
                        PublisherId = pubId,
                        Name = parts[2].Trim(),
                        CirculationK = circulation
                    });
                }
            }
            context.SaveChanges();
        }

        private static void AddDefaultPublishers(AppDbContext context)
        {
            var publishers = new[]
            {
                new Publisher { Id = 1, Name = "Science" },
                new Publisher { Id = 2, Name = "Education" },
                new Publisher { Id = 3, Name = "Sport" },
                new Publisher { Id = 4, Name = "Domino" }
            };
            context.Publishers.AddRange(publishers);
            context.SaveChanges();
        }

        private static void AddDefaultJournals(AppDbContext context)
        {
            var journals = new[]
            {
                new Journal { Id = 101, PublisherId = 1, Name = "Quantum", CirculationK = 45 },
                new Journal { Id = 102, PublisherId = 1, Name = "Nature", CirculationK = 20 },
                new Journal { Id = 103, PublisherId = 2, Name = "Technic", CirculationK = 55 },
                new Journal { Id = 104, PublisherId = 2, Name = "Mumu", CirculationK = 80 },
                new Journal { Id = 105, PublisherId = 3, Name = "Earth", CirculationK = 120 },
                new Journal { Id = 106, PublisherId = 3, Name = "Yuang", CirculationK = 60 },
                new Journal { Id = 107, PublisherId = 4, Name = "For car", CirculationK = 200 },
                new Journal { Id = 108, PublisherId = 4, Name = "All for mum", CirculationK = 150 },
                new Journal { Id = 109, PublisherId = 1, Name = "Helth", CirculationK = 30 },
                new Journal { Id = 110, PublisherId = 2, Name = "About sport", CirculationK = 90 },
                new Journal { Id = 111, PublisherId = 3, Name = "Phisics everywhere", CirculationK = 40 },
                new Journal { Id = 112, PublisherId = 4, Name = "Time to live", CirculationK = 110 }
            };
            context.Journals.AddRange(journals);
            context.SaveChanges();
        }
    }
}