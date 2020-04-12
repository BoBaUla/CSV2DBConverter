using CSV2DBConverter.CSVHandling;
using System.Collections.Generic;
using System.Linq;

namespace CSV2DBConverter.CSVHandling
{

    public interface ICSVEntryParser
    {
        List<CSVRow> TableEntries { get; }
        List<CSVTableAttribute> TablePattern { get; }
        void Initialize();
    }

    public class CSVEntryParser : ICSVEntryParser
    {
        private readonly string path;
        private readonly int tableLine;
        private readonly ICSVReader cSVReader;
        private readonly string[] columsToSkip;

        public List<CSVRow> TableEntries { get; private set; }

        public List<CSVTableAttribute> TablePattern { get; private set; }

    public CSVEntryParser(
            string path, 
            int tableLine,
            ICSVReader cSVReader,
            string[] columsToSkip)
        {
            this.path = path;
            this.tableLine = tableLine;
            this.cSVReader = cSVReader;
            this.columsToSkip = columsToSkip;
        }

        public void Initialize()
        {
            TableEntries = new List<CSVRow>();
            cSVReader.Analyze(tableLine);
            var table = cSVReader.Table.First().Split(';');
            var indizesToSkip = FilterColumnsToSkip(table);

            TablePattern = SelectRowEntriesWithoutSkipedValues(indizesToSkip, table);
            TableEntries = SelectTableEntriesWithoutSkippedValues(indizesToSkip);
        }



        private string FilterInvalidCharacters(string value)
        {
            return value.Replace("/", "_").Replace("'","''");
        }

        private List<CSVRow> SelectTableEntriesWithoutSkippedValues(List<int> indizesToSkip)
        {
            var csvRowList = new List<CSVRow>();

            foreach (var line in cSVReader.Body)
            {
                var lineValues = line.Split(';');
                var tableRow = new CSVRow();

                var filteredLineValues = SelectRowEntriesWithoutSkipedValues(indizesToSkip, lineValues).Select(m => m.AttributeName);

                tableRow.Fill(TablePattern, filteredLineValues.ToArray());

                csvRowList.Add(tableRow);
            }

            return csvRowList;
        }

        private List<CSVTableAttribute> SelectRowEntriesWithoutSkipedValues(List<int> skipedIndizes, string[] entries)
        {
            var filteredEntries = new List<CSVTableAttribute>();

            for(var i =0; i <entries.Length; i++)
            {
                if(!skipedIndizes.Contains(i))
                {
                    filteredEntries.Add(new CSVTableAttribute
                    {
                        AttributeName = FilterInvalidCharacters(entries[i]),
                        IsForeignKey = false
                    });                        
                }
            }
            return filteredEntries;
        }

        private List<int> FilterColumnsToSkip(string[] columns)
        {
            var indizes = new List<int>();
            for (var i = 0; i < columns.Length; i++)
            {
                if (columsToSkip.Contains(columns[i]))
                {
                    indizes.Add(i);
                }
            }
            return indizes;
        }


    }
}
