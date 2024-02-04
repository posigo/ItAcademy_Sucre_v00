using FluentValidation;
using Sucre_Models;
using Sucre_DataAccess.Repository.IRepository;
namespace Sucre_MVC.FluentValidation
{
    public class UserLoginValidator: AbstractValidator<AppUserLoginM>
    {
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        public UserLoginValidator(ISucreUnitOfWork sucreUnitOfWork)
        {
            _sucreUnitOfWork = sucreUnitOfWork;

            RuleFor(user => user.Email)
                .EmailAddress()
                .NotEmpty();
            RuleFor(user => user.Password)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20);

        }
    }
}
