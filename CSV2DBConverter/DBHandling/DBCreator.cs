
using System.Data.SQLite;

namespace CSV2DBConverter.DBHandling
{

    public interface IDBCreator
    {
        void Create();
    }

    public class DBCreator : IDBCreator
    {
        private readonly IDBConnectionString dBConnectionString;

        public DBCreator(IDBConnectionString dBConnectionString)
        {
            this.dBConnectionString = dBConnectionString;
        }

        public void Create()
        {
            using (var con = new SQLiteConnection(dBConnectionString.ConnectionString))
            {
                con.Open();
            }
        }
    }

}