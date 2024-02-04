using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class EnergyMapper
    {
        public partial EnergyDto EnergyToEnergyDto(Energy energy);
        public partial Energy EnergyDtoToEnergy(EnergyDto energy);
        public partial EnergyM EnergyDtoToModel(EnergyDto energy);
        public partial EnergyDto ModelToEnergyDto(EnergyM energy);
    }
}
