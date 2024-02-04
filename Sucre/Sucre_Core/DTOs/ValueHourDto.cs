namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Часовое значение
    /// </summary>
    public class ValueHourDto
    {        
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public decimal Value { get; set; }
        public bool Changed { get; set; }
    }
}

