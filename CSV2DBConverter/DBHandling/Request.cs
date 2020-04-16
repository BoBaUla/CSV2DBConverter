using CSV2DBConverter.CSVHandling;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace CSV2DBConverter.DBHandling
{
    public interface IRequestCommand
    {
        Dictionary<int, List<SCSVEntry>> Values { get; }
        void RequestAllTable();
    }

    public class RequestCommand: IRequestCommand
    {
        private readonly IDBConnectionString connectionString;
        private readonly string tableName;
        private readonly List<CSVTableAttribute> tablePattern;

        public RequestCommand(
            IDBConnectionString connectionString,
            string tableName,
            List<CSVTableAttribute> tablePattern)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
            this.tablePattern = tablePattern;
        }

        public Dictionary<int, List<SCSVEntry>> Values { get; private set; } = new Dictionary<int, List<SCSVEntry>>();

        public void RequestAllTable()
        {
            using (var con = new SQLiteConnection(connectionString.ConnectionString))
            {
                con.Open();
                
                var cmd = new SQLiteCommand(con);

                cmd.CommandText = CreateRequestCommand();
                var dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    var id = dataReader.GetInt32(0);
                    List<SCSVEntry> values = new List<SCSVEntry>();
                    for(var attribute = 0; attribute < tablePattern.Count(); attribute++)
                    {
                        var key = tablePattern[attribute].AttributeName;
                        var value = dataReader.GetString(attribute + 1);
                        values.Add(new SCSVEntry { Key = key, Value = value });
                    }
                    Values.Add(id, values);
                }
            }
        }

        private string CreateRequestCommand()
        {
            var commandStatement = $"Select * from {tableName};";

            return commandStatement;
        }
    }
}
