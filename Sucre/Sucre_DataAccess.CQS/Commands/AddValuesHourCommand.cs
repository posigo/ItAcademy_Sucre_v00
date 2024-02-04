using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddValuesHourCommand: IRequest
    {
        public List<ValueHour> ValueHours { get; set; }
    }
}
