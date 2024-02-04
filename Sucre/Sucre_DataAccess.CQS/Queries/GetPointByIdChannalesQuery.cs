using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetPointByIdChannalesQuery: IRequest<PointChannalesFullDto>
    {
        public int Id { get; set; }
    }
}
