using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetPointsFullQuery: IRequest<List<Point>>
    {
    }
}
