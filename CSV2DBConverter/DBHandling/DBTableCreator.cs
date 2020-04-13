using CSV2DBConverter.CSVHandling;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace CSV2DBConverter.DBHandling
{
    public struct ForeignKey
    {
        public string Key;
        public string Table;
        public string TableID;
    }
         
    public interface IDBTableCreator
    {
        void Create();
        void CreateWithForeignKeys(List<ForeignKey> keyList);
    }

    public class DBTableCreator: IDBTableCreator
    {
        private readonly IDBConnectionString connectionString;
        private readonly IList<CSVTableAttribute> tablePattern;
        private readonly string tableName;

        public DBTableCreator(
            IDBConnectionString connectionString,
            IList<CSVTableAttribute> tablePattern,
            string name)
        {
            this.connectionString = connectionString;
            this.tablePattern = tablePattern;
            this.tableName = name;
        }

        public void Create()
        {
            CreateInternal(CreateSQLStatement());
        }
        
        public void CreateWithForeignKeys(List<ForeignKey> keyList)
        {
            CreateInternal(CreateSQLStatementWithForeignKey(keyList));
        }

        private void CreateInternal(string command)
        {
            using (var con = new SQLiteConnection(connectionString.ConnectionString))
            {
                con.Open();
                var cmd = new SQLiteCommand(con);
                cmd.CommandText = $"Drop Table if exists {tableName}";
                cmd.ExecuteNonQuery();

                cmd.CommandText = FinishStatement(command);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private string CreateSQLStatement()
        {
            var createTableStatment = $"CREATE TABLE IF NOT EXISTS {tableName}("
            + $" {tableName}_id integer PRIMARY KEY ";
            foreach (var field in tablePattern)
            {
                if(!field.IsForeignKey)
                    createTableStatment += $", {field.AttributeName} text NOT NULL ";
            }

            return createTableStatment;
        }

        private string CreateSQLStatementWithForeignKey(List<ForeignKey> keyList)
        {
            var createTableStatement = CreateSQLStatement();
            foreach (var key in tablePattern.Where(m => m.IsForeignKey))
            {
                createTableStatement += $", {key.ForeignKeyID} integer ";
            }

            foreach (var key in keyList)
            {
                createTableStatement += $", {key.Key} integer ";
            }
            
            foreach (var field in tablePattern.Where(m => m.IsForeignKey))
            {
                createTableStatement += $", FOREIGN KEY({field.ForeignKeyID}) REFERENCES {field.ForeignKeyTable}({field.ForeignKeyID})";
            }

            foreach (var key in keyList)
            {
                createTableStatement += $", FOREIGN KEY({key.Key}) REFERENCES {key.Table}({key.TableID})";
            }
            return createTableStatement;
        }

        private static string FinishStatement(string createTableStatment)
        {
            createTableStatment += ");";
            return createTableStatment;
        }
    }   
}