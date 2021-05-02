using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Contract.News.Request;
using DemirorenCase.Contract.News.Response;
using DemirorenCase.Domain.Commands.News;
using DemirorenCase.Domain.Queries.News;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenCase.API.Controllers.v1._0
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public NewsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Generates 20 Fake News
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("generateFakes")]
        public async Task<IActionResult> GenerateFakes(CancellationToken cancellationToken)
        {
            var command = new GenerateFakeNewsCommand();
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("group")]
        public async Task<IActionResult> CreateNewsGroup(CreateNewsGroupRequestModel model,
            CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateNewsGroupCommand>(model);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPatch("group/{groupId}/addNews")]
        public async Task<IActionResult> AddNewsToGroup(string groupId, [FromBody] AddNewsToGroupModel model,
            CancellationToken cancellationToken)
        {
            var command = new AddNewsToGroupCommand
            {
                GroupId = groupId,
                NewsId = model.NewsId,
                Order = model.Order
            };
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<NewsGroupViewModel>(response));
        }

        [HttpPatch("group/{groupId}/changeOrder")]
        public async Task<IActionResult> ChangeNewsOrder(string groupId, [FromBody] ChangeNewsOrderModel model,
            CancellationToken cancellationToken)
        {
            var command = new ChangeNewsOrderCommand
            {
                GroupId = groupId,
                NewsId = model.NewsId,
                Order = model.Order
            };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet("group")]
        public async Task<IActionResult> GetNewsGroups(CancellationToken cancellationToken)
        {
            var query = new GetNewsGroupsQuery();
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<NewsGroupsViewModel>(response));
        }

        [HttpGet("group/{id}")]
        public async Task<IActionResult> GetNewsGroup(string id, CancellationToken cancellationToken)
        {
            var query = new GetNewsGroupQuery
            {
                GroupId = id
            };
            var response = await _mediator.Send(query, cancellationToken);
            var newsGroupViewModel = _mapper.Map<NewsGroupViewModel>(response);
            newsGroupViewModel.OrderedNews = newsGroupViewModel.OrderedNews.OrderBy(x => x.Order);
            return Ok(newsGroupViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(CreateNewsRequestModel model, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateNewsCommand>(model);
            var response = await _mediator.Send(command, cancellationToken);
            return Created(new Uri(HttpContext.Request.GetDisplayUrl()), response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(string id, [FromBody] UpdateNewsRequestModel model,
            CancellationToken cancellationToken)
        {
            var command = _mapper.Map<UpdateNewsCommand>(model);
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews(CancellationToken cancellationToken)
        {
            var query = new GetAllNewsQuery();
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(_mapper.Map<IEnumerable<NewsViewModel>>(response));
        }
    }
}