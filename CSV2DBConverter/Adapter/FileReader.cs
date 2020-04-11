using System.IO;
using System.Text;

namespace CSV2DBConverter.Adapter
{
    public interface IFileReader
    {
        string[] Read(string path);
    }

    public class FileReader : IFileReader
    {
        public string[] Read(string path)
        {
            return File.ReadAllLines(path, Encoding.Default);
        }
    }
}
