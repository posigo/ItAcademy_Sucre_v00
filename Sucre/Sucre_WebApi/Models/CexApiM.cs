namespace Sucre_WebApi.Models
{
    /// <summary>
    /// Описание местоположения точки
    /// </summary>    
    public class CexApiM 
    {   
        /// <summary>
        /// Управление
        /// </summary>     
        public string? Management { get; set; }
        /// <summary>
        /// цех
        /// </summary>
        public string? CexName { get; set; }
        /// <summary>
        /// участок
        /// </summary>
        public string? Area { get; set; }
        /// <summary>
        /// устанорвка
        /// </summary>
        public string? Device { get; set; }
        /// <summary>
        /// локация
        /// </summary>
        public string? Location { get; set; }
    }
}
