using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bd2.Server.Common.Managers;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;
using Bd2.Server.Common.Utils;
using Bd2.Server.iServices;
using Bd2.Server.Model.DAO.Game;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using SqlSugar;

namespace Bd2.Server.Services;

public class BatchService : IBatchService
{
	private readonly ILogger<BatchService> _logger;

	private readonly ISqlSugarClient _db;

	private readonly IMapper _mapper;

	public BatchService(ILogger<BatchService> logger, ISqlSugarClient db, IMapper mapper)
	{
		_mapper = mapper;
		_db = db;
		_logger = logger;
	}

	public GameActionResult TotalWarRewardState(string data, string uid)
	{
		return ResultUtil.CreateResult(new TotalWarRewardResponse().ToByteArray(), 252);
	}

	public GameActionResult CommunityRewardInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new CommunityRewardInfoResponse().ToByteArray(), 289);
	}

	public GameActionResult RecipeInfo(string data, string uid)
	{
		RecipeInfoResponse recipeInfoResponse = new RecipeInfoResponse();
		recipeInfoResponse.RecipeId.AddRange(new int[15]
		{
			101, 102, 103, 104, 105, 202, 203, 204, 205, 302,
			303, 307, 402, 503, 504
		});
		return ResultUtil.CreateResult(recipeInfoResponse.ToByteArray(), 46);
	}

	public GameActionResult CharPartnerInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new CharPartnerInfoResponse().ToByteArray(), 66);
	}

	public GameActionResult EventHubInfo(string data, string uid)
	{
		string json = "{\"eventHubInfo\":[{\"endTime\":\"1724198399000\",\"hubId\":6,\"playEndTime\":\"1723593599000\",\"settingInfo\":[{\"eventUid\":[466,467,468,469,470,471,472,473,474,475],\"hubContentType\":1},{\"eventUid\":[476],\"hubContentType\":2,\"slot\":1},{\"eventUid\":[477],\"hubContentType\":2,\"slot\":2},{\"eventUid\":[478],\"hubContentType\":6,\"slot\":3},{\"eventUid\":[18],\"hubContentType\":3,\"slot\":4},{\"eventUid\":[479],\"hubContentType\":9,\"slot\":6},{\"eventUid\":[480],\"hubContentType\":8,\"slot\":7}],\"startTime\":\"1722470400000\",\"uid\":18},{\"endTime\":\"1725494399000\",\"hubId\":13,\"playEndTime\":\"1724889599000\",\"settingInfo\":[{\"eventUid\":[481,482,483,484,485,486,487,488,489,490],\"hubContentType\":1},{\"eventUid\":[491],\"hubContentType\":2,\"slot\":1},{\"eventUid\":[492],\"hubContentType\":2,\"slot\":2},{\"eventUid\":[493],\"hubContentType\":6,\"slot\":3},{\"eventUid\":[19],\"hubContentType\":3,\"slot\":4},{\"eventUid\":[494],\"hubContentType\":9,\"slot\":6},{\"eventUid\":[495],\"hubContentType\":8,\"slot\":7}],\"startTime\":\"1723593600000\",\"uid\":19}]}";
		EventHubInfoResponse.Parser.ParseJson(json);
		return ResultUtil.CreateResult(new EventHubInfoResponse());
	}

	public GameActionResult EventExchangeInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EventExchangeInfoResponse().ToByteArray(), 183);
	}

	public GameActionResult PersonalInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new PersonalInfoResponse().ToByteArray(), 199);
	}

	public GameActionResult MyLikeInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new MyLikeInfoResponse().ToByteArray(), 218);
	}

	public GameActionResult DispatchInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new DispatchInfoResponse().ToByteArray(), 0);
	}

	public GameActionResult TutorialInfo(string data, string uid)
	{
		TutorialInfoResponse tutorialInfoResponse = new TutorialInfoResponse();
		foreach (TutorialClearInfoDao item in (from x in _db.Queryable<TutorialClearInfoDao>()
			where x.Uid == long.Parse(uid)
			select x).ToList())
		{
			tutorialInfoResponse.TutorialClearId.Add(item.TutorialClearId);
		}
		return ResultUtil.CreateResult(tutorialInfoResponse);
	}

	public GameActionResult MonsterHuntDeckInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new MonsterHuntDeckInfoResponse().ToByteArray(), 263);
	}

	public GameActionResult MyRoomItemInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new MyRoomItemInfoResponse().ToByteArray(), 232);
	}

	public GameActionResult EquipPresetInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EquipPresetInfoResponse().ToByteArray(), 253);
	}

	public GameActionResult CashMailInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new CashMailInfoResponse().ToByteArray(), 140);
	}

	public GameActionResult PresetInfo(string data, string uid)
	{
		List<PresetInfoDao> list = (from x in _db.Queryable<PresetInfoDao>()
			where x.Uid == long.Parse(uid)
			select x).ToList();
		List<PresetDBInfo> list2 = new List<PresetDBInfo>();
		foreach (PresetInfoDao item in list)
		{
			PresetDBInfo presetDBInfo = new PresetDBInfo
			{
				PresetName = item.PresetName,
				PresetResourceId = item.PresetResourceId,
				PresetResourceColor = item.PresetResourceColor,
				Slot = item.Slot
			};
			presetDBInfo.DeckInfo.AddRange(JsonConvert.DeserializeObject<RepeatedField<PresetDeckDBInfo>>(item.DeckInfo));
			list2.Add(presetDBInfo);
		}
		PresetInfoResponse presetInfoResponse = new PresetInfoResponse();
		presetInfoResponse.PresetInfo.AddRange(list2);
		return ResultUtil.CreateResult(presetInfoResponse);
	}

	public GameActionResult GuildInitInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new GuildInitInfoResponse().ToByteArray(), 0);
	}

	public GameActionResult HuntDispatchInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new HuntDispatchInfoResponse().ToByteArray(), 189);
	}

	public GameActionResult QuestMaxClearInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(JsonConvert.DeserializeObject<QuestMaxClearInfoResponse>("{\"maxClearInfo\":[{\"maxClearId\":46,\"packId\":1},{\"maxClearId\":43,\"packId\":2},{\"maxClearId\":39,\"packId\":3},{\"maxClearId\":41,\"packId\":4},{\"maxClearId\":45,\"packId\":5},{\"maxClearId\":45,\"packId\":6},{\"maxClearId\":42,\"packId\":7},{\"maxClearId\":38,\"packId\":8},{\"maxClearId\":40,\"packId\":9},{\"maxClearId\":41,\"packId\":10},{\"maxClearId\":43,\"packId\":11},{\"maxClearId\":40,\"packId\":12},{\"maxClearId\":48,\"packId\":13},{\"maxClearId\":48,\"packId\":14},{\"maxClearId\":38,\"packId\":1001},{\"maxClearId\":36,\"packId\":1002},{\"maxClearId\":34,\"packId\":1003},{\"maxClearId\":34,\"packId\":1004},{\"maxClearId\":29,\"packId\":1005},{\"maxClearId\":29,\"packId\":1006},{\"maxClearId\":18,\"packId\":2001},{\"maxClearId\":19,\"packId\":2002},{\"maxClearId\":6,\"packId\":3001},{\"maxClearId\":15,\"packId\":3002},{\"maxClearId\":5,\"packId\":3003},{\"maxClearId\":3,\"packId\":3004},{\"maxClearId\":3,\"packId\":3005}]}"));
	}

	public GameActionResult MonsterHuntScheduleInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new MonsterHuntScheduleInfoResponse());
	}

	public GameActionResult EventScheduleInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EventScheduleInfoResponse());
	}

	public GameActionResult AchievementInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(JsonConvert.DeserializeObject<AchievementInfoResponse>("{\"achievementInfo\":[{\"groupId\":1,\"value\":\"56561\",\"maxClearId\":9},{\"groupId\":2,\"value\":\"4311023\",\"maxClearId\":8},{\"groupId\":3,\"value\":\"3600\",\"maxClearId\":7},{\"groupId\":4,\"value\":\"1875\",\"maxClearId\":5},{\"groupId\":5,\"value\":\"2440\",\"maxClearId\":6},{\"groupId\":6,\"value\":\"432\",\"maxClearId\":3},{\"groupId\":7,\"value\":\"959\",\"maxClearId\":4},{\"groupId\":8,\"value\":\"57000\",\"maxClearId\":9},{\"groupId\":9,\"value\":\"787388\",\"maxClearId\":11},{\"groupId\":10,\"value\":\"500\",\"maxClearId\":4},{\"groupId\":12,\"value\":\"10\"},{\"groupId\":13,\"value\":\"330\",\"maxClearId\":3},{\"groupId\":15,\"value\":\"33\",\"maxClearId\":3},{\"groupId\":16,\"value\":\"10\",\"maxClearId\":3},{\"groupId\":17,\"value\":\"2\",\"maxClearId\":1},{\"groupId\":18,\"value\":\"122\",\"maxClearId\":11},{\"groupId\":19,\"value\":\"15\",\"maxClearId\":4},{\"groupId\":20,\"value\":\"136\",\"maxClearId\":5},{\"groupId\":21,\"value\":\"51\",\"maxClearId\":2},{\"groupId\":22,\"value\":\"58\",\"maxClearId\":2},{\"groupId\":23,\"value\":\"22\",\"maxClearId\":1},{\"groupId\":24,\"value\":\"5\"},{\"groupId\":25,\"value\":\"40\",\"maxClearId\":1},{\"groupId\":27,\"value\":\"7\"},{\"groupId\":28,\"value\":\"72\",\"maxClearId\":2},{\"groupId\":29,\"value\":\"27\",\"maxClearId\":1},{\"groupId\":30,\"value\":\"9\",\"maxClearId\":1},{\"groupId\":31,\"value\":\"36\",\"maxClearId\":3},{\"groupId\":32,\"value\":\"410\",\"maxClearId\":8},{\"groupId\":33,\"value\":\"1\"},{\"groupId\":34,\"value\":\"11\",\"maxClearId\":2},{\"groupId\":35,\"value\":\"55\"},{\"groupId\":40,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":41,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":42,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":43,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":44,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":45,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":46,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":47,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":48,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":74,\"value\":\"3\",\"maxClearId\":1},{\"groupId\":75,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":77,\"value\":\"14\",\"maxClearId\":3},{\"groupId\":78,\"value\":\"9\",\"maxClearId\":2},{\"groupId\":79,\"value\":\"13\",\"maxClearId\":3},{\"groupId\":80,\"value\":\"20\",\"maxClearId\":4},{\"groupId\":81,\"value\":\"2406\",\"maxClearId\":12},{\"groupId\":82,\"value\":\"1\"},{\"groupId\":83,\"value\":\"88\",\"maxClearId\":6},{\"groupId\":84,\"value\":\"55\",\"maxClearId\":1},{\"groupId\":85,\"value\":\"70\",\"maxClearId\":1},{\"groupId\":86,\"value\":\"56\",\"maxClearId\":1},{\"groupId\":87,\"value\":\"2\"},{\"groupId\":88,\"value\":\"2\"},{\"groupId\":89,\"value\":\"1\"},{\"groupId\":90,\"value\":\"52\",\"maxClearId\":6},{\"groupId\":95,\"value\":\"5\",\"maxClearId\":2},{\"groupId\":96,\"value\":\"3\",\"maxClearId\":1},{\"groupId\":97,\"value\":\"2\",\"maxClearId\":1},{\"groupId\":100,\"value\":\"51\",\"maxClearId\":7},{\"groupId\":101,\"value\":\"147\",\"maxClearId\":14},{\"groupId\":102,\"value\":\"232\",\"maxClearId\":5},{\"groupId\":103,\"value\":\"43\",\"maxClearId\":1},{\"groupId\":104,\"value\":\"85\",\"maxClearId\":2},{\"groupId\":105,\"value\":\"38\",\"maxClearId\":7},{\"groupId\":106,\"value\":\"25\",\"maxClearId\":6},{\"groupId\":121,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":149,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":151,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":153,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":168,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":174,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":190,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":191,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":219,\"value\":\"7\",\"maxClearId\":1},{\"groupId\":220,\"value\":\"7\",\"maxClearId\":1},{\"groupId\":290,\"value\":\"200\",\"maxClearId\":6},{\"groupId\":291,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":292,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":293,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":294,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":295,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":296,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":297,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":299,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":300,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":301,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":302,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":303,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":304,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":305,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":306,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":307,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":308,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":309,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":310,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":311,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":312,\"value\":\"4\",\"maxClearId\":1},{\"groupId\":313,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":347,\"value\":\"91\",\"maxClearId\":2},{\"groupId\":348,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":349,\"value\":\"61\",\"maxClearId\":1},{\"groupId\":350,\"value\":\"113\",\"maxClearId\":12},{\"groupId\":351,\"value\":\"906\",\"maxClearId\":12},{\"groupId\":352,\"value\":\"65\",\"maxClearId\":4},{\"groupId\":353,\"value\":\"404\",\"maxClearId\":8},{\"groupId\":354,\"value\":\"39\",\"maxClearId\":3},{\"groupId\":355,\"value\":\"5\",\"maxClearId\":1},{\"groupId\":356,\"value\":\"8\",\"maxClearId\":1},{\"groupId\":357,\"value\":\"29\",\"maxClearId\":3},{\"groupId\":359,\"value\":\"1\",\"maxClearId\":1},{\"groupId\":372,\"value\":\"26\",\"maxClearId\":12},{\"groupId\":373,\"value\":\"7\",\"maxClearId\":1},{\"groupId\":394,\"value\":\"106\",\"maxClearId\":10},{\"groupId\":395,\"value\":\"64\",\"maxClearId\":8},{\"groupId\":396,\"value\":\"9\",\"maxClearId\":2}]}"));
	}

	public GameActionResult MonsterHuntUserInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new MonsterHuntUserInfoResponse());
	}

	public GameActionResult SeasonRewardInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new SeasonRewardInfoResponse
		{
			RewardInfo = 
			{
				new SeasonRewardDBInfo
				{
					PackId = 3003,
					Season = 28,
					IsRewardReceived = true
				},
				new SeasonRewardDBInfo
				{
					PackId = 3001,
					Season = 48,
					IsRewardReceived = true
				}
			}
		}.ToByteArray(), 0);
	}

	public GameActionResult CashShopInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(JsonConvert.DeserializeObject<CashShopInfoResponse>("{\"productInfo\":[{\"groupId\":310001,\"id\":76,\"startTime\":\"1714003200000\",\"endTime\":\"4102444800000\",\"endDelayMinutes\":3},{\"groupId\":310001,\"id\":77,\"startTime\":\"1714003200000\",\"endTime\":\"4102444800000\",\"endDelayMinutes\":5},{\"groupId\":310001,\"id\":110,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":310001,\"id\":111,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":310001,\"id\":112,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":500001,\"id\":120,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":500001,\"id\":119,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":500001,\"id\":118,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":500001,\"id\":117,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5},{\"groupId\":600001,\"id\":32,\"startTime\":\"1721260800000\",\"endTime\":\"1723593600000\",\"endDelayMinutes\":5},{\"groupId\":320001,\"id\":2,\"startTime\":\"1721260800000\",\"endTime\":\"1722470400000\",\"endDelayMinutes\":5}],\"dailyResetTime\":\"1722384000000\",\"weeklyResetTime\":\"1722816000000\",\"monthlyResetTime\":\"1722470400000\"}").ToByteArray(), 60);
	}

	public GameActionResult ItemInfo(string data, string uid)
	{
		List<ItemInfoDao> source = (from x in _db.Queryable<ItemInfoDao>()
			where x.Uid == long.Parse(uid)
			select x).ToList();
		List<ItemSpecialInfoDao> source2 = (from x in _db.Queryable<ItemSpecialInfoDao>()
			where x.Uid == long.Parse(uid)
			select x).ToList();
		List<ItemDBInfo> values = _mapper.Map<List<ItemDBInfo>>(source);
		List<ItemDBInfo> values2 = _mapper.Map<List<ItemDBInfo>>(source2);
		ItemInfoResponse itemInfoResponse = new ItemInfoResponse();
		itemInfoResponse.ItemInfo.AddRange(values);
		itemInfoResponse.ItemInfo.AddRange(values2);
		return ResultUtil.CreateResult(itemInfoResponse);
	}

	public GameActionResult PackInfo(string data, string uid)
	{
		List<QuestInfoDao> list = (from x in _db.Queryable<QuestInfoDao>()
			where x.Uid == long.Parse(uid)
			select x).ToList();
		PackInfoResponse packInfoResponse = new PackInfoResponse();
		foreach (QuestInfoDao item in list)
		{
			int count = JsonConvert.DeserializeObject<List<int>>(item.QuestClear).Count;
			packInfoResponse.PackInfo.Add(new PackDBInfo
			{
				Id = item.PackId,
				ClearQuestCount = count,
				IsPackComplete = (item.IsPackComplete == 1)
			});
		}
		if (packInfoResponse.PackInfo.Where((PackDBInfo x) => x.Id == 3).ToList().Count != 0)
		{
			int[] array = new int[6] { 3001, 3002, 3003, 3004, 3005, 3006 };
			foreach (int id in array)
			{
				if (packInfoResponse.PackInfo.All((PackDBInfo x) => x.Id != id))
				{
					packInfoResponse.PackInfo.Add(new PackDBInfo
					{
						Id = id
					});
				}
			}
		}
		return ResultUtil.CreateResult(packInfoResponse);
	}

	public GameActionResult FriendInfoList(string data, string uid)
	{
		return ResultUtil.CreateResult(new FriendInfoListResponse().ToByteArray(), 204);
	}

	public GameActionResult PassInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new PassInfoResponse());
	}

	public GameActionResult CharInfo(string data, string uid)
	{
		List<CharInfoDao> source = (from x in _db.Queryable<CharInfoDao>()
			where x.Uid == (long)int.Parse(uid)
			select x).ToList();
		List<CharDBInfo> values = _mapper.Map<List<CharDBInfo>>(source);
		CharInfoResponse charInfoResponse = new CharInfoResponse();
		charInfoResponse.CharInfo.AddRange(values);
		return ResultUtil.CreateResult(charInfoResponse);
	}

	public GameActionResult CostumeInfo(string data, string uid)
	{
		List<CostumeInfoDao> source = (from x in _db.Queryable<CostumeInfoDao>()
			where x.Uid == (long)int.Parse(uid)
			select x).ToList();
		List<CostumeDBInfo> values = _mapper.Map<List<CostumeDBInfo>>(source);
		CostumeInfoResponse costumeInfoResponse = new CostumeInfoResponse();
		costumeInfoResponse.CostumeInfo.AddRange(values);
		return ResultUtil.CreateResult(costumeInfoResponse);
	}

	public GameActionResult CharScoutInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(JsonConvert.DeserializeObject<CharScoutInfoResponse>("{ \"appearCharId\": [ 6040, 6340 ], \"nextAutoResetTime\": \"1722872609457\" }").ToByteArray(), 0);
	}

	public GameActionResult PvpBattleUserInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new PvpBattleUserInfoResponse
		{
			BaseInfo = new PvpBattleUserBaseInfo
			{
				Vp = 1000
			},
			PvpTableChangeSeason = 30
		}.ToByteArray(), 86);
	}

	public GameActionResult MissionInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new MissionInfoResponse());
	}

	public GameActionResult EventMissionInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EventMissionInfoResponse());
	}

	public GameActionResult MailInfo(string data, string uid)
	{
		MailInfoRequest mailInfoRequest = new MailInfoRequest();
		try
		{
			mailInfoRequest = MailInfoRequest.Parser.ParseFrom(Convert.FromBase64String(data));
		}
		catch (Exception)
		{
			mailInfoRequest = ProtoUtil.Deserialize<MailInfoRequest>(data);
		}
		_logger.LogInformation("邮件 {}", mailInfoRequest);
		List<MailDBInfo> list = new List<MailDBInfo>();
		foreach (MailInfoDao item in (from x in _db.Queryable<MailInfoDao>()
			where x.Uid == (long)int.Parse(uid) && x.InvenIndex > mailInfoRequest.StartInvenIndex && x.IsOpen == 0
			select x).ToList())
		{
			MailDBInfo mailDBInfo = new MailDBInfo
			{
				InvenIndex = item.InvenIndex,
				Type = item.Type,
				MailId = item.MailId,
				RewardExpireTime = item.RewardExpireTime,
				IsOpen = (item.IsOpen == 1),
				OpenTime = item.OpenTime,
				CreateTime = item.CreateTime,
				HistoryDeleteTime = item.HistoryDeleteTime,
				IsCash = (item.IsCash == 1)
			};
			if (item.SenderText != null)
			{
				mailDBInfo.SenderText = item.SenderText;
			}
			if (item.MessageText != null)
			{
				mailDBInfo.MessageText = item.MessageText;
			}
			if (item.TitleText != null)
			{
				mailDBInfo.TitleText = item.TitleText;
			}
			mailDBInfo.ItemId.Add(item.ItemId);
			mailDBInfo.ItemCount.Add(item.ItemCount);
			mailDBInfo.ItemType.Add(item.ItemType);
			list.Add(mailDBInfo);
		}
		MailInfoResponse mailInfoResponse = new MailInfoResponse();
		mailInfoResponse.MailInfo.AddRange(list);
		mailInfoResponse.TotalCount = list.Count;
		return ResultUtil.CreateResult(mailInfoResponse);
	}

	public GameActionResult EvilCastleRogueLikeAllUserScoreInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EvilCastleRogueLikeAllUserScoreInfoResponse
		{
			AllUserTotalScore = 10086L
		});
	}

	public GameActionResult EvilCastleInfo(string data, string uid)
	{
		EvilCastleInfoRequest evilCastleInfoRequest = EvilCastleInfoRequest.Parser.ParseFrom(Convert.FromBase64String(data));
		_logger.LogWarning("批量恶魔城信息请求: {}", evilCastleInfoRequest);
		return ResultUtil.CreateResult(new EvilCastleInfoResponse
		{
			EvilCastleTotalInfo = new EvilCastleTotalDBInfo(),
			Season = 31,
			RegularSeason = 6
		});
	}

	public GameActionResult EvilCastleTowerInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EvilCastleTowerInfoResponse());
	}

	public GameActionResult EvilCastleEnter(string data, string uid)
	{
		return ResultUtil.CreateResult(new EvilCastleEnterResponse());
	}

	public GameActionResult EvilCastleRogueLikeInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EvilCastleRogueLikeInfoResponse());
	}

	public GameActionResult EvilCastleRewardInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new EvilCastleRewardInfoResponse
		{
			EndSeasonTotalInfo = new EvilCastleEndSeasonTotalDBInfo(),
			RewardInfoBundle = new RewardDBInfoBundle()
		});
	}

	public GameActionResult CharAwakeInfo(string data, string uid)
	{
		List<CharAwakeInfoDao> list = (from x in _db.Queryable<CharAwakeInfoDao>()
			where x.OwnerIndex == long.Parse(uid)
			select x).ToList();
		CharAwakeInfoResponse charAwakeInfoResponse = new CharAwakeInfoResponse();
		foreach (CharAwakeInfoDao item in list)
		{
			charAwakeInfoResponse.CharAwakeInfo.Add(new CharAwakeDBInfo
			{
				UniqueCharId = item.UniqueCharId,
				ImprintSlot1Level = item.ImprintSlot1Level,
				ImprintSlot2Level = item.ImprintSlot2Level,
				ImprintSlot3Level = item.ImprintSlot3Level,
				IsAwake = (item.IsAwake == 1),
				OwnerIndex = item.OwnerIndex
			});
		}
		return ResultUtil.CreateResult(charAwakeInfoResponse);
	}

	public GameActionResult FieldTrapInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new FieldTrapInfoResponse());
	}

	public GameActionResult WaypointInfo(string data, string uid)
	{
		FieldTrapInfoRequest fieldTrapInfoRequest = FieldTrapInfoRequest.Parser.ParseFrom(Convert.FromBase64String(data));
		WayPointInfoDao wayPointInfoDao = _db.Queryable<WayPointInfoDao>().Single((WayPointInfoDao it) => it.Uid == long.Parse(uid) && it.PackId == fieldTrapInfoRequest.PackId);
		WaypointInfoResponse waypointInfoResponse = new WaypointInfoResponse();
		if (wayPointInfoDao != null)
		{
			waypointInfoResponse.WaypointId.AddRange(JsonConvert.DeserializeObject<int[]>(wayPointInfoDao.PointId));
		}
		return ResultUtil.CreateResult(waypointInfoResponse);
	}

	public GameActionResult SkyWayScheduleInfo(string data, string uid)
	{
		string json = "{ \"scheduleInfo\": [ { \"groupId\": 1, \"day\": [ 2, 4, 5, 6, 0 ], \"bonusRate\": 100 }, { \"groupId\": 2, \"day\": [ 1, 3, 5, 6, 0 ], \"bonusRate\": 100 }, { \"groupId\": 3 }, { \"groupId\": 4 }, { \"groupId\": 5 }, { \"groupId\": 6 }, { \"groupId\": 7 } ] }";
		return ResultUtil.CreateResult(SkyWayScheduleInfoResponse.Parser.ParseJson(json));
	}

	public GameActionResult DeckInfo(string data, string uid)
	{
		DeckInfoResponse deckInfoResponse = new DeckInfoResponse();
		foreach (DeckInfoDao item in (from it in _db.Queryable<DeckInfoDao>()
			where it.Uid == long.Parse(uid)
			select it).ToList())
		{
			deckInfoResponse.DeckInfo.Add(_mapper.Map<DeckDBInfo>(item));
		}
		deckInfoResponse.TalentSlotInfo.AddRange(new int[4]);
		return ResultUtil.CreateResult(deckInfoResponse);
	}

	public GameActionResult FieldObjectInfo(string data, string uid)
	{
		return ResultUtil.CreateResult(new FieldObjectInfoResponse().ToByteArray(), 28);
	}

	public GameActionResult SkyWayInfo(string data, string uid)
	{
		string json = "{ \"skyWayInfo\": [ { \"groupId\": 1, \"currentId\": 12, \"maxClearLevel\": 2 }, { \"groupId\": 2, \"currentId\": 22, \"maxClearLevel\": 2 }, { \"groupId\": 3, \"currentId\": 32, \"maxClearLevel\": 2 }, { \"groupId\": 4, \"currentId\": 42, \"maxClearLevel\": 2 }, { \"groupId\": 5, \"currentId\": 52, \"maxClearLevel\": 2 }, { \"groupId\": 6, \"currentId\": 62, \"maxClearLevel\": 2 }, { \"groupId\": 7, \"currentId\": 72, \"maxClearLevel\": 2 } ] }";
		return ResultUtil.CreateResult(SkyWayInfoResponse.Parser.ParseJson(json));
	}

	public GameActionResult PackInGameInfo(string data, string uid)
	{
		PackInGameInfoRequest packInGameInfoRequest = PackInGameInfoRequest.Parser.ParseFrom(Convert.FromBase64String(data));
		PackInGameInfoResponse packInGameInfoResponse = new PackInGameInfoResponse();
		PositionInfoDao positionInfoDao = _db.Queryable<PositionInfoDao>().Single((PositionInfoDao x) => x.Uid == long.Parse(uid) && x.PackId == packInGameInfoRequest.PackId);
		packInGameInfoResponse.Position = ((positionInfoDao == null) ? "{}" : positionInfoDao.Position);
		foreach (MapActiveInfoDao item in (from x in _db.Queryable<MapActiveInfoDao>()
			where x.Uid == long.Parse(uid) && x.PackId == packInGameInfoRequest.PackId
			select x).ToList())
		{
			MapActiveInfo mapActiveInfo = new MapActiveInfo
			{
				MapId = item.MapId
			};
			mapActiveInfo.ActiveInfo.AddRange(JsonConvert.DeserializeObject<string[]>(item.Active));
			packInGameInfoResponse.MapActiveInfo.Add(mapActiveInfo);
		}
		packInGameInfoResponse.HuntingGroundInfo = new HuntingGroundDBInfo();
		QuestInfoDao questInfoDao = _db.Queryable<QuestInfoDao>().Single((QuestInfoDao x) => x.PackId == packInGameInfoRequest.PackId && x.Uid == long.Parse(uid));
		if (questInfoDao == null)
		{
			packInGameInfoResponse.QuestInfo.Add(new QuestDBInfo
			{
				Id = 1
			});
		}
		else
		{
			packInGameInfoResponse.ClearQuestIds.AddRange(JsonConvert.DeserializeObject<int[]>(questInfoDao.QuestClear));
			if (questInfoDao.QuestId != 0)
			{
				packInGameInfoResponse.QuestInfo.Add(new QuestDBInfo
				{
					Id = questInfoDao.QuestId
				});
			}
		}
		(from it in _db.Updateable<UserInfoDao>().SetColumns((UserInfoDao x) => x.LastPlayPackId == packInGameInfoRequest.PackId)
			where it.OwnerIndex == long.Parse(uid)
			select it).ExecuteCommand();
		if (packInGameInfoRequest.PackId < 1010)
		{
			List<MonsterDBInfo> monsterInfoListByPackId = BattleDeckManager.GetMonsterInfoListByPackId(packInGameInfoRequest.PackId);
			packInGameInfoResponse.MonsterInfo.AddRange(monsterInfoListByPackId);
		}
		packInGameInfoResponse.ReputationInfo.Add(new ReputationDBInfo
		{
			GroupId = 1,
			State = 1,
			ElapsedSeconds = 10086
		});
		return ResultUtil.CreateResult(packInGameInfoResponse);
	}
}
