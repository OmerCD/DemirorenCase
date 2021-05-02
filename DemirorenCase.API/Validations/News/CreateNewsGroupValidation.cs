using DemirorenCase.Contract.News.Request;
using FluentValidation;

namespace DemirorenCase.API.Validations.News
{
    public class CreateNewsGroupValidation: AbstractValidator<CreateNewsGroupRequestModel>
    {
        public CreateNewsGroupValidation()
        {
            RuleFor(x => x.GroupName).NotEmpty();
        }
    }
}