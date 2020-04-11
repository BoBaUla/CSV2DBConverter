using System.Collections.Generic;
using System.Linq;

namespace CSV2DBConverter
{

    public interface ICSVEntryParser
    {
        List<CSVRow> TableEntries { get; }
        List<string> TablePattern { get; }
        void Initialize();
    }

    public class CSVEntryParser : ICSVEntryParser
    {
        private readonly string path;
        private readonly int tableLine;
        private readonly ICSVReader cSVReader;
        private readonly string[] columsToSkip;

        public List<CSVRow> TableEntries { get; private set; }

        public List<string> TablePattern { get; private set; }

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

                var filteredLineValues = SelectRowEntriesWithoutSkipedValues(indizesToSkip, lineValues);

                tableRow.Fill(TablePattern.ToArray(), filteredLineValues.ToArray());

                csvRowList.Add(tableRow);
            }

            return csvRowList;
        }

        private List<string> SelectRowEntriesWithoutSkipedValues(List<int> skipedIndizes, string[] entries)
        {
            var filteredEntries = new List<string>();

            for(var i =0; i <entries.Length; i++)
            {
                if(!skipedIndizes.Contains(i))
                {
                    filteredEntries.Add(FilterInvalidCharacters(entries[i]));
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
