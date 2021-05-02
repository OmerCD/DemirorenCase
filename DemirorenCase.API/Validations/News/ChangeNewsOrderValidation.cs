using DemirorenCase.Contract.News.Request;
using FluentValidation;

namespace DemirorenCase.API.Validations.News
{
    public class ChangeNewsOrderValidation : AbstractValidator<ChangeNewsOrderModel>
    {
        public ChangeNewsOrderValidation()
        {
            RuleFor(x => x.Order).GreaterThan(0);
            RuleFor(x => x.NewsId).NotEmpty();
        }
    }
}