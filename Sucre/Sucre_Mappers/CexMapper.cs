using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class CexMapper
    {
        public partial CexDto CexToCexDto(Cex cex);
        public partial Cex CexDtoToCex(CexDto cexDto);
        public partial CexM CexDtoToModel(CexDto cexDto);
        public partial CexDto ModelToCexDto(CexM cexM);
    }
}
