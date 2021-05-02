using System;
using DemirorenCase.Contract.News.Request;
using FluentValidation;

namespace DemirorenCase.API.Validations.News
{
    public class UpdateNewsValidation : AbstractValidator<UpdateNewsRequestModel>
    {
        public UpdateNewsValidation()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Headline).NotEmpty().Length(3, 50);
            RuleFor(x => x.Link).Custom((link, context) =>
            {
                try
                {
                    new Uri(link);
                }
                catch
                {
                    context.AddFailure("Not a valid link");
                }
            });
        }
    }
}