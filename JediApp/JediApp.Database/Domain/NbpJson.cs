namespace JediApp.Database.Domain
{
    public class NBPJsonRoot
    {
        public string table { get; set; }
        public string no { get; set; }
        public string tradingDate { get; set; }
        public string effectiveDate { get; set; }
        public List<NBPJsonRate> rates { get; set; }
    }

    public class NBPJsonRate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }

    }
}
