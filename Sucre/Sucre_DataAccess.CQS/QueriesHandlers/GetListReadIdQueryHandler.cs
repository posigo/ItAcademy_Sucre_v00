using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetListReadIdQueryHandler: IRequestHandler<GetListReadIdQuery,List<ChannaleRead>>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public GetListReadIdQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ChannaleRead>> Handle(GetListReadIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<ChannaleRead> channalesRead = new List<ChannaleRead>();

                if (request.ReadId == null || request.ReadId.Count == 0)
                    return channalesRead;
                //var rApi = await _applicationDbContext.DeviceTags
                //    .Where(tag => request.ReadId.Contains(tag.ChannaleId) == true)
                //    .ToListAsync(cancellationToken);
                var rApi = await _applicationDbContext.DeviceTags
                    .Where(tag => request.ReadId.Contains(tag.ChannaleId) == true)
                    .ToListAsync(cancellationToken);
                if (rApi == null || rApi.Count == 0)
                    return channalesRead;

                foreach (var dd in rApi)
                {
                    //var chread = new ChannaleRead()
                    //{
                    //    ChannaleId = dd.ChannaleId,
                    //    UrlAPi = (await _applicationDbContext.Devices
                    //        .FirstOrDefaultAsync(dvc => dvc.Id == dd.DeviceId, cancellationToken))
                    //        .Connection,
                    //    Query = $"?evn={dd.Enviroment}&prm={dd.ParameterCode}"
                    //};
                    var chread = new ChannaleRead();
                    chread.ChannaleId = dd.ChannaleId;
                    chread.UrlAPi = (await _applicationDbContext.Devices
                        .FirstOrDefaultAsync(dvc => dvc.Id == dd.DeviceId, cancellationToken))
                        .Connection;
                    chread.Query = $"?evn={dd.Enviroment}&prm={dd.ParameterCode}";
                    channalesRead.Add(chread);
                    //var id = dd.ChannaleId;
                    //var env = dd.Enviroment;
                    //var prm = dd.ParameterCode;
                    //var urlApi = (await _applicationDbContext.Devices
                    //    .FirstOrDefaultAsync(dvc => dvc.Id == dd.DeviceId, cancellationToken))
                    //    .Connection;
                }
                return channalesRead;
            }
            catch (Exception ex)
            {
                return null;
            }
            

        }
    }
}
