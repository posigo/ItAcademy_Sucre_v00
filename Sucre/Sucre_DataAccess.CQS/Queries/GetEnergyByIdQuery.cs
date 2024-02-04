using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetEnergyByIdQuery: IRequest<Energy> //EnergyDto
    {
        public int Id { get; set; }
    }
}
