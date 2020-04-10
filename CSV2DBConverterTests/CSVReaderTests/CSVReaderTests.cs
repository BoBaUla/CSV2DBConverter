using CSV2DBConverter;
using CSV2DBConverter.Adapter;
using Moq;
using NUnit.Framework;

namespace CSVReaderTests
{
    [TestFixture]
    public class CSVReaderTests
    {
        string _path = "somePath";
        string _separator = ";";

        Mock<IFileReader> _fileReaderMock;
        ICSVReader _cut;

        [SetUp]
        public void Setup()
        {
            _fileReaderMock = new Mock<IFileReader>();

            _cut = new CSVReader(
                _path,
                _separator,
                _fileReaderMock.Object);
        }

        [TestFixture]
        public class Analyze : CSVReaderTests
        {
           [Test]
           public void ReadsFile()
            {
                _cut.Analyze();
                _fileReaderMock.Verify(m => m.Read(_path));
            }
        }
    }
}