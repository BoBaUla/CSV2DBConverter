using CSV2DBConverter.Adapter;
using CSV2DBConverter.CSVHandling;
using CSV2DBConverter.DBHandling;
using System.Collections.Generic;

namespace HaushaltsbuchConverter.Logic.DBInitialisation
{
    internal class TablePatternForFixedTables
    {
        public readonly static string _KostenTableName = "KostenTable";
        public string KostenTableName = _KostenTableName;
        public string[] ExcludedFieldsFromKostenTable = { "Währung", "Valuta" };
        public int TableHeadLine = 14;

        public readonly static string _HashTableName = "HashesTable";
        public string HashTableName = "HashesTable";
        public List<CSVTableAttribute> HashTable = new List<CSVTableAttribute>
        {
            new CSVTableAttribute("Hash")
        };

        public readonly static string _DateTableName = "DatumTable";
        public string DateTableName = _DateTableName;
        public List<CSVTableAttribute> DateTable = new List<CSVTableAttribute>
        {
            new CSVTableAttribute("Date")
        };

        public readonly static string _RecipientTableName = "EmpfängerTable";
        public string RecipientTableName = _RecipientTableName;
        public List<CSVTableAttribute> RecipientTable = new List<CSVTableAttribute>
        {
            new CSVTableAttribute("Empfänger")
        };

        public readonly static string _BookingTableName = "BuchungsTable";
        public string BookingTableName = _BookingTableName;
        public List<CSVTableAttribute> BookingTable = new List<CSVTableAttribute>
        {
            new CSVTableAttribute("Buchung")
        };
        public List<string> BookingValues = new List<string>()
        {
            "Gutschrift",
            "Lastschrift",
            "Dauerauftrag",
            "Überweisung"
        };

        public readonly static string _LabelsTableName = "LabelsTable";
        public string LabelsTableName = "LabelsTable";
        public List<CSVTableAttribute> LabelsTable = new List<CSVTableAttribute>()
        {
            new CSVTableAttribute("Label")
        };
        public List<string> LabelsValues = new List<string>()
        {
            "Arzt",
            "Auto",
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
            CreateForeignKey(_HashTableName),
            CreateForeignKey(_LabelsTableName)
        };

        private static ForeignKey CreateForeignKey(string key)
        {
            return new ForeignKey
            {
                Key = key + "_id",
                Table = key,
                TableID = key + "_id"
            };
        }
    }

    internal class TableExtractor
    {
        private static readonly string _DateExtractName = "Buchung";
        private static readonly string _RecipientExtractName = "Auftraggeber_Empfänger";
        private static readonly string _BookingTextExtractName = "Buchungstext";

        public List<string> ExtractedTables = new List<string>
        {
            _DateExtractName, 
            _RecipientExtractName,
            _BookingTextExtractName
        };

        public Dictionary<string, string> TableMap = new Dictionary<string, string>
        {
            { _DateExtractName, TablePatternForFixedTables._DateTableName },
            { _RecipientExtractName, TablePatternForFixedTables._RecipientTableName },
            { _BookingTextExtractName, TablePatternForFixedTables._BookingTableName },

        };
    }

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
