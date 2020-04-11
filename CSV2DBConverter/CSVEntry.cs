using System;
using System.Collections.Generic;

namespace CSV2DBConverter
{
    public struct SCSVEntry
    {
        public string Key;
        public string Value;
    }

    public interface ICSVRow
    {
        List<SCSVEntry> Row { get; }
        void Fill(string[] tablePattern, string[] line);
    }

    public class CSVRow: ICSVRow
    {
        public List<SCSVEntry> Row { get; private set; } = new List<SCSVEntry>();

        public void Fill(string[] tablePattern, string[] line)
        {
            if(tablePattern.Length != line.Length )
            {
                throw new WrongPatternException();
            }

            for(var i = 0; i < tablePattern.Length; i++)
            {
                Row.Add(new SCSVEntry()
                {
                    Key = tablePattern[i],
                    Value = line[i]
                });
            }
        }
    }

    public class WrongPatternException: Exception { }
}
