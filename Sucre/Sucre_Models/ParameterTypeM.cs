using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class ParameterTypeM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter NAME of type parameter.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter MNEMONIC of type parameter.")]
        public string Mnemo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter UNIT MEASSURE of type parameter.")]
        public string UnitMeas { get; set; } = string.Empty;


    }
}
