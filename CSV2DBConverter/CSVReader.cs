using CSV2DBConverter.Adapter;

namespace CSV2DBConverter
{
    public interface ICSVReader
    {
        string[] Head { get; }
        string[] Table { get; }
        string[] Body { get; }

        void Analyze();
    }

    public class CSVReader : ICSVReader
    {
        private readonly string path;
        private readonly string seperator;
        private readonly IFileReader fileReader;
        private string[] file;

        public string[] Head { get; private set; }
        public string[] Table { get; private set; }
        public string[] Body { get; private set; }

        public CSVReader(
            string path, 
            string seperator,
            IFileReader fileReader)
        {
            this.path = path;
            this.seperator = seperator;
            this.fileReader = fileReader;
        }

        public void Analyze()
        {
            file = fileReader.Read(path);
        }
    }
}
