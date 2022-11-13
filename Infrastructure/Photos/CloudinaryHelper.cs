using Microsoft.Extensions.Configuration;
using CloudinaryDotNet;

namespace Infrastructure.Photos;

public class CloudinaryHelper
{
    public CloudinaryHelper(IConfiguration configuration)
    {
        _configuration = configuration;

        var cloudName = _configuration["Cloudinary:Name"];
        var apiKey = _configuration["Cloudinary:PublicKey"];
        var secretKey = _configuration["Cloudinary:PrivateKey"];

        var account = new Account(cloudName, apiKey, secretKey);
        this.Cloudinary = new Cloudinary(account);
    }
    public IConfiguration _configuration { get; set; }
    public Cloudinary Cloudinary { get; set; }

    public async Task<IEnumerable<string>> GetAllPhotosUrls()
    {
        var res = await Cloudinary.ListResourcesAsync();
        return res.Resources.Select(x => x.Url.ToString());
    }

}
