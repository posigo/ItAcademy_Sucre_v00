using MediatR;

namespace Sucre_DataAccess.CQS.Commands
{
    public class SetArticleRateCommand: IRequest
    {
        public Guid Id { get; set; }
        public int? Rate { get; set; }
    }
}
