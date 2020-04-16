using System.Collections.Generic;

namespace HaushaltsbuchConverter.Logic.DBInitialisation
{
    public class TableExtractor
    {
        public static readonly string _DateExtractName = "Buchung";
        public static readonly string _RecipientExtractName = "Auftraggeber_Empfänger";
        public static readonly string _BookingTextExtractName = "Buchungstext";

        public List<string> ExtractedTables = new List<string>
        {
            _DateExtractName, 
            _RecipientExtractName,
            _BookingTextExtractName
        };

        public Dictionary<string, string> TableMap = new Dictionary<string, string>
        {
            { _DateExtractName, TablePatternForFixedTables._DateTableName },
            { _RecipientExtractName, TablePatternForFixedTables._RecipientTableName },
            { _BookingTextExtractName, TablePatternForFixedTables._BookingTableName },

        };
    }
}
