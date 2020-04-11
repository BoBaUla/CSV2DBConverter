using System.Collections.Generic;
using System.Linq;

namespace CSV2DBConverter
{

    public interface ICSVEntryParser
    {
        List<CSVRow> TableEntries { get; }
        void Initialize();
    }

    public class CSVEntryParser : ICSVEntryParser
    {
        private readonly string path;
        private readonly int tableLine;
        private readonly ICSVReader cSVReader;

        public List<CSVRow> TableEntries { get; private set; }

        public CSVEntryParser(
            string path, 
            int tableLine,
            ICSVReader cSVReader)
        {
            this.path = path;
            this.tableLine = tableLine;
            this.cSVReader = cSVReader;
        }

        public void Initialize()
        {
            TableEntries = new List<CSVRow>();
            cSVReader.Analyze(tableLine);
            var table = cSVReader.Table.First().Split(';');
            foreach (var line in cSVReader.Body)
            {
                var lineValues = line.Split(';');
                var tableRow = new CSVRow();
                tableRow.Fill(table, lineValues);
                TableEntries.Add(tableRow);
            }
        }
    }
}
