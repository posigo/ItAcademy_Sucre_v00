using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddValuesDayCommand: IRequest
    {
        public List<ValueDay> ValueDays { get; set; }
    }
}
