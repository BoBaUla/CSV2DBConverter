using System.IO;

namespace CSV2DBConverter.DBHandling
{
    public interface IDBConnectionString
    {
        string DBPath { get; }
        string DBName { get; }

        string ConnectionString { get; }
    }

    public class DBConncetionString : IDBConnectionString
    {
        public string ConnectionString => $"DataSource = {Path.Combine(DBPath, DBName)}";

        public string DBPath { get; private set; }

        public string DBName { get; private set; }

        public DBConncetionString(string name, string path)
        {
            DBName = name;
            DBPath = path;
        }
    }
}