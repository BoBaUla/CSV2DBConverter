using CSV2DBConverter.Adapter;
using CSV2DBConverter.CSVHandling;
using CSV2DBConverter.DBHandling;
using System.Collections.Generic;

namespace HaushaltsbuchConverter.Logic.DBInitialisation
{
    public class TablePatternForFixedTables
    {
        private static string _KostenTableName = "KostenTable";
        public string KostenTableName = _KostenTableName;
        public string[] ExcludedFieldsFromKostenTable = { "Währung" };
        public int TableHeadLine = 14;

        private static string _HashTableName = "HashesTable";
        public string HashTableName = "HashesTable";
        public List<CSVTableAttribute> HashTable = new List<CSVTableAttribute>()
        {
            new CSVTableAttribute("Hash")
        };

        private static string _LabelsTableName = "LabelsTable";
        public string LabelsTableName = "LabelsTable";
        public List<CSVTableAttribute> LabelsTable = new List<CSVTableAttribute>()
        {
            new CSVTableAttribute("Label")
        };

        public List<string> LabelsValues = new List<string>()
        {
            "Arzt",
            "Auto 1",
            "Auto 2",
            "Bahn",
            "Bargeld",
            "Büromaterial",
            "Drogerie",
            "Einkauf Lebensmittel",
            "Einlage Elke",
            "Einlage Boris",
            "Freizeit",
            "Geschenke",
            "Haus und Garten",
            "Hobby",
            "Kleidung",
            "Malte",
            "Nebenkosten"
        };

        public List<ForeignKey> ForeignKeys = new List<ForeignKey>
        {
            new ForeignKey
            {
                Key = _HashTableName + "_id",
                Table = _HashTableName,
                TableID = _HashTableName + "_id"
            },
            new ForeignKey
            {
                Key = _LabelsTableName + "_id",
                Table = _LabelsTableName,
                TableID = _LabelsTableName + "_id"
            }
        };
    }


    public class DBInitialisator
    {
        IDBConnectionString _connectionString;
        TablePatternForFixedTables _tables;

        public DBInitialisator(string dbName, string cwd)
        {
            _connectionString = new DBConncetionString(dbName, cwd);
            _tables = new TablePatternForFixedTables();
        }

        public void Initialise()
        {
            var dbCreator = new DBCreator(_connectionString);
            dbCreator.Create();
        }

        public void CreateTables(string filename)
        {
            CreateHashesTable();
            CreateLabelsTable();
            FillLabelsTable();
            CreateKostenTable(filename);
        }

        private void CreateHashesTable()
        {
            var dbTableCreator = new DBTableCreator(
                            _connectionString,
                            _tables.HashTable,
                            _tables.HashTableName);

            dbTableCreator.Create();
        }

        private void CreateLabelsTable()
        {
            var dbTableCreator = new DBTableCreator(
                            _connectionString,
                            _tables.LabelsTable,
                            _tables.LabelsTableName);

            dbTableCreator.Create();
        }

        private void FillLabelsTable()
        {
            foreach(var value in _tables.LabelsValues)
            {
                var csvRow = new CSVRow();
                csvRow.Fill(_tables.LabelsTable, new string[] {value});

                var dbInsertCommand = new InsertCommand(
                    _connectionString,
                    _tables.LabelsTableName,
                    csvRow);
                dbInsertCommand.Insert();
            }            
        }

        private void CreateKostenTable(string filename)
        {
            var csvEntryParser = CreateCSVEntryParser(filename);
            csvEntryParser.Initialize();

            var dbTableCreator = new DBTableCreator(
                           _connectionString,
                           csvEntryParser.TablePattern,
                           _tables.KostenTableName);

            dbTableCreator.CreateWithForeignKeys(_tables.ForeignKeys);
        }

        private CSVReader CrearteCSVReader(string filename)
        {
            return new CSVReader(filename, new FileReader());
        }

        private CSVEntryParser CreateCSVEntryParser(string filename)
        {
            CSVReader csvReader = CrearteCSVReader(filename);
            return new CSVEntryParser(
                filename,
                _tables.TableHeadLine,
                csvReader,
                _tables.ExcludedFieldsFromKostenTable);
        }
    }
}
