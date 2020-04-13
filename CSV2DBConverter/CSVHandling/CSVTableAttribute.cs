namespace CSV2DBConverter.CSVHandling
{
    public class CSVTableAttribute
    {
        public string AttributeName{ get; set; }
        public bool IsForeignKey { get; set; }
        public string ForeignKeyName => AttributeName;
        public string ForeignKeyTable { get; set; }
        public string ForeignKeyID => ForeignKeyTable + "_id";

        public CSVTableAttribute() { }

        public CSVTableAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }
    }
}
