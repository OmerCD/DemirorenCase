using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.IdentityServer.Models;
using IdentityModel.Client;
using MediatR;
using Newtonsoft.Json;

namespace DemirorenCase.Domain.Queries.Authentication
{
    public record GetAdminIdQuery : IRequest<GetAdminIdQueryResponse>;
    public record GetAdminIdQueryResponse(int Id);
    public class GetAdminIdQueryHandler : IRequestHandler<GetAdminIdQuery,GetAdminIdQueryResponse>
    {
        private readonly HttpClient _httpClient;

        public GetAdminIdQueryHandler(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("identityServer");
        }
        public async Task<GetAdminIdQueryResponse> Handle(GetAdminIdQuery request, CancellationToken cancellationToken)
        {
            var responseMessage = await _httpClient.GetAsync("general/adminId", cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<GetAdminInfoResponseModel>(
                    await responseMessage.Content.ReadAsStringAsync(cancellationToken));
                if (model == null) return null;
                return new GetAdminIdQueryResponse(model.Id);
            }

            return null;
        }
    }
}