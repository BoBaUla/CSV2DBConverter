using CSV2DBConverter.CSVHandling;
using System.Data.SQLite;

namespace CSV2DBConverter.DBHandling
{
    public interface IInsertCommand
    {
        void Insert();
    }

    public class InsertCommand : IInsertCommand
    {
        private readonly IDBConnectionString connectionString;
        private readonly string tableName;
        private readonly CSVRow csvEntries;

        public InsertCommand(
            IDBConnectionString connectionString, 
            string tableName, 
            CSVRow csvEntries)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
            this.csvEntries = csvEntries;
        }

        public void Insert()
        {
            using (var con = new SQLiteConnection(connectionString.ConnectionString))
            {
                con.Open();
                var cmd = new SQLiteCommand(con);
                
                cmd.CommandText = CreateInsertCommand();
                cmd.ExecuteNonQuery();
            }
        }

        private string CreateInsertCommand()
        {
            var command = $"Insert into {tableName}(";
            var entries = csvEntries.Row;
            var entriesWithoutLastOne = csvEntries.Row.Count - 1;
            for (var entry = 0; entry < entriesWithoutLastOne; entry++)
            {
                command += entries[entry].Key + ",";
            }
            command += entries[entriesWithoutLastOne].Key + ")";

            command += " Values(";
            for (var entry = 0; entry < entriesWithoutLastOne; entry++)
            {
                command += "'" + entries[entry].Value + "'" + ",";
            }
            command += "'" + entries[entriesWithoutLastOne].Value + "'";
            command += ");";
            return command;
        }
    }

}