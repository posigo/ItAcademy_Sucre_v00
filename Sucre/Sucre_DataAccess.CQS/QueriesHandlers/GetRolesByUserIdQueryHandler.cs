using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.DTOs;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_Mappers;
using Sucre_Utility;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetRolesByUserIdQueryHandler : IRequestHandler<GetRolesByUserIdQuery, List<AppRoleDto>>
    {
        private readonly AppRoleMapper _appRoleMapper;
        private readonly ApplicationDbContext _applicationDbContext;

        public GetRolesByUserIdQueryHandler(
            AppRoleMapper appRoleMapper,
            ApplicationDbContext applicationDbContext)
        {
            _appRoleMapper = appRoleMapper;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<AppRoleDto>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _applicationDbContext.AppUsers
                .Include(WC.AppRolesName)
                .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

                var appRoles = user.AppRoles;

                var appRolesDto = appRoles.Select(role => _appRoleMapper.AppRoleToAppRoleDto(role));

                return appRolesDto.ToList();
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
