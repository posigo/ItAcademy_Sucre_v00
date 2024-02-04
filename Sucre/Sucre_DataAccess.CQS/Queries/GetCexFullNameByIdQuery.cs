using MediatR;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetCexFullNameByIdQuery: IRequest<string>
    {
        public int Id { get; set; }
    }
}
