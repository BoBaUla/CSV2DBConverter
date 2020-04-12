using CSV2DBConverter;
using CSV2DBConverter.Adapter;
using CSV2DBConverter.CSVHandling;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace CSVReaderTests
{
    [TestFixture]
    public class CSVReaderTests
    {
        string _path = "somePath";
        string _separator = ";";
        string _endOfHeadlineIdentifier = "endOfHeadline";
        string _tableLineIdentifier = "tableLine";
        int _endOfHeadLine;
        int _tableLine;
        
        Mock<IFileReader> _fileReaderMock;
        ICSVReader _cut;

        string[] _file;

        [SetUp]
        public void Setup()
        {
            _file = CSVDummy.SetupFile(_separator, _endOfHeadlineIdentifier, _tableLineIdentifier);
            _endOfHeadLine = GetEndOfHeadline();
            _tableLine = GetTableline();

            _fileReaderMock = new Mock<IFileReader>();
            _fileReaderMock.Setup(m => m.Read(_path)).Returns(_file);

            _cut = new CSVReader(
                _path,
                _fileReaderMock.Object);
        }

        private int GetEndOfHeadline()
        {
            var i = 0;
            foreach(var line in _file)
            {
                if (line.Contains(_endOfHeadlineIdentifier))
                    return i;
                i++;
            }
            return i;
        }

        private int GetTableline()
        {
            var i = 0;
            foreach (var line in _file)
            {
                if (line.Contains(_tableLineIdentifier))
                    return i;
                i++;
            }
            return i;
        }
        
        [TestFixture]
        public class Analyze : CSVReaderTests
        {
           [Test]
           public void ReadsFile()
            {
                _cut.Analyze(0);
                _fileReaderMock.Verify(m => m.Read(_path));
            }

            [Test]
            public void InitializesHead()
            {
                _cut.Analyze(_tableLine);

                var head = _cut.Head;

                for(var i = 0; i < _endOfHeadLine; i++)
                {
                    Assert.AreEqual(_file[i], head[i]);
                }

                Assert.AreEqual(_endOfHeadlineIdentifier, head.Last() ,"letzter Eintrag falsch");
            }

            [Test]
            public void InitializesTable()
            {
                _cut.Analyze(_tableLine);

                var table = _cut.Table;

                Assert.True(table.Length == 1, $"zu viele Einträge: {table.Length}");
                Assert.True(table.Last().Contains(_tableLineIdentifier), "letzter Eintrag falsch");
            }

            [Test]
            public void InitializesBody()
            {
                _cut.Analyze(_tableLine);

                var body = _cut.Body;

                Assert.AreEqual(_file[_tableLine + 1], body.First() , "erster Eintrag falsch");
                Assert.AreEqual(_file.Last(), body.Last() , "letzter Eintrag falsch");
                Assert.AreEqual(_file.Length - (_tableLine + 1), body.Length, "Falsche länge");
            }
        }
    }
}