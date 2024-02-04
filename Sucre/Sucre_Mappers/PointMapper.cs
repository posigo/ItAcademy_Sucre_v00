using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class PointMapper
    {
        public partial PointDto PointToPointDto(Point point);
        public partial Point PointDtoToPoint(PointDto pointDto);
        public partial PointM PointDtoToModel(PointDto pointDto);
        public partial PointDto ModelToPointDto(PointM pointM);
        [MapProperty(nameof(PointTableDto.PointDto), nameof(PointTableM.PointM))]
        public partial PointTableM PointTableDtoToModel(PointTableDto pointTableTdo);
    }
}
