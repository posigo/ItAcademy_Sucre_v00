using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class SetArticleRateCommandHandler : IRequestHandler<SetArticleRateCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public SetArticleRateCommandHandler(ApplicationDbContext applicationDbContext)
        {               
            _applicationDbContext = applicationDbContext;
        }
        public async Task Handle(SetArticleRateCommand request, CancellationToken cancellationToken)
        {
            var guid = request.Id;
            var rate = request.Rate;
            var r = await _applicationDbContext.AppRoles
                .FirstOrDefaultAsync(x => x.Id.ToString() != string.Empty, cancellationToken);
                //throw new NotImplementedException();
        }
    }   
}
