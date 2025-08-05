using Bd2.Server.Common.Model;

namespace Bd2.Server.iServices;

public interface IBatchService
{
	GameActionResult TotalWarRewardState(string data, string uid);

	GameActionResult CommunityRewardInfo(string data, string uid);

	GameActionResult RecipeInfo(string data, string uid);

	GameActionResult CharPartnerInfo(string data, string uid);

	GameActionResult EventHubInfo(string data, string uid);

	GameActionResult EventExchangeInfo(string data, string uid);

	GameActionResult PersonalInfo(string data, string uid);

	GameActionResult MyLikeInfo(string data, string uid);

	GameActionResult DispatchInfo(string data, string uid);

	GameActionResult TutorialInfo(string data, string uid);

	GameActionResult MonsterHuntDeckInfo(string data, string uid);

	GameActionResult MyRoomItemInfo(string data, string uid);

	GameActionResult EquipPresetInfo(string data, string uid);

	GameActionResult CashMailInfo(string data, string uid);

	GameActionResult PresetInfo(string data, string uid);

	GameActionResult GuildInitInfo(string data, string uid);

	GameActionResult HuntDispatchInfo(string data, string uid);

	GameActionResult QuestMaxClearInfo(string data, string uid);

	GameActionResult MonsterHuntScheduleInfo(string data, string uid);

	GameActionResult EventScheduleInfo(string data, string uid);

	GameActionResult AchievementInfo(string data, string uid);

	GameActionResult MonsterHuntUserInfo(string data, string uid);

	GameActionResult SeasonRewardInfo(string data, string uid);

	GameActionResult CashShopInfo(string data, string uid);

	GameActionResult ItemInfo(string data, string uid);

	GameActionResult PackInfo(string data, string uid);

	GameActionResult FriendInfoList(string data, string uid);

	GameActionResult PassInfo(string data, string uid);

	GameActionResult CharInfo(string data, string uid);

	GameActionResult CostumeInfo(string data, string uid);

	GameActionResult CharScoutInfo(string data, string uid);

	GameActionResult PvpBattleUserInfo(string data, string uid);

	GameActionResult MissionInfo(string data, string uid);

	GameActionResult EventMissionInfo(string data, string uid);

	GameActionResult FieldTrapInfo(string data, string uid);

	GameActionResult WaypointInfo(string data, string uid);

	GameActionResult SkyWayScheduleInfo(string data, string uid);

	GameActionResult DeckInfo(string data, string uid);

	GameActionResult FieldObjectInfo(string data, string uid);

	GameActionResult SkyWayInfo(string data, string uid);

	GameActionResult PackInGameInfo(string data, string uid);

	GameActionResult MailInfo(string data, string uid);

	GameActionResult EvilCastleRogueLikeAllUserScoreInfo(string data, string uid);

	GameActionResult EvilCastleInfo(string data, string uid);

	GameActionResult EvilCastleTowerInfo(string data, string uid);

	GameActionResult EvilCastleEnter(string data, string uid);

	GameActionResult EvilCastleRogueLikeInfo(string data, string uid);

	GameActionResult EvilCastleRewardInfo(string data, string uid);

	GameActionResult CharAwakeInfo(string data, string uid);
}
