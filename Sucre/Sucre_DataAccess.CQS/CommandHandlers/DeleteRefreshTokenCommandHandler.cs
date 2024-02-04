using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class DeleteRefreshTokenCommandHandler : IRequestHandler<DeleteRefreshTokenCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteRefreshTokenCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task Handle(DeleteRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = await _applicationDbContext.RefreshTokens
                .FirstOrDefaultAsync(
                    rt => rt.Id.Equals(command.Id),
                    cancellationToken);
                _applicationDbContext.Remove(refreshToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->Error, Remove refresh token: {ex.Message}");
                throw;
            }
            
        }
    }
}
