using DemirorenCase.Contract.Authentication.Request;
using FluentValidation;

namespace DemirorenCase.API.Validations.Authentication
{
    public class LoginValidation : AbstractValidator<LoginRequestModel>
    {
        public LoginValidation()
        {
            RuleFor(x => x.UserName).Length(3, 15);
            RuleFor(x => x.Password).Length(6, 20);
        }
    }
}