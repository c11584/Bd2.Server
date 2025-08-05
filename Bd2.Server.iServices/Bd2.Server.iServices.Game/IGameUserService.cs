using System.Threading.Tasks;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;

namespace Bd2.Server.iServices.Game;

public interface IGameUserService
{
	Task<GameActionResult> JoinUser(string uid);

	Task<GameActionResult> LoginUser(string uid);

	Task<GameActionResult> NoticeInfo();

	Task<GameActionResult> NoticeDetailInfo(int id);

	Task<GameActionResult> ScheduleInfo();

	GameActionResult MailHistoryInfo();

	Task<GameActionResult> ActiveMap(ActiveMapRequest map, string uid);

	Task<GameActionResult> SaveUserPosition(SaveUserPositionRequest position, string uid);

	Task<GameActionResult> DeckSave(DeckSaveRequest deck, string uid);

	Task<GameActionResult> TutorialClear(TutorialClearRequest tutorial, string uid);

	Task<GameActionResult> PresetSave(PresetSaveRequest req, string uid);

	Task<GameActionResult> PresetInfoChange(PresetInfoChangeRequest req, string uid);

	Task<GameActionResult> PresetDelete(PresetDeleteRequest req, string uid);

	Task<GameActionResult> WaypointSave(WaypointSaveRequest req, string uid);

	Task<GameActionResult> PackPreviewInfo(PackPreviewInfoRequest req, string uid);

	Task<GameActionResult> MonsterInfo(MonsterInfoRequest req, string uid);

	Task<GameActionResult> UseRandomBox(UseRandomBoxRequest req, string uid);

	Task<GameActionResult> HuntDispatch(HuntDispatchRequest req, string uid);

	Task<GameActionResult> ChargeCostInfo(string uid);

	Task<GameActionResult> MailOpen(MailOpenRequest req);
}
