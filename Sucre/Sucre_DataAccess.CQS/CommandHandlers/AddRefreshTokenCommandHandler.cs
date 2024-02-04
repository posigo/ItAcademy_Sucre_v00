using MediatR;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AddRefreshTokenCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task Handle(AddRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = new RefreshToken()
                {
                    Id = command.Id,
                    GenerateAt = DateTime.UtcNow,
                    ExpiringAt = DateTime.UtcNow.AddMinutes(command.AddMinutes),
                    AssociatedDeviceName = command.Ip,
                    AppUserId = command.UserId
                };
                await _applicationDbContext.AddAsync(refreshToken,cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error("*->Add refresh token in Bd", ex);
                throw;
            }
        }
    }
}
