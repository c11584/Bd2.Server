using System.Threading.Tasks;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;

namespace Bd2.Server.iServices.Game;

public interface IGameGachaService
{
	Task<GameActionResult> GachaInfo();

	Task<GameActionResult> GachaBuy(GachaBuyRequest request, string uid);
}
