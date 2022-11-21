using Microsoft.Extensions.Configuration;

namespace Infrastructure.ExternalResources;

public class GoogleHelper
{
    public GoogleHelper(IConfiguration configuration)
    {
        var id = configuration["Google:Id"];
        var secret = configuration["Google:Secret"];
    }
}
