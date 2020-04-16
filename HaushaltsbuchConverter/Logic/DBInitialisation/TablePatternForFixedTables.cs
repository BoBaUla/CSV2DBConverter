using CSV2DBConverter.CSVHandling;
using CSV2DBConverter.DBHandling;
using System.Collections.Generic;

namespace HaushaltsbuchConverter.Logic.DBInitialisation
{
    public class TablePatternForFixedTables
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
}
