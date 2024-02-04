using MediatR;
using Sucre_Core;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetListReadIdQuery: IRequest<List<ChannaleRead>>
    {
        public List<int> ReadId { get; set; }
    }
}
