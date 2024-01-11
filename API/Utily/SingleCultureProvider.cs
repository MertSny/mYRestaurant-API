using Microsoft.AspNetCore.Localization;

namespace API.Utily
{
    public class SingleCultureProvider : IRequestCultureProvider
    {
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            return Task.Run(() => new ProviderCultureResult("en-US", "en-US"));
        }
    }
}
