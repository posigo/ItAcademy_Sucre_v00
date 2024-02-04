using Microsoft.Extensions.Configuration;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;
using Sucre_Utility;

namespace Sucre_Services
{
    public class AsPazService: IAsPazService
    {
        private readonly IConfiguration _configuration;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly AsPazMapper _asPazMapper;        

        public AsPazService(
            IConfiguration configuration,
            ISucreUnitOfWork sucreUnitOfWork,
            AsPazMapper asPazMapper)
        {
            _configuration = configuration;
            _sucreUnitOfWork = sucreUnitOfWork;
            _asPazMapper = asPazMapper;
        }

        public async Task<int> CheckAndDelByChanaleIdAsync(int IdCanale)
        {
            try
            {
                AsPaz asPaz = await _sucreUnitOfWork.repoSucreAsPaz
                    .FirstOrDefaultAsync(item => item.CanalId == IdCanale);
                if (asPaz != null)
                {
                    await _sucreUnitOfWork.repoSucreAsPaz
                        .RemoveByIdAsync(asPaz.Id);
                    return 2;
                }
                else 
                    return 1;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, AsPazService-> {nameof(CheckAndDelByChanaleIdAsync)}");
                
            }            
            return 0; 
        }

        public async Task<bool> DeleteAsPazAsync(AsPazDto asPazDto)
        {
            try
            {
                if (asPazDto == null) return false;
                
                await _sucreUnitOfWork.repoSucreAsPaz
                    .RemoveAsync(_asPazMapper.AsPazDtoToAsPaz(asPazDto));
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, AsPazService->{nameof(DeleteAsPazAsync)}");
            }
            return false;
        }

        public async Task<bool> DeleteAsPazByIdAsync(int Id)
        {
            try
            {
                if (Id == 0) return false;
                await _sucreUnitOfWork.repoSucreAsPaz.RemoveByIdAsync(Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, AsPazService-> {nameof(DeleteAsPazByIdAsync)}");
            }
            return false;
        }

        public async Task<AsPazDto> GetAsPazByIdAsync(int Id)
        {
            try
            {
                var asPazDb = await _sucreUnitOfWork.repoSucreAsPaz
                    .FindAsync(Id);// .FirstOrDefaultAsync(asPaz => asPaz.CanalId == Id);
                return (asPazDb != null)
                    ? _asPazMapper.AsPazToAsPazDto(asPazDb)
                    : null;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, AsPazService-> {nameof(GetAsPazByIdAsync)}");
            }
            return null;
        }
                
        public async Task<AsPazChannaleDto> GetAsPazChannaleByIdAsync(int Id)
        {
            if (Id == 0) return null;
            var asPazCanaleDb = _sucreUnitOfWork.repoSucreAsPaz
                .FirstOrDefault(
                    filter: asPaz => asPaz.Id == Id,
                    includeProperties: $"{WC.CanalName}");
            var asPazCanaleDto = new AsPazChannaleDto()
            {
                AsPazDto = _asPazMapper.AsPazToAsPazDto(asPazCanaleDb),
                CanalName = asPazCanaleDb.Canal.Name
            };
            return asPazCanaleDto;
        }

        public async Task<AsPazChannaleDto> GetAsPazChannaleByIdCanAsync(int IdCanale)
        {
            if (IdCanale == 0) return null;
            var asPazCanaleDb = _sucreUnitOfWork.repoSucreAsPaz
                .FirstOrDefault(
                    filter: asPaz => asPaz.CanalId == IdCanale,
                    includeProperties: $"{WC.CanalName}");
            var asPazCanaleDto = new AsPazChannaleDto()
            {
                AsPazDto = _asPazMapper.AsPazToAsPazDto(asPazCanaleDb),
                CanalName = asPazCanaleDb.Canal.Name
            };
            return asPazCanaleDto;
        }

        public async Task<IEnumerable<AsPazChannaleDto>> GetListAsPasChannaleAsync()
        {
            try
            {
                var asPazsDb = await _sucreUnitOfWork
                    .repoSucreAsPaz
                    .GetAllAsync(includeProperties: $"{WC.CanalName}");
                IEnumerable<AsPazChannaleDto> asPazsChanaleDto = asPazsDb
                    .Select(asPaz => new AsPazChannaleDto
                    {
                        AsPazDto = _asPazMapper.AsPazToAsPazDto(asPaz),
                        CanalName = asPaz.Canal.Name
                    });
                return asPazsChanaleDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, AsPazService->  {nameof(GetListAsPasChannaleAsync)}");
            }
            return null;
        }

        public async Task<bool> UpsertAsPazAsync(AsPazDto asPazDto)
        {
            try
            {
                if (asPazDto == null) { return false; }
                AsPaz asPaz = _asPazMapper.AsPazDtoToAsPaz(asPazDto);

                if (asPaz.Id == null || asPaz.Id == 0)
                {
                    await _sucreUnitOfWork.repoSucreAsPaz.AddAsync(asPaz);
                }
                else
                {
                    await _sucreUnitOfWork.repoSucreAsPaz.Patch(asPaz.Id, new List<PatchDto>()
                        {
                            new() {PropertyName = nameof(asPaz.CanalId),PropertyValue = asPaz.CanalId},
                            new() {PropertyName = nameof(asPaz.High), PropertyValue = asPaz.High},
                            new() {PropertyName = nameof(asPaz.Low),PropertyValue = asPaz.Low},
                            new() {PropertyName = nameof(asPaz.A_HighEin),PropertyValue = asPaz.A_HighEin},
                            new() {PropertyName = nameof(asPaz.A_HighType),PropertyValue = asPaz.A_HighType},
                            new() {PropertyName = nameof(asPaz.A_High),PropertyValue = asPaz.A_High},
                            new() {PropertyName = nameof(asPaz.W_HighEin),PropertyValue = asPaz.W_HighEin},
                            new() {PropertyName = nameof(asPaz.W_HighType),PropertyValue = asPaz.W_HighType},
                            new() {PropertyName = nameof(asPaz.W_High),PropertyValue = asPaz.W_High},
                            new() {PropertyName = nameof(asPaz.W_LowEin),PropertyValue = asPaz.W_LowEin},
                            new() {PropertyName = nameof(asPaz.W_LowType),PropertyValue = asPaz.W_LowType},
                            new() {PropertyName = nameof(asPaz.W_Low),PropertyValue = asPaz.W_Low},
                            new() {PropertyName = nameof(asPaz.A_LowEin),PropertyValue = asPaz.A_LowEin},
                            new() {PropertyName = nameof(asPaz.A_LowType),PropertyValue = asPaz.A_LowType},
                            new() {PropertyName = nameof(asPaz.A_Low),PropertyValue = asPaz.A_Low}
                        });

                    //await _sucreUnitOfWork.repoSucrePoint .UpdateAsync(point);
                }
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, AsPazService->   {nameof(UpsertAsPazAsync)}");
            }

            return false;
        }

    }
}
