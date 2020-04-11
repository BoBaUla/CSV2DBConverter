using CSV2DBConverter;
using CSV2DBConverter.Adapter;
using CSVReaderTests;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace CSV2DBConverterTests.TableEntryParserTests
{
    [TestFixture]
    public class CSVEntryParserTests
    {
        string _path = "somePath";
        string _separator = ";";
        string _endOfHeadlineIdentifier = "endOfHeadline";
        string _tableLineIdentifier = "tableLine";
        int _endOfHeadLine;
        int _tableLine;

        Mock<IFileReader> _fileReaderMock;
        ICSVEntryParser _cut;
        ICSVReader _csvReader;
        string[] _file;

        [SetUp]
        public void Setup()
        {
            _file = CSVDummy.SetupFile(_separator, _endOfHeadlineIdentifier, _tableLineIdentifier);
            _endOfHeadLine = GetEndOfHeadline();
            _tableLine = GetTableline();

            _fileReaderMock = new Mock<IFileReader>();
            _fileReaderMock.Setup(m => m.Read(_path)).Returns(_file);

            _csvReader = new CSVReader(_path, _fileReaderMock.Object);

            _cut = new CSVEntryParser(
                _path,
                _tableLine,
                _csvReader);
        }

        private int GetEndOfHeadline()
        {
            var i = 0;
            foreach (var line in _file)
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
        public class Initialize : CSVEntryParserTests
        {
            [Test]
            public void ReadsFile()
            {
                _cut.Initialize();
                _fileReaderMock.Verify(m => m.Read(_path));
            }            
        }

        [TestFixture]
        public class GetTableEntry: CSVEntryParserTests
        {
            [Test]
            public void ReturnsCorrectCount()
            {
                _cut.Initialize();
                var result = _cut.TableEntries;
                Assert.AreEqual(_file.Length - _tableLine - 1, result.Count);
            }

            [Test]
            public void ReturnsCorrectLastValue()
            {
                var table = _file[_tableLine].Split(_separator);
                var last = _file.Last().Split(_separator);

                _cut.Initialize();
                var result = _cut.TableEntries.Last();

                for(var i = 0; i < table.Length; i++ )
                {
                    Assert.AreEqual(table[i], result.Row[i].Key);
                    Assert.AreEqual(last[i], result.Row[i].Value);
                }
            }
        }


    }
}
