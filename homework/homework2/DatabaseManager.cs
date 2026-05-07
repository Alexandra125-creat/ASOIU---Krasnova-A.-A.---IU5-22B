using Microsoft.Data.Sqlite;

namespace Homework2
{
    class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS publisher (
                    publisher_id INTEGER PRIMARY KEY,
                    publisher_name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS journal (
                    journal_id INTEGER PRIMARY KEY,
                    publisher_id INTEGER NOT NULL,
                    journal_name TEXT NOT NULL,
                    circulation_k INTEGER NOT NULL,
                    FOREIGN KEY (publisher_id) REFERENCES publisher(publisher_id)
                );";
            command.ExecuteNonQuery();
        }

        public void ImportFromCsv(string publishersPath, string journalsPath)
        {
            ImportPublishersFromCsv(publishersPath);
            ImportJournalsFromCsv(journalsPath);
        }

        private void ImportPublishersFromCsv(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл {path} не найден!");
                return;
            }

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM publisher";
            long count = (long)checkCmd.ExecuteScalar();

            if (count > 0)
                return;

            var lines = File.ReadAllLines(path);

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var parts = lines[i].Split(';');

                if (parts.Length < 2)
                    continue;

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO publisher (publisher_id, publisher_name) VALUES (@id, @name)";
                command.Parameters.AddWithValue("@id", int.Parse(parts[0]));
                command.Parameters.AddWithValue("@name", parts[1]);
                command.ExecuteNonQuery();
            }
        }

        private void ImportJournalsFromCsv(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл {path} не найден!");
                return;
            }

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM journal";
            long count = (long)checkCmd.ExecuteScalar();

            if (count > 0)
                return;

            var lines = File.ReadAllLines(path);

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var parts = lines[i].Split(';');

                if (parts.Length < 4)
                    continue;

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO journal (journal_id, publisher_id, journal_name, circulation_k) 
                    VALUES (@id, @pubId, @name, @circ)";
                command.Parameters.AddWithValue("@id", int.Parse(parts[0]));
                command.Parameters.AddWithValue("@pubId", int.Parse(parts[1]));
                command.Parameters.AddWithValue("@name", parts[2]);
                command.Parameters.AddWithValue("@circ", int.Parse(parts[3]));
                command.ExecuteNonQuery();
            }
        }

        public List<Publisher> GetAllPublishers()
        {
            var result = new List<Publisher>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT publisher_id, publisher_name FROM publisher ORDER BY publisher_id";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Publisher(reader.GetInt32(0), reader.GetString(1)));
            }
            return result;
        }

        public List<Journal> GetAllJournals()
        {
            var result = new List<Journal>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT journal_id, publisher_id, journal_name, circulation_k FROM journal ORDER BY journal_id";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Journal(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetInt32(3)
                ));
            }
            return result;
        }

        public Journal? GetJournalById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT journal_id, publisher_id, journal_name, circulation_k FROM journal WHERE journal_id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Journal(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetInt32(3)
                );
            }
            return null;
        }

        public void AddJournal(Journal journal)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO journal (publisher_id, journal_name, circulation_k)
                VALUES (@pubId, @name, @circ)";
            command.Parameters.AddWithValue("@pubId", journal.PublisherId);
            command.Parameters.AddWithValue("@name", journal.Name);
            command.Parameters.AddWithValue("@circ", journal.CirculationK);
            command.ExecuteNonQuery();
        }

        public void UpdateJournal(Journal journal)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE journal 
                SET publisher_id = @pubId, journal_name = @name, circulation_k = @circ
                WHERE journal_id = @id";
            command.Parameters.AddWithValue("@id", journal.Id);
            command.Parameters.AddWithValue("@pubId", journal.PublisherId);
            command.Parameters.AddWithValue("@name", journal.Name);
            command.Parameters.AddWithValue("@circ", journal.CirculationK);
            command.ExecuteNonQuery();
        }

        public void DeleteJournal(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM journal WHERE journal_id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public (string[] columns, List<string[]> rows) ExecuteQuery(string sql)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sql;

            using var reader = command.ExecuteReader();

            var columns = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns[i] = reader.GetName(i);
            }

            var rows = new List<string[]>();
            while (reader.Read())
            {
                var row = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader.GetValue(i)?.ToString() ?? "";
                }
                rows.Add(row);
            }
            return (columns, rows);
        }
    }
}
