namespace Sucre_Core.DTOs
{
    public class ValuesDayByIdPointDto
    {
        /// <summary>
        /// данные по точке учёта
        /// </summary>
        public MeasurementPointHDto ValuesDayPoint { get; set; }
        /// <summary>
        /// заголовок
        /// </summary>
        public string Heading { get; set; }
        /// <summary>
        /// количество колонок
        /// </summary>
        public int Columns { get; set; }
        //public List<string>[] ColumnsName { get; set; }
        /// <summary>
        /// номер колонки и название колонки
        /// </summary>
        public (int, List<string>)[] ColumnsName { get; set; }
        /// <summary>
        /// количество строк
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// словарь: дата строка значений
        /// </summary>
        public Dictionary<DateTime, decimal[]> TableDict { get; set; }
    }
}
