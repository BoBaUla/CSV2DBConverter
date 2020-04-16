using CSV2DBConverter.Adapter;
using CSV2DBConverter.CSVHandling;
using CSV2DBConverter.DBHandling;
using HaushaltsbuchConverter.Logic.DBInitialisation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaushaltsbuchConverter.Logic.DBFilling
{
    public class DBFiller
    {
        IDBConnectionString _connectionString;
        TablePatternForFixedTables _tables;
        CSVEntryParser _csvEntryParser;
        TableExtractor _tableExtractor;

        public DBFiller(string dbName, string cwd, string filename)
        {
            _connectionString = new DBConncetionString(dbName, cwd);
            _tables = new TablePatternForFixedTables();
            _csvEntryParser = CreateCSVEntryParser(filename);
            _tableExtractor = new TableExtractor();
        }

        public void Fill()
        {
            _csvEntryParser.Initialize();
            var csvTablePattern = _csvEntryParser.TablePattern;
            AdjustForeignKeys(csvTablePattern);

            var csvRows = _csvEntryParser.TableEntries;

            foreach (var csvRow in csvRows)
            {
                var hash = csvRow.GetHashCode();
                var savedHashes = ReadFromDB(_tables.HashTableName, _tables.HashTable);
                if (savedHashes.Contains(hash.ToString()))
                {
                    continue;
                }
                InsertHash(hash);

                var dateValue = GetDateValue(csvRow);
                var savedDates = ReadFromDB(_tables.DateTableName, _tables.DateTable);
                if (!savedDates.Contains(dateValue))
                {
                    InsertIntoSubtable(dateValue, _tables.DateTableName, _tables.DateTable);
                }
                
                var recipientValue = GetRecipientValue(csvRow);
                var savedRecipients = ReadFromDB(_tables.RecipientTableName, _tables.RecipientTable);
                if(!savedRecipients.Contains(recipientValue))
                {
                    InsertIntoSubtable(recipientValue, _tables.RecipientTableName, _tables.RecipientTable);
                }
            }
        }

        private List<string> ReadFromDB(string tableName, List<CSVTableAttribute> tablePattern)
        {
            var dbReadCommand = new RequestCommand(
                 _connectionString,
                 tableName,
                 tablePattern);
            dbReadCommand.RequestAllTable();

            var values = dbReadCommand.Values.Select(m => m.Value).ToList();

            if (!values.Any())
                return new List<string>();
            return values.Select(m => m.First().Value).ToList();
        }

        void InsertIntoSubtable(string value, string tableName, List<CSVTableAttribute> tablePattern)
        {
            var valuRowe = new CSVRow();
            valuRowe.Fill(tablePattern, new string[] { value });

            InsertCommand insertCommand = new InsertCommand(
                _connectionString,
                tableName,
                valuRowe);
            insertCommand.Insert();
        }
        
        private string GetRecipientValue(CSVRow csvRow)
        {
            return csvRow.Row.Where(FilterRecipient).First().Value;
        }

        private bool FilterRecipient(SCSVEntry entry)
        {
            return entry.Key == TableExtractor._RecipientExtractName;
        }
        
        private bool FilterBooking(SCSVEntry entry)
        {
            return entry.Key == TableExtractor._BookingTextExtractName;

        }

        private string GetDateValue(CSVRow csvRow)
        {
            return csvRow.Row.Where(FilterDate).First().Value;
        }

        private bool FilterDate(SCSVEntry entry)
        {
            return entry.Key == TableExtractor._DateExtractName;
        }

        private void InsertHash(int hash)
        {
            var hashedValue = new CSVRow();
            hashedValue.Fill(_tables.HashTable, new string[] { hash.ToString() });
            InsertCommand dbInsertHashCommand = CreateInsertHashCMD(hashedValue);
            dbInsertHashCommand.Insert();
        }

        private InsertCommand CreateInsertHashCMD(CSVRow hashedValue)
        {
            return new InsertCommand(
                _connectionString,
                _tables.HashTableName,
                hashedValue);
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
