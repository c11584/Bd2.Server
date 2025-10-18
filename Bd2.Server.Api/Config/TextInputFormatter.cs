using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Bd2.Server.Api.Config;

public class TextInputFormatter : InputFormatter
{
	public TextInputFormatter()
	{
		((Collection<string>)base.SupportedMediaTypes).Add("text/plain");
	}

	public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
	{
		HttpRequest request = context.HttpContext.Request;
		using StreamReader reader = new StreamReader(request.Body, Encoding.UTF8);
		return await InputFormatterResult.SuccessAsync(await reader.ReadToEndAsync());
	}

	protected override bool CanReadType(Type type)
	{
		return type == typeof(string);
	}
}
