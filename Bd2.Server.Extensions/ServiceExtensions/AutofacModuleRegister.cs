using System;
using System.IO;
using System.Reflection;
using Autofac;

namespace Bd2.Server.Extensions.ServiceExtensions;

public class AutofacModuleRegister : Autofac.Module
{
	protected override void Load(ContainerBuilder builder)
	{
		Assembly assembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Bd2.Server.Services.dll"));
		builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerDependency()
			.PropertiesAutowired();
	}
}
