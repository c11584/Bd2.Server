using System.Threading.Tasks;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;
using Bd2.Server.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bd2.Server.Api.Controllers.Game;

[Authorize]
[ApiController]
[Route("Game/[Action]")]
public class GuildController : ControllerBase
{
	[HttpPut]
	public async Task<GameActionResult> GuildInfo([FromBody] string data)
	{
		await Task.CompletedTask;
		GuildInfoResponse data2 = new GuildInfoResponse
		{
			GuildInfo = new GuildDBInfo()
		};
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public async Task<GameActionResult> GuildRecommend([FromBody] string data)
	{
		await Task.CompletedTask;
		GuildDBInfo item = new GuildDBInfo
		{
			GuildBaseInfo = new GuildBaseDBInfo
			{
				Id = 10086L,
				Name = "测试工会",
				Icon = 600004,
				IconColor = "C21F21FF"
			},
			JoinType = Define_GuildJoinType.GuildTypeAny,
			Message = "随便写的测试工会，别鸡巴随便乱点",
			UpdateDate = 1720063507000L,
			Date = 1720062668000L,
			MemberCount = 28
		};
		GuildRecommendResponse guildRecommendResponse = new GuildRecommendResponse();
		guildRecommendResponse.GuildInfo.Add(item);
		return ResultUtil.CreateResult(guildRecommendResponse);
	}

	[HttpPut]
	public async Task<GameActionResult> GuildJoinSendInfo([FromBody] string data)
	{
		return await Task.FromResult(ResultUtil.CreateResult(new GuildJoinSendInfoResponse()));
	}
}
