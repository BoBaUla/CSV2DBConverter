using CSV2DBConverter.Adapter;
using System.Collections.Generic;
using System.Linq;

namespace CSV2DBConverter
{

    public interface ITableEntryParser
    {
        List<TableRow> TableEntries { get; }
        void Initialize();
    }

    public class TableEntryParser : ITableEntryParser
    {
        private readonly string path;
        private readonly int tableLine;
        private readonly ICSVReader cSVReader;

        public List<TableRow> TableEntries { get; private set; }

        public TableEntryParser(
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
            TableEntries = new List<TableRow>();
            cSVReader.Analyze(tableLine);
            var table = cSVReader.Table.First().Split(';');
            foreach (var line in cSVReader.Body)
            {
                var lineValues = line.Split(';');
                var tableRow = new TableRow();
                tableRow.Fill(table, lineValues);
                TableEntries.Add(tableRow);
            }
        }
    }
}
