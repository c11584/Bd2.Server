using System;
using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Bd2.Server.Api.Extensions;

public class AutofacPropertityModuleReg : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		Type controllerBaseType = typeof(ControllerBase);
		(from t in builder.RegisterAssemblyTypes(typeof(Program).Assembly)
			where controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType
			select t).PropertiesAutowired();
	}
}
