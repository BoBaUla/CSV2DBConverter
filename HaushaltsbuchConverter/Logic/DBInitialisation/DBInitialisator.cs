using CSV2DBConverter.Adapter;
using CSV2DBConverter.CSVHandling;
using CSV2DBConverter.DBHandling;
using System.Collections.Generic;

namespace HaushaltsbuchConverter.Logic.DBInitialisation
{
    public class DBInitialisator
    {
        IDBConnectionString _connectionString;
        TablePatternForFixedTables _tables;
        TableExtractor _tableExtractor;

        public DBInitialisator(string dbName, string cwd)
        {
            _connectionString = new DBConncetionString(dbName, cwd);
            _tables = new TablePatternForFixedTables();
            _tableExtractor = new TableExtractor();
        }

        public void Initialise()
        {
            var dbCreator = new DBCreator(_connectionString);
            dbCreator.Create();
        }

        public void CreateTables(string filename)
        {
            CreateSubTable(_tables.LabelsTable, _tables.LabelsTableName);
            CreateSubTable(_tables.RecipientTable, _tables.RecipientTableName);
            CreateSubTable(_tables.DateTable, _tables.DateTableName);
            CreateSubTable(_tables.HashTable, _tables.HashTableName);
            CreateSubTable(_tables.BookingTable, _tables.BookingTableName);
            FillTable(_tables.LabelsValues, _tables.LabelsTableName, _tables.LabelsTable);
            FillTable(_tables.BookingValues, _tables.BookingTableName, _tables.BookingTable);

            CreateKostenTable(filename);
        }

        private void CreateSubTable(List<CSVTableAttribute> tablePattern, string tableName)
        {
            var dbTableCreator = new DBTableCreator(
                            _connectionString,
                            tablePattern,
                            tableName);

            dbTableCreator.Create();
        }

        private void FillTable(List<string> values, string tableName, List<CSVTableAttribute> tableAttributes)
        {
            foreach(var value in values)
            {
                var csvRow = new CSVRow();
                csvRow.Fill(tableAttributes, new string[] {value});

                var dbInsertCommand = new InsertCommand(
                    _connectionString,
                    tableName,
                    csvRow);
                dbInsertCommand.Insert();
            }            
        }

        private void CreateKostenTable(string filename)
        {
            var csvEntryParser = CreateCSVEntryParser(filename);
            csvEntryParser.Initialize();

            var tablePattern = csvEntryParser.TablePattern;
            AdjustForeignKeys(tablePattern);

            var dbTableCreator = new DBTableCreator(
                           _connectionString,
                           tablePattern,
                           _tables.KostenTableName);

            dbTableCreator.CreateWithForeignKeys(_tables.ForeignKeys);
        }

        private void AdjustForeignKeys(List<CSVTableAttribute> tablePattern)
        {
            foreach (var entry in tablePattern)
            {
                if (_tableExtractor.ExtractedTables.Contains(entry.AttributeName))
                {
                    entry.IsForeignKey = true;
                    entry.ForeignKeyTable = _tableExtractor.TableMap[entry.AttributeName];
                }
            }
        }

        private CSVReader CreateCSVReader(string filename)
        {
            return new CSVReader(filename, new FileReader());
        }

        private CSVEntryParser CreateCSVEntryParser(string filename)
        {
            CSVReader csvReader = CreateCSVReader(filename);
            return new CSVEntryParser(
                filename,
                _tables.TableHeadLine,
                csvReader,
                _tables.ExcludedFieldsFromKostenTable);
        }
    }
}
