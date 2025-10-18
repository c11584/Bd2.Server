using AutoMapper;

namespace Bd2.Server.Extensions.ServiceExtensions;

public class AutoMapperConfig
{
	public static MapperConfiguration RegisterMappings()
	{
		return new MapperConfiguration(delegate(IMapperConfigurationExpression cfg)
		{
			cfg.AddProfile(new CustomProfile());
		});
	}
}
