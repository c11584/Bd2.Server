using System;
using System.Collections.Generic;
using System.Text;
using Bd2.Server.Common;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Model.Game;
using Bd2.Server.Common.Proto.Net;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bd2.Server.Api.Controllers.Common;

[ApiController]
[Route("[controller]")]
public class CommonController : ControllerBase
{
	private readonly ILogger<CommonController> _logger;

	public CommonController(ILogger<CommonController> logger)
	{
		_logger = logger;
	}

	[HttpPost("StateCheckInfoJson")]
	public Dictionary<string, object> StateCheckInfoJson()
	{
		return new Dictionary<string, object>
		{
			{ "country", "CN" },
			{ "errorType", 0 },
			{ "errorMessage", "" }
		};
	}

	[HttpPost("api/accounts/v3/weblogin/login/webtoken")]
	public Dictionary<string, object> GetWebToken([FromForm] string neon_web_token)
	{
		_logger.LogInformation("GetWebToken: {}", neon_web_token);
		string text = Encoding.UTF8.GetString(Convert.FromBase64String(neon_web_token));
		string[] array = text.Split("|", StringSplitOptions.RemoveEmptyEntries);
		WebToken webToken = new WebToken();
		Member member = new Member
		{
			member_id = int.Parse(array[0])
		};
		webToken.access_token = text;
		webToken.member = member;
		return new Dictionary<string, object>
		{
			{ "result_code", "000" },
			{ "result_msg", "API_OK" },
			{ "value", webToken }
		};
	}

	[HttpPost("api/gpg/check/user")]
	public Dictionary<string, object> CheckUser()
	{
		return new Dictionary<string, object>
		{
			{ "result_code", "000" },
			{ "result_msg", "API_OK" },
			{ "value", false }
		};
	}

	[HttpPut("MaintenanceInfo")]
	public BaseActionResult MaintenanceInfo()
	{
		MaintenanceInfoResponse maintenanceInfoResponse = new MaintenanceInfoResponse();
		MaintenanceInfo marketInfo = new MaintenanceInfo
		{
			Version = "1.68.11",
			BundleVersion = "20240828125914",
			Date = "2023-09-18 00-00-00",
			UseDsa = 1,
			MaintenanceUrl = "http://mwe.dq.pmang.com/:brown_Event/maintenance?e=900&market_type=*",
			DownloadUrl = "https://www.browndust2.com/",
			BundleVersionSd = "1.0.0",
			MarketType = 4
		};
		maintenanceInfoResponse.MarketInfo = marketInfo;
		return new BaseActionResult
		{
			data = Convert.ToBase64String(maintenanceInfoResponse.ToByteArray())
		};
	}

	[HttpPut("ServerInfo")]
	public BaseActionResult ServerInfo()
	{
		string text = AppSettings.app("GameServer");
		ServerInfoResponse.Types.ServerInfo item = new ServerInfoResponse.Types.ServerInfo
		{
			GameServerInfo = text + "Game/",
			CdnInfo = text + "ServerData",
			LogServerInfo = text + "Loki/push",
			CharServerInfo = "127.0.0.1:38501",
			CouponWebInfo = text + "mail/index.html"
		};
		ServerInfoResponse serverInfoResponse = new ServerInfoResponse();
		serverInfoResponse.InfoList.Add(item);
		return new BaseActionResult
		{
			data = Convert.ToBase64String(serverInfoResponse.ToByteArray())
		};
	}

