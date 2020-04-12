using CSV2DBConverter.Adapter;
using System.Linq;

namespace CSV2DBConverter.CSVHandling
{
    public interface ICSVReader
    {
        string[] Head { get; }
        string[] Table { get; }
        string[] Body { get; }

        void Analyze(int tableLine);
    }

    public class CSVReader : ICSVReader
    {
        private readonly string path;
        private readonly IFileReader fileReader;
        private string[] file;

        public string[] Head { get; private set; }
        public string[] Table { get; private set; }
        public string[] Body { get; private set; }

        public CSVReader(
            string path, 
            IFileReader fileReader)
        {
            this.path = path;
            this.fileReader = fileReader;
        }

        public void Analyze(int tableLine)
        {
            file = fileReader.Read(path);
            Head = file.Take(tableLine).ToArray();
            Table = file.Skip(tableLine).Take(1).ToArray();
            Body = file.Skip(tableLine + 1).ToArray();
        }
    }

}
