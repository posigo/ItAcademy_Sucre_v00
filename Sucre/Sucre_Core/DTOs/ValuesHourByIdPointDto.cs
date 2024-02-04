namespace Sucre_Core.DTOs
{
    public class ValuesHourByIdPointDto
    {
        public MeasurementPointDto ValuesHourPoint { get; set; }
        public string Heading { get; set; }
        public int Columns { get; set; }
        //public List<string>[] ColumnsName { get; set; }
        public (int, List<string>)[] ColumnsName { get; set; }
        public int Rows { get; set; }
        public Dictionary<int, decimal[]> TableDict { get; set; }
    }
}
