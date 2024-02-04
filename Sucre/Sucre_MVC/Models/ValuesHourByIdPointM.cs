using Sucre_Core.DTOs;

namespace Sucre_MVC.Models
{
    public class ValuesHourByIdPointM
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