	[HttpPost("api/gpg/price/localization")]
	public Dictionary<string, object> GetPriceLocalization()
	{
		string value = "{\"skus\":{\"brd2_dia_500\":{\"price\":\"230\",\"priceMicros\":\"230000000\",\"formattedPrice\":\"NT$230.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_40\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_41\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_42\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_43\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_pack_pick_30\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_4\":{\"price\":\"2050\",\"priceMicros\":\"2050000000\",\"formattedPrice\":\"NT$2,050.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_5\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_dia_3000\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_6\":{\"price\":\"999\",\"priceMicros\":\"999000000\",\"formattedPrice\":\"NT$999.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_7\":{\"price\":\"1499\",\"priceMicros\":\"1499000000\",\"formattedPrice\":\"NT$1,499.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_1\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_2\":{\"price\":\"999\",\"priceMicros\":\"999000000\",\"formattedPrice\":\"NT$999.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_3\":{\"price\":\"1499\",\"priceMicros\":\"1499000000\",\"formattedPrice\":\"NT$1,499.00\",\"currency\":\"TWD\"},\"brd2_dia_100\":{\"price\":\"50\",\"priceMicros\":\"50000000\",\"formattedPrice\":\"NT$50.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_33\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_34\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_35\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_250x2\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_36\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_8\":{\"price\":\"2050\",\"priceMicros\":\"2050000000\",\"formattedPrice\":\"NT$2,050.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_37\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_9\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_38\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_39\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_50\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_51\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_52\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_6500x2\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_53\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_54\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_44\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_45\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_46\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_47\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_1800x2\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_48\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_49\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_60\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_61\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_62\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_daily_pack\":{\"price\":\"30\",\"priceMicros\":\"30000000\",\"formattedPrice\":\"NT$30.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_63\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_64\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_65\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_pack_pick_50\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_dia_1000\":{\"price\":\"449\",\"priceMicros\":\"449000000\",\"formattedPrice\":\"NT$449.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_55\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_dia_1800\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_56\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_57\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_58\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_59\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_monthly_pack_1\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_monthly_pack_2\":{\"price\":\"230\",\"priceMicros\":\"230000000\",\"formattedPrice\":\"NT$230.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_70\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_71\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_72\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_73\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_74\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_75\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_76\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_66\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_67\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_68\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_69\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_dia_250\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_80\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_81\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_82\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_83\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_84\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_85\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_86\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_87\":{\"price\":\"230\",\"priceMicros\":\"230000000\",\"formattedPrice\":\"NT$230.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_500x2\":{\"price\":\"230\",\"priceMicros\":\"230000000\",\"formattedPrice\":\"NT$230.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_77\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_78\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_79\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_storypack_clear_package_1\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_storypack_clear_package_2\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_91\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_92\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_93\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_94\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_95\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_96\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_97\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_10\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_98\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_90\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_pack_start2\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_pack_start1\":{\"price\":\"449\",\"priceMicros\":\"449000000\",\"formattedPrice\":\"NT$449.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_1000x2\":{\"price\":\"449\",\"priceMicros\":\"449000000\",\"formattedPrice\":\"NT$449.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_88\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_89\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_100x2\":{\"price\":\"50\",\"priceMicros\":\"50000000\",\"formattedPrice\":\"NT$50.00\",\"currency\":\"TWD\"},\"brd2_pack_pick_10\":{\"price\":\"230\",\"priceMicros\":\"230000000\",\"formattedPrice\":\"NT$230.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_20\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_21\":{\"price\":\"799\",\"priceMicros\":\"799000000\",\"formattedPrice\":\"NT$799.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_19\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_11\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_12\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_13\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_14\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_15\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_16\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_17\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_18\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_pack_pick_100\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_30\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_31\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_32\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_1st_dia_3000x2\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_dia_6500\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_23\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_24\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_25\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_26\":{\"price\":\"125\",\"priceMicros\":\"125000000\",\"formattedPrice\":\"NT$125.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_27\":{\"price\":\"530\",\"priceMicros\":\"530000000\",\"formattedPrice\":\"NT$530.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_28\":{\"price\":\"1299\",\"priceMicros\":\"1299000000\",\"formattedPrice\":\"NT$1,299.00\",\"currency\":\"TWD\"},\"brd2_limited_pack_29\":{\"price\":\"2780\",\"priceMicros\":\"2780000000\",\"formattedPrice\":\"NT$2,780.00\",\"currency\":\"TWD\"}}}";
		Dictionary<string, Dictionary<string, Dictionary<string, string>>> value2 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(value);
		return new Dictionary<string, object>
		{
			{ "result_code", "000" },
			{ "result_msg", "API_OK" },
			{ "value", value2 }
		};
	}
}
