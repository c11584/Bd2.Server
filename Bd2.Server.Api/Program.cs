using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bd2.Server.Api.Config;
using Bd2.Server.Api.Extensions;
using Bd2.Server.Api.Filter;
using Bd2.Server.Common;
using Bd2.Server.Extensions.ServiceExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bd2.Server.Api;

public class Program
{
	public static void Main(string[] args)
	{
		Environment.CurrentDirectory = AppContext.BaseDirectory;
		WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
		webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer(delegate(ContainerBuilder binder)
		{
			binder.RegisterModule<AutofacModuleRegister>();
			binder.RegisterModule<AutofacPropertityModuleReg>();
		});
		webApplicationBuilder.Services.AddLogging(delegate(ILoggingBuilder logging)
		{
			logging.AddConsole();
		});
		webApplicationBuilder.Services.AddControllers(delegate(MvcOptions options)
		{
			options.InputFormatters.Add(new TextInputFormatter());
			options.Filters.Add<CustomerExceptionFilter>();
		});
		webApplicationBuilder.Services.AddEndpointsApiExplorer();
		webApplicationBuilder.Services.AddSwaggerGen();
		webApplicationBuilder.Services.AddAutoMapper(typeof(AutoMapperConfig));
		AutoMapperConfig.RegisterMappings();
		webApplicationBuilder.Services.AddSingleton(new AppSettings(webApplicationBuilder.Configuration));
		webApplicationBuilder.Services.AddJwtSetup();
		webApplicationBuilder.Services.AddSqlsugarSetup();
		webApplicationBuilder.Services.AddHostedService<EquipmentOptionService>();
		WebApplication webApplication = webApplicationBuilder.Build();
		if (webApplication.Environment.IsDevelopment())
		{
			webApplication.UseSwagger();
			webApplication.UseSwaggerUI();
		}
		webApplication.UseAuthentication();
		webApplication.UseAuthorization();
		webApplication.UseStaticFiles(new StaticFileOptions
		{
			ServeUnknownFileTypes = true,
			DefaultContentType = "binary/octet-stream"
		});
		webApplication.MapControllers();
		webApplication.Run();
	}
}
