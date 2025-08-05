using System;
using System.Collections.Generic;
using System.Reflection;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Model.Game;
using Bd2.Server.Common.Utils;
using Bd2.Server.iServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bd2.Server.Api.Controllers.Game;

[Authorize]
[ApiController]
[Route("Game/[Action]")]
public class BatchController : ControllerBase
{
	private readonly ILogger<BatchController> _logger;

	private readonly IBatchService _batchService;

	public BatchController(ILogger<BatchController> logger, IBatchService batchService)
	{
		_logger = logger;
		_batchService = batchService;
	}

	[HttpPut]
	public List<Dictionary<string, object>> BatchRequest([FromBody] string data)
	{
		string value = AesUtils.AesDecrypt(data);
		List<BatchRequestModel> list = JsonConvert.DeserializeObject<List<BatchRequestModel>>(value);
		Type typeFromHandle = typeof(IBatchService);
		MethodInfo[] methods = typeFromHandle.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		string value2 = base.User.FindFirst("user").Value;
		foreach (BatchRequestModel batchRequestModel in list)
		{
			int num = Array.FindIndex(methods, delegate(MethodInfo method)
			{
				string name = method.Name;
				string path = batchRequestModel.path;
				return name == path.Substring(1, path.Length - 1);
			});
			if (num == -1)
			{
				break;
			}
			GameActionResult value3 = (GameActionResult)methods[num].Invoke(_batchService, new object[2] { batchRequestModel.requestData, value2 });
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("path", "/" + methods[num].Name);
			dictionary.Add("responseData", value3);
			list2.Add(dictionary);
		}
		return list2;
	}

	[HttpPut]
	public GameActionResult MissionInfo([FromBody] string data)
	{
		return _batchService.MissionInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult TotalWarRewardState([FromBody] string data)
	{
		return _batchService.TotalWarRewardState(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CommunityRewardInfo([FromBody] string data)
	{
		return _batchService.CommunityRewardInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult RecipeInfo([FromBody] string data)
	{
		return _batchService.RecipeInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CharPartnerInfo([FromBody] string data)
	{
		return _batchService.CharPartnerInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EventHubInfo([FromBody] string data)
	{
		return _batchService.EventHubInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EventExchangeInfo([FromBody] string data)
	{
		return _batchService.EventExchangeInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult PersonalInfo([FromBody] string data)
	{
		return _batchService.PersonalInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult MyLikeInfo([FromBody] string data)
	{
		return _batchService.MyLikeInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult DispatchInfo([FromBody] string data)
	{
		return _batchService.DispatchInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult TutorialInfo([FromBody] string data)
	{
		return _batchService.TutorialInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult MonsterHuntDeckInfo([FromBody] string data)
	{
		return _batchService.MonsterHuntDeckInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult MyRoomItemInfo([FromBody] string data)
	{
		return _batchService.MyRoomItemInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EquipPresetInfo([FromBody] string data)
	{
		return _batchService.EquipPresetInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CashMailInfo([FromBody] string data)
	{
		return _batchService.CashMailInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult PresetInfo([FromBody] string data)
	{
		return _batchService.PresetInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult GuildInitInfo([FromBody] string data)
	{
		return _batchService.GuildInitInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult HuntDispatchInfo([FromBody] string data)
	{
		return _batchService.HuntDispatchInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult QuestMaxClearInfo([FromBody] string data)
	{
		return _batchService.QuestMaxClearInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult MonsterHuntScheduleInfo([FromBody] string data)
	{
		return _batchService.MonsterHuntScheduleInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EventScheduleInfo([FromBody] string data)
	{
		return _batchService.EventScheduleInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult AchievementInfo([FromBody] string data)
	{
		return _batchService.AchievementInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult MonsterHuntUserInfo([FromBody] string data)
	{
		return _batchService.MonsterHuntUserInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult SeasonRewardInfo([FromBody] string data)
	{
		return _batchService.SeasonRewardInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CashShopInfo([FromBody] string data)
	{
		return _batchService.CashShopInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult ItemInfo([FromBody] string data)
	{
		return _batchService.ItemInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult PackInfo([FromBody] string data)
	{
		return _batchService.PackInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult FriendInfoList([FromBody] string data)
	{
		return _batchService.FriendInfoList(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult PassInfo([FromBody] string data)
	{
		return _batchService.PassInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CharInfo([FromBody] string data)
	{
		return _batchService.CharInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CostumeInfo([FromBody] string data)
	{
		return _batchService.CostumeInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CharScoutInfo([FromBody] string data)
	{
		return _batchService.CharScoutInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult PvpBattleUserInfo([FromBody] string data)
	{
		return _batchService.PvpBattleUserInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EventMissionInfo([FromBody] string data)
	{
		return _batchService.EventMissionInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult FieldTrapInfo([FromBody] string data)
	{
		return _batchService.FieldTrapInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult WaypointInfo([FromBody] string data)
	{
		return _batchService.WaypointInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult SkyWayScheduleInfo([FromBody] string data)
	{
		return _batchService.SkyWayScheduleInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult DeckInfo([FromBody] string data)
	{
		return _batchService.DeckInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult FieldObjectInfo([FromBody] string data)
	{
		return _batchService.FieldObjectInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult SkyWayInfo([FromBody] string data)
	{
		return _batchService.SkyWayInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult PackInGameInfo([FromBody] string data)
	{
		return _batchService.PackInGameInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult MailInfo([FromBody] string data)
	{
		return _batchService.MailInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EvilCastleRogueLikeAllUserScoreInfo([FromBody] string data)
	{
		return _batchService.EvilCastleRogueLikeAllUserScoreInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EvilCastleRogueLikeInfo([FromBody] string data)
	{
		return _batchService.EvilCastleRogueLikeInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EvilCastleEnter([FromBody] string data)
	{
		return _batchService.EvilCastleEnter(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EvilCastleRewardInfo([FromBody] string data)
	{
		return _batchService.EvilCastleRewardInfo(data, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult CharAwakeInfo([FromBody] string data)
	{
		return _batchService.CharAwakeInfo(data, base.User.FindFirst("user").Value);
	}
}
