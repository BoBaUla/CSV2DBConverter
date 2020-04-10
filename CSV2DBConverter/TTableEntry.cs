using System;
using System.Collections.Generic;

namespace CSV2DBConverter
{
    public struct STableEntry
    {
        public string Key;
        public string Value;
    }

    public interface ITableRow
    {
        List<STableEntry> Row { get; }
        void Fill(string[] tablePattern, string[] line);
    }

    public class TableRow: ITableRow
    {
        public List<STableEntry> Row { get; private set; } = new List<STableEntry>();

        public void Fill(string[] tablePattern, string[] line)
        {
            if(tablePattern.Length != line.Length )
            {
                throw new WrongPatternException();
            }

            for(var i = 0; i < tablePattern.Length; i++)
            {
                Row.Add(new STableEntry()
                {
                    Key = tablePattern[i],
                    Value = line[i]
                });
            }
        }
    }

    public class WrongPatternException: Exception { }
}
