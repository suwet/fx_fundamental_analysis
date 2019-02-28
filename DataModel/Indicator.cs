namespace fx_fundamental_analysis.DataModel
{
    public class Indicator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Actual { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q2 { get; set; }
        public decimal Q3 { get; set; }
        public decimal Q4 { get; set; }

        public string Currency{get;set;}

        public string Remark{get;set;}
    }
}