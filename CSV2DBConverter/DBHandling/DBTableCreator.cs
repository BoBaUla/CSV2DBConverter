
using System.Data.SQLite;

namespace CSV2DBConverter.DBHandling
{
    public interface IDBTableCreator
    {
        void Create();
    }

    public class DBTableCreator: IDBTableCreator
    {
        private readonly IDBConnectionString connectionString;
        private readonly string[] tablePattern;
        private readonly string tableName;

        public DBTableCreator(
            IDBConnectionString connectionString,
            string[] tablePattern,
            string name)
        {
            this.connectionString = connectionString;
            this.tablePattern = tablePattern;
            this.tableName = name;
        }

        public void Create()
        {
            using (var con = new SQLiteConnection(connectionString.ConnectionString))
            {
                con.Open();
                var cmd = new SQLiteCommand(con);
                cmd.CommandText = $"Drop Table if exists {tableName}";
                cmd.ExecuteNonQuery();

                cmd.CommandText = CreateSQLStatement();
                cmd.ExecuteNonQuery();
            }
        }

        private string CreateSQLStatement()
        {
            var createTableStatment = $"CREATE TABLE IF NOT EXISTS {tableName}("
            + $" {tableName}_id integer PRIMARY KEY ";
            foreach (var field in tablePattern)
            {
                createTableStatment += $", {field} text NOT NULL ";
            }
            createTableStatment += ");";
            return createTableStatment;
        }
    }
}