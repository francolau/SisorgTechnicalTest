namespace Test.Models
{
        public class Entry
        {
            public int ID { get; set; }
            public int Count { get; set; }
            public DateTime Timestamp { get; set; }
            public List<Row> Rows { get; set; }


        public Entry(int count, DateTime timestamp, List<Row> rows)
        {
            Count = count;
            Timestamp = timestamp;
            Rows = rows;
        }
        
        public void setId(int id) { ID = id; }

        }

    public class EntryResume
    {
        public int id;
        public int count;
        public DateTime timestamp { get; set; }
        public string countries;

        public EntryResume(int entrieId, int entrieCount, DateTime entrieTimestamp, List<Row> entrieRows)
        {
            string entrieCountries = "";
            foreach(Row row in entrieRows)
            {

                entrieCountries += $"{row.Name},";

            }

            id = entrieId;
            count = entrieCount;
            timestamp = entrieTimestamp;
            countries = entrieCountries;

        }
    }

        public class Row
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public string Color { get; set; }

            public Row(string name, int value, string color)
        {
            Name = name;
            Value = value;
            Color = color;
        }
    }
 
}
