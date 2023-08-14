using Microsoft.Data.Sqlite;
using Windows.Storage;
using Test.Models;
using Newtonsoft.Json;

namespace Test
{
    public class Db
    {
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("DataApi.db", CreationCollisionOption.OpenIfExists); // Crea la instancia de la Db
            string dbpath = Path.Combine(Environment.CurrentDirectory, "DataApi.db"); // Path De la Db 

            var connString = $"Filename={dbpath}";

            using (SqliteConnection db =
               new SqliteConnection(connString))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " + "EXISTS MyTable (ID INTEGER PRIMARY KEY, " + "TimeStamp DATETIME, " + "Count INT, " + "Rows TEXT)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                Console.WriteLine("Genere Base de datos");

                createTable.ExecuteReader();

                db.Close();
            }
        }
        public static List<EntryResume> GetAll()
        {
            string dbpath = Path.Combine(Environment.CurrentDirectory, "DataApi.db");

            var connString = $"Filename={dbpath}";

            using (SqliteConnection db =
                    new SqliteConnection(connString))
            {
                db.Open();

                string insertCommand = "SELECT * FROM MyTable";
                SqliteCommand insertTable = new SqliteCommand(insertCommand, db);

                List<EntryResume> entries = new List<EntryResume>();
                using (SqliteDataReader reader = insertTable.ExecuteReader())
                {
                    while (reader.Read()) // Leo lo que devolvio la db
                    {
                        int entrieID = reader.GetInt32(reader.GetOrdinal("ID"));
                        int entrieCount = reader.GetInt32(reader.GetOrdinal("Count"));
                        DateTime entrieTimestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"));
                        string rowsJson = reader.GetString(reader.GetOrdinal("Rows"));

                        List<Row> rowsList = JsonConvert.DeserializeObject<List<Row>>(rowsJson); // Json a .net type

                        EntryResume entrie = new EntryResume(entrieID, entrieCount, entrieTimestamp, rowsList); // Devuelvo las entradas via un metodo resumido

                        entries.Add(entrie);
                    }
                }

                return entries;

                db.Close();

            }
        }
        public static Entry GetInfoById(int id)
        {
            string dbpath = Path.Combine(Environment.CurrentDirectory, "DataApi.db");

            var connString = $"Filename={dbpath}";

            using (SqliteConnection db =
                    new SqliteConnection(connString))
            {
                db.Open();

                string insertCommand = "SELECT * FROM MyTable WHERE ID = @id";
                SqliteCommand insertData = new SqliteCommand(insertCommand, db);
                insertData.Parameters.AddWithValue("@id", id);

                using (SqliteDataReader reader = insertData.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int entrieID = reader.GetInt32(reader.GetOrdinal("ID"));
                        int entrieCount = reader.GetInt32(reader.GetOrdinal("Count"));
                        DateTime entrieTimestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"));
                        string rowsJson = reader.GetString(reader.GetOrdinal("Rows"));

                        List<Row> rowsList = JsonConvert.DeserializeObject<List<Row>>(rowsJson);

                        Entry entry = new Entry(entrieCount, entrieTimestamp, rowsList);

                        entry.setId(entrieID);
                        return entry;
                    }
                    return null;
                }
                db.Close();
            }
        }
        public static object DeleteInfoById(int id)
        {
            string dbpath = Path.Combine(Environment.CurrentDirectory, "DataApi.db");

            var connString = $"Filename={dbpath}";

            string selectCommand = "SELECT * FROM MyTable WHERE ID = @id";

            using (SqliteConnection db =
                    new SqliteConnection(connString))
            {
                db.Open();

                try
                {

                using (SqliteCommand selectData = new SqliteCommand(selectCommand, db))
                {
                    selectData.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader reader = selectData.ExecuteReader())
                    {
                        Entry deletedEntrie = null;

                        if (reader.Read())
                        {
                            int entrieCount = reader.GetInt32(reader.GetOrdinal("Count"));
                            DateTime entrieTimestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"));

                            string rowsJson = reader.GetString(reader.GetOrdinal("Rows"));
                            List<Row> rowsList = JsonConvert.DeserializeObject<List<Row>>(rowsJson);

                                deletedEntrie = new Entry(entrieCount, entrieTimestamp, rowsList);
                                deletedEntrie.setId(id);
                        }

                        if (deletedEntrie != null)
                        {
                            string deleteCommand = "DELETE FROM MyTable WHERE ID = @id";
                            using (SqliteCommand deleteData = new SqliteCommand(deleteCommand, db))
                            {
                                deleteData.Parameters.AddWithValue("@id", id);
                                deleteData.ExecuteNonQuery();
                            }
                        }

                        return deletedEntrie; // Devuelve el objeto eliminado o null si no se encontró
                    }
                }
                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                } finally
                {
                    db.Close();
                }
            }
        }
        public static int InsertInfo(Entry newEntrie)
        {
            string dbpath = Path.Combine(Environment.CurrentDirectory, "DataApi.db");

            var connString = $"Filename={dbpath}";

            int insertedId;

            using (SqliteConnection db =
                    new SqliteConnection(connString))
            {
                db.Open();
                string insertCommand = "INSERT INTO MyTable (TimeStamp, Count, Rows) VALUES (@TimeStamp, @Count, @Rows); SELECT last_insert_rowid()"; // Ingreso de la data a los campos
                SqliteCommand insertData = new SqliteCommand(insertCommand, db);
                insertData.Parameters.AddWithValue("@TimeStamp", newEntrie.Timestamp);
                insertData.Parameters.AddWithValue("@Count", newEntrie.Count);
                insertData.Parameters.AddWithValue("@Rows", JsonConvert.SerializeObject(newEntrie.Rows));

                insertedId = Convert.ToInt32(insertData.ExecuteScalar());

                db.Close();
            }

            return insertedId;

        }
    }
}
