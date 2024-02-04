using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetCexPointsByIdQuery: IRequest<CexPointsCanalsDto>
    {
        public int Id { get; set; }
    }
}
