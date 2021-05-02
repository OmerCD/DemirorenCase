namespace DemirorenCase.Infrastructure.Abstractions.ValueObjects
{
    
    public class IdentityServerOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}