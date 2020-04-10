using CSV2DBConverter.Adapter;
using System.Linq;

namespace CSV2DBConverter
{
    public interface ICSVReader
    {
        string[] Head { get; }
        string[] Table { get; }
        string[] Body { get; }

        void Analyze(int headLineEnd, int tableLine);
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

        public void Analyze(int headLineEnd, int tableLine)
        {
            file = fileReader.Read(path);
            Head = file.Take(headLineEnd + 1).ToArray();
            Table = file.Skip(headLineEnd + 1).Take(1).ToArray();
            Body = file.Skip(tableLine + 1).ToArray();
        }
    }
}
