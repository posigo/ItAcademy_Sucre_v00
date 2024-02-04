using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;

namespace Sucre_Services
{
    public class CexService: ICexService
    {
        private readonly IConfiguration _configuration;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly CexMapper _cexMapper;
        private readonly IMediator _mediator; 

        public CexService(
            IConfiguration configuration,
            ISucreUnitOfWork sucreUnitOfWork,
            CexMapper cexMapper,
            IMediator mediator)
        {
            _configuration = configuration;
            _sucreUnitOfWork = sucreUnitOfWork;
            _cexMapper = cexMapper;
            _mediator = mediator;
        }

        public async Task<bool> CreateCexAsync(CexDto cexDto)
        {
            try
            {
                var addCexCommand = new AddCexCommand()
                {
                    CexDto = cexDto,
                };
                _mediator.Send(addCexCommand);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> DeleteCexAsync(CexDto cexDto)
        {
            try
            {
                if (cexDto == null) return false;                
                Cex cex = _cexMapper.CexDtoToCex(cexDto);
                await _sucreUnitOfWork.repoSucreCex.RemoveAsync(cex);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CexService->{nameof(DeleteCexAsync)}");
            }
            return false;
        }

        public async Task<bool> DeleteCexByIdAsync(int Id)
        {
            try
            {
                if (Id == 0) return false;
                await _sucreUnitOfWork.repoSucreCex.RemoveByIdAsync(Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CexService-> {nameof(DeleteCexByIdAsync)}");
            }
            return false;
        }

        public async Task<CexDto> GetCexByIdAsync(int Id)
        {
            try
            {
                //var ccexDb = await _sucreUnitOfWork.repoSucreCex
                //    .FirstOrDefaultAsync(
                //        filter: cex => cex.Id == Id,
                //        includeProperties: WC.PointsName);
                var cexDb = await _sucreUnitOfWork.repoSucreCex.FindAsync(Id);                
                return (cexDb != null)
                    ? _cexMapper.CexToCexDto(cexDb)
                    : null;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CexService-> {nameof(GetCexByIdAsync)}");
            }
            return null;
        }

        public async Task<CexPointsCanalsDto> GetCexPointsChanalesById(int id)
        {  
            try
            {
                var cexPointsChanalesDto = new CexPointsCanalsDto();
                var request= new GetCexPointsByIdQuery() { Id = id };
                cexPointsChanalesDto = await _mediator
                    .Send(new GetCexPointsByIdQuery() { Id = id });
                return cexPointsChanalesDto;
            }
            catch (Exception ex)
            { 
            }
            return null;
        }

        public async Task<IEnumerable<CexDto>> GetListCexsAsync()
        {
            try
            {
                var cexsDb = await _sucreUnitOfWork.repoSucreCex.GetAllAsync();                
                IEnumerable<CexDto> cexsDto = cexsDb
                    .Select(cex => _cexMapper.CexToCexDto(cex));
                return cexsDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, CexService->  {nameof(GetListCexsAsync)}");
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetCexSelectList(bool addFirstSelect = true, 
            string valueFirstSelect = null)
        {
            return _sucreUnitOfWork.repoSucreCex.GetAllDropdownList(addFirstSelect, valueFirstSelect);
        }

        public async Task<bool> UpsertCexPatchAsync(CexDto cexDto)
        {
            try
            {
                if (cexDto == null) { return false; }
                Cex cex = _cexMapper.CexDtoToCex(cexDto);
                
                List<PatchDto> patchs = new List<PatchDto>()
                {
                    new()
                    {
                        PropertyName = nameof(cex.Management),
                        PropertyValue = cex.Management
                    },
                    new()
                    {
                        PropertyName = nameof(cex.CexName),
                        PropertyValue = cex.CexName
                    },
                    new()
                    {
                        PropertyName = nameof(cex.Area),
                        PropertyValue = cex.Area
                    },
                    new()
                    {
                        PropertyName = nameof(cex.Device),
                        PropertyValue = cex.Device
                    },
                    new()
                    {
                        PropertyName = nameof(cex.Location),
                        PropertyValue = cex.Location
                    }
                };
                await _sucreUnitOfWork.repoSucreCex.Patch(cex.Id, patchs);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, CexService->{nameof(UpsertCexPatchAsync)}");
            }

            return false;
        }

        public async Task<bool> UpsertCexAsync(CexDto cexDto)
        {
            try
            {
                if (cexDto == null) { return false; }
                Cex cex = new Cex();
                cex = _cexMapper.CexDtoToCex(cexDto);                
                if (cexDto.Id == null || cexDto.Id == 0)
                {
                    await _sucreUnitOfWork.repoSucreCex.AddAsync(cex);
                }
                else
                {
                    //energy.Id = energyDto.Id;
                    await _sucreUnitOfWork.repoSucreCex.UpdateAsync(cex);
                }
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, CexService->   {nameof(UpsertCexAsync)}");
            }
            
            return false;
        }

    }
}
