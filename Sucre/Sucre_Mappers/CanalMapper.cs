using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class CanalMapper
    {        
        public partial CanalDto CanalToCanalDto(Canal canal);
        public partial Canal CanalDtoToCanal(CanalDto canalDto);        
        public partial CanalM CanalDtoToModel(CanalDto canalDto);
        public partial CanalDto ModelToCanalDto(CanalM canalM);
       // public partial AsPazDto AsPazToAsPazDto(AsPaz asPaz);
       // public partial AsPaz AsPazDtoToAsPaz(AsPazDto asPazDto);
        //public partial PointDto ModelToPointDto(PointM pointM);
        //[MapProperty(nameof(PointTableDto.PointDto), nameof(PointTableM.PointM))]
        //public partial PointTableM PointTableDtoToModel(PointTableDto pointTableTdo);        

        public CanalM CannaleFullDtoToM(CannaleFullDto cannaleFullDto)
        {
            CanalM cannaleM = CanalDtoToModel(cannaleFullDto.CannaleDto);
            if (cannaleFullDto.AsPazDto == null) 
                cannaleM.AsPazEmpty = true;
            if (cannaleFullDto.ParameterTypeDto!=null && 
                cannaleFullDto.ParameterTypeDto
                .Id != 0)
            {
                List<string> listText = new List<string>();
                if (cannaleFullDto.ParameterTypeDto.Name != null &&
                    cannaleFullDto.ParameterTypeDto.Name.Trim() != "")
                    listText.Add(cannaleFullDto.ParameterTypeDto.Name);
                if (cannaleFullDto.ParameterTypeDto.Mnemo != null && 
                    cannaleFullDto.ParameterTypeDto.Mnemo.Trim() != "")
                    listText.Add(cannaleFullDto.ParameterTypeDto.Mnemo);
                if (cannaleFullDto.ParameterTypeDto.UnitMeas != null && 
                    cannaleFullDto.ParameterTypeDto.UnitMeas.Trim() != "")
                    listText.Add(cannaleFullDto.ParameterTypeDto.UnitMeas);
                cannaleM.ParameterTypeName = String.Join(" ", listText.ToArray());

            }
            else
            {
                cannaleM.ParameterTypeName = string.Empty;
            }
            
            return cannaleM;
        }
}
}
