using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetPointsFullByIdQuery: IRequest<Point>
    {
        public int Id;
    }
}
