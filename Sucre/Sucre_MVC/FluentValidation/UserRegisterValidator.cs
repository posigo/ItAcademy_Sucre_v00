using FluentValidation;
using Sucre_Models;

namespace Sucre_MVC.FluentValidation
{
    public class UserRegisterValidator: AbstractValidator<AppUserRegisterM>
    {
        public UserRegisterValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty()
                .EmailAddress()
                .NotEqual("");
        }

    }
}
