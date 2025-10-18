using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;
using Bd2.Server.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bd2.Server.Api.Controllers.Game;

[Authorize]
[ApiController]
[Route("Game/[Action]")]
public class EvilCastleController : ControllerBase
{
	private readonly ILogger<EvilCastleController> _logger;

	public EvilCastleController(ILogger<EvilCastleController> logger)
	{
		_logger = logger;
	}

	[HttpPut]
	public GameActionResult EvilCastleInfo([FromBody] string data)
	{
		EvilCastleInfoRequest evilCastleInfoRequest = ProtoUtil.Deserialize<EvilCastleInfoRequest>(data);
		_logger.LogWarning("恶魔城信息请求: {}", evilCastleInfoRequest);
		EvilCastleInfoResponse data2 = new EvilCastleInfoResponse
		{
			EvilCastleTotalInfo = new EvilCastleTotalDBInfo(),
			Season = 31,
			RegularSeason = 6
		};
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public GameActionResult EvilCastleTowerInfo()
	{
		return ResultUtil.CreateResult(new EvilCastleTowerInfoResponse());
	}
}
