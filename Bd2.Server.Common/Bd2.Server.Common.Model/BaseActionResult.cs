namespace Bd2.Server.Common.Model;

public class BaseActionResult
{
	public int errorType { get; set; }

	public string data { get; set; }

	public int length { get; set; }

	public string ip { get; set; } = "127.0.0.1";
}
