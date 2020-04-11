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
        string[] _columnsToSkip;

        [SetUp]
        public void Setup()
        {
            _file = CSVDummy.SetupFile(_separator, _endOfHeadlineIdentifier, _tableLineIdentifier);
            _endOfHeadLine = GetEndOfHeadline();
            _tableLine = GetTableline();

            _fileReaderMock = new Mock<IFileReader>();
            _fileReaderMock.Setup(m => m.Read(_path)).Returns(_file);

            _csvReader = new CSVReader(_path, _fileReaderMock.Object);
            _columnsToSkip = new string[] { };
            _cut = new CSVEntryParser(
                _path,
                _tableLine,
                _csvReader,
                _columnsToSkip);
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

            [Test]
            public void SkipsCertainColumn()
            {
                _columnsToSkip = new string[] { "tableLine2" };
                _cut = new CSVEntryParser(
                _path,
                _tableLine,
                _csvReader,
                _columnsToSkip);

                _cut.Initialize();

                var table = _cut.TablePattern;

                Assert.IsFalse(table.Contains(_columnsToSkip.First()));
            }

            [Test]
            public void SkipsCertainColumnIfTheyAreDuplicated()
            {
                _columnsToSkip = new string[] { "tableLine2" };
                _file = CSVDummy.SetupFileDuplicatedEntries(
                    _separator,
                    _endOfHeadlineIdentifier,
                    _tableLineIdentifier);
                _fileReaderMock.Setup(m => m.Read(_path)).Returns(_file);

                _cut = new CSVEntryParser(
                _path,
                _tableLine,
                _csvReader,
                _columnsToSkip);

                _cut.Initialize();

                var table = _cut.TablePattern;
                Assert.AreEqual(_file.Count() - _tableLine - _columnsToSkip.Count() -1, table.Count());
                Assert.IsFalse(table.Contains(_columnsToSkip.First()));
            }
        }


    }
}
