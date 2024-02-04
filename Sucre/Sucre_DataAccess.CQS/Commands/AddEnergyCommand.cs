using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddEnergyCommand : IRequest
    {
        public EnergyDto EnergyDto { get; set; }
    }
    //public class AddEnergyCommand : IRequest<int>
    //{
    //    public EnergyDto EnergyDto { get; set; }
    //}
}
