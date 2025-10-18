using System.Threading.Tasks;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;

namespace Bd2.Server.iServices.Game;

public interface IGameQuestService
{
	Task<GameActionResult> QuestUpdate(QuestUpdateRequest req, string uid);

	Task<GameActionResult> QuestClear(QuestClearRequest req, string uid);
}
