using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Commands
{
    public class UpsertValueHourCommand: IRequest
    {
        public ValueHour ValueHour { get; set; }
    }
}
