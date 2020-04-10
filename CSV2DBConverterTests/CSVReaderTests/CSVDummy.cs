namespace CSVReaderTests
{
    public static class CSVDummy
    {
        public static string[] SetupFile(
            string separator,
            string endOfHeadlineIdentifier,
            string tableLineIdentifier)
        {
            return new string[]
            {
                "someLine",
                "Another" + separator + "seperatedLine",
                "\n",
                endOfHeadlineIdentifier,
                tableLineIdentifier + separator+"tableLine2"+ separator+"tableLine3",
                "1content1"+ separator+"content2"+ separator+"content3",
                "2content1"+ separator+"content2"+ separator+"content3",
                "3content1"+ separator+"content2"+ separator+"content3"
            };
        }
    }
}