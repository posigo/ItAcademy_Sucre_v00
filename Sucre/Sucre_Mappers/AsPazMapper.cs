using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class AsPazMapper
    {   
        public partial AsPazDto AsPazToAsPazDto(AsPaz asPaz);
        public partial AsPaz AsPazDtoToAsPaz(AsPazDto asPazDto);
        public partial AsPazM AsPazDtoToModel(AsPazDto asPazDto);
        public partial AsPazDto ModelToAsPazDto(AsPazM asPazM);
        [MapProperty(nameof(AsPazCanalM.AsPazM), nameof(AsPazChannaleDto.AsPazDto))]
        public partial AsPazChannaleDto ModelToAsPazChannaleDto(AsPazCanalM asPazCanalM);
        [MapProperty(nameof(AsPazChannaleDto.AsPazDto), nameof(AsPazCanalM.AsPazM))]
        public partial AsPazCanalM AsPazChannaleDtoToModel(AsPazChannaleDto asPazChannaleDto);

        //public partial PointDto ModelToPointDto(PointM pointM);
        //[MapProperty(nameof(PointTableDto.PointDto), nameof(PointTableM.PointM))]
        //public partial PointTableM PointTableDtoToModel(PointTableDto pointTableTdo);        

    }
}
