namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Описание местоположения точки
    /// </summary>    
    public class CexDto : IBaseEntity<int>
    {        
        public int Id { get; set; }
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
