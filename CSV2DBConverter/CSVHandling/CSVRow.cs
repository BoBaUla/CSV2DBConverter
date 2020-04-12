using System;
using System.Collections.Generic;

namespace CSV2DBConverter.CSVHandling
{
    public struct SCSVEntry
    {
        public string Key;
        public string Value;
    }

    public interface ICSVRow
    {
        List<SCSVEntry> Row { get; }
        void Fill(List<CSVTableAttribute> tablePattern, string[] line);
    }

    public class CSVRow: ICSVRow
    {
        public List<SCSVEntry> Row { get; private set; } = new List<SCSVEntry>();

        public void Fill(List<CSVTableAttribute> tablePattern, string[] line)
        {
            if(tablePattern.Count != line.Length )
            {
                throw new WrongPatternException();
            }

            for(var i = 0; i < tablePattern.Count; i++)
            {
                Row.Add(new SCSVEntry()
                {
                    Key = tablePattern[i].AttributeName,
                    Value = line[i]
                });
            }
        }
    }

    public class WrongPatternException: Exception { }
}
