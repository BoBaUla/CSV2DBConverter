namespace CSV2DBConverter.CSVHandling
{
    public class CSVTableAttribute
    {
        public string AttributeName{ get; set; }
        public bool IsForeignKey { get; set; }

        public CSVTableAttribute() { }

        public CSVTableAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }
    }
}
