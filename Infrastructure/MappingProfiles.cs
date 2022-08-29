using AutoMapper;
using Core;

namespace Infrastructure;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDto, AppUser>()
            .ForMember(x => x.CreationDate, y => y.AddTransform(t => DateTime.UtcNow));
    }

}
