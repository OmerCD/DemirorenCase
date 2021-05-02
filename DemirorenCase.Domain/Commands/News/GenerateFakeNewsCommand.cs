using System.Threading;
using System.Threading.Tasks;
using Bogus;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MediatR;

namespace DemirorenCase.Domain.Commands.News
{
    public record GenerateFakeNewsCommand:IRequest<GenerateFakeNewsCommandResponse>;

    public record GenerateFakeNewsCommandResponse;
    public class GenerateFakeNewsCommandHandler:IRequestHandler<GenerateFakeNewsCommand,GenerateFakeNewsCommandResponse>
    {
        private readonly INewsService _newsService;

        public GenerateFakeNewsCommandHandler(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<GenerateFakeNewsCommandResponse> Handle(GenerateFakeNewsCommand request, CancellationToken cancellationToken)
        {
            var newsFaker = new Faker<CreateNewsDto>()
                .CustomInstantiator(faker => new CreateNewsDto())
                .RuleFor(dto => dto.Description, faker => faker.Lorem.Paragraph(2))
                .RuleFor(dto => dto.Headline, faker => faker.Lorem.Random.Words(2))
                .RuleFor(dto=>dto.Link, faker => faker.Internet.Url());
            for (int i = 0; i < 20; i++)
            {
                await _newsService.CreateNewsAsync(newsFaker.Generate(), cancellationToken);
            }

            return new GenerateFakeNewsCommandResponse();
        }
    }
}