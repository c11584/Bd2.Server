using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bd2.Server.Common.Model;
using Bd2.Server.Common.Proto.Net;
using Bd2.Server.Common.Utils;
using Bd2.Server.iServices.Game;
using Bd2.Server.Model.DAO.Game;
using EasyTool;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SqlSugar;

namespace Bd2.Server.Api.Controllers.Game;

[ApiController]
[Authorize]
[Route("Game/[Action]")]
public class UserController : ControllerBase
{
	private readonly ILogger<UserController> _logger;

	private readonly ISqlSugarClient _db;

	private readonly IGameUserService _gameUserService;

	private readonly IMapper _mapper;

	public UserController(ILogger<UserController> logger, ISqlSugarClient db, IGameUserService gameUserService, IMapper mapper)
	{
		_mapper = mapper;
		_db = db;
		_logger = logger;
		_gameUserService = gameUserService;
	}

	private static string BuildToken(string uid)
	{
		int num = 1;
		List<Claim> list = new List<Claim>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Claim> span = CollectionsMarshal.AsSpan(list);
		int num2 = 0;
		span[num2] = new Claim("user", uid);
		num2++;
		List<Claim> claims = list;
		DateTime value = DateTime.Now.AddSeconds(86400.0);
		byte[] bytes = Encoding.UTF8.GetBytes("1bece8b1c0cfa6ed404d0473d920f2a8");
		SymmetricSecurityKey key = new SymmetricSecurityKey(bytes);
		SigningCredentials signingCredentials = new SigningCredentials(key, "HS256");
		DateTime? expires = value;
		SigningCredentials signingCredentials2 = signingCredentials;
		JwtSecurityToken token = new JwtSecurityToken("blue", "blue", claims, null, expires, signingCredentials2);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private static string CheckAccessToken(string accessToken)
	{
		string[] array = accessToken.Split("|", StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 6)
		{
			return "";
		}
		if (!(array[4] != AesUtils.Sha1Hash(array[0] + array[5])))
		{
			return array[0];
		}
		return "";
	}

	[HttpPut]
	[AllowAnonymous]
	public async Task<GameActionResult> JoinUser([FromBody] string data)
	{
		JoinUserRequest joinUserRequest = ProtoUtil.Deserialize<JoinUserRequest>(data);
		string text = CheckAccessToken(joinUserRequest.AccessToken);
		if (text == "")
		{
			return ResultUtil.CreateErrorResult();
		}
		base.Response.Cookies.Append("token", BuildToken(text));
		return await _gameUserService.JoinUser(text);
	}

	[HttpPut]
	[AllowAnonymous]
	public async Task<GameActionResult> LoginUser([FromBody] string data)
	{
		LoginUserRequest loginUserRequest = ProtoUtil.Deserialize<LoginUserRequest>(data);
		_logger.LogInformation("LoginUser {}", loginUserRequest);
		string text = CheckAccessToken(loginUserRequest.AccessToken);
		if (text == "")
		{
			return ResultUtil.CreateErrorResult();
		}
		base.Response.Cookies.Append("token", BuildToken(text));
		return await _gameUserService.LoginUser(text);
	}

	[HttpPut]
	[AllowAnonymous]
	public GameActionResult RefreshToken([FromBody] string data)
	{
		RefreshTokenRequest refreshTokenRequest = ProtoUtil.Deserialize<RefreshTokenRequest>(data);
		string[] array = refreshTokenRequest.AccessToken.Split("|", StringSplitOptions.RemoveEmptyEntries);
		array[5] = TimestampUtil.GetCurrentTimestamp().ToString();
		array[4] = AesUtils.Sha1Hash(array[0] + array[5]);
		RefreshTokenResponse message = new RefreshTokenResponse
		{
			AccessToken = string.Join("|", array)
		};
		return ResultUtil.CreateResult(message.ToByteArray(), 186);
	}

	[HttpPut]
	public GameActionResult BalanceVersionCheck()
	{
		BalanceVersionCheckResponse data = new BalanceVersionCheckResponse();
		return ResultUtil.CreateResult(data);
	}

	[HttpPut]
	public GameActionResult LoginEvent()
	{
		LoginEventResponse data = new LoginEventResponse();
		return ResultUtil.CreateResult(data);
	}

	[HttpPut]
	public async Task<GameActionResult> NoticeInfo()
	{
		return await _gameUserService.NoticeInfo();
	}

	[HttpPut]
	public async Task<GameActionResult> NoticeDetailInfo([FromBody] string data)
	{
		NoticeDetailInfoRequest noticeDetailInfoRequest = ProtoUtil.Deserialize<NoticeDetailInfoRequest>(data);
		return await _gameUserService.NoticeDetailInfo(noticeDetailInfoRequest.NoticeId);
	}

	[HttpPut]
	public async Task<GameActionResult> ScheduleInfo()
	{
		return await _gameUserService.ScheduleInfo();
	}

	[HttpPut]
	public async Task<GameActionResult> ChargeCostInfo()
	{
		return await _gameUserService.ChargeCostInfo(base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult Attendance()
	{
		AttendanceInfoResponse attendanceInfoResponse = new AttendanceInfoResponse();
		attendanceInfoResponse.AttendanceTime.Add(8L);
		attendanceInfoResponse.AttendanceTime.Add(1L);
		attendanceInfoResponse.AttendanceTime.Add(16L);
		attendanceInfoResponse.AttendanceTime.Add(1L);
		attendanceInfoResponse.AttendanceTime.Add(24L);
		attendanceInfoResponse.AttendanceTime.Add(1L);
		attendanceInfoResponse.AttendanceTime.Add(8L);
		attendanceInfoResponse.AttendanceTime.Add(463L);
		attendanceInfoResponse.AttendanceTime.Add(16L);
		attendanceInfoResponse.AttendanceTime.Add(30L);
		attendanceInfoResponse.AttendanceTime.Add(24L);
		attendanceInfoResponse.AttendanceTime.Add(1L);
		return ResultUtil.CreateResult(attendanceInfoResponse);
	}

	[HttpPut]
	public GameActionResult EventRewardHistory()
	{
		string text = "Cg0IARADGgcBAgMEBQYHCgAKAAoACgAKAAoACgAKAAoACgAKAAoACgAKAAoACgAKAAoACgwIxQMQHRoFAQIDBAUKAAoACgAKAAoACgAKAAoA";
		return new GameActionResult
		{
			packetCode = 0,
			length = text.Length,
			data = AesUtils.AesEncrypt(text)
		};
	}

	[HttpPut]
	public GameActionResult AttendanceInfo()
	{
		string value = "{\"attendanceTime\":[\"1687564800000\",\"1687737600000\",\"1687910400000\",\"1687996800000\",\"1688083200000\",\"1688169600000\",\"1688169600000\",\"1688342400000\",\"1688342400000\",\"1688515200000\",\"1688774400000\",\"1689638400000\",\"1690070400000\",\"1690588800000\",\"1690934400000\",\"1691452800000\",\"1692316800000\",\"1692662400000\",\"1693440000000\",\"1694563200000\",\"1695427200000\",\"1695600000000\",\"1695945600000\",\"1696291200000\",\"1696550400000\",\"1696896000000\",\"1697241600000\",\"1697500800000\",\"1697846400000\",\"1698105600000\",\"1698451200000\",\"1698883200000\",\"1699401600000\",\"1699920000000\",\"1700265600000\",\"1700611200000\",\"1700956800000\",\"1701388800000\",\"1701820800000\",\"1702166400000\",\"1702598400000\",\"1703030400000\",\"1703548800000\",\"1703894400000\",\"1704153600000\",\"1704585600000\",\"1704931200000\",\"1705276800000\",\"1705536000000\",\"1705708800000\",\"1705795200000\",\"1705968000000\",\"1706054400000\",\"1706227200000\",\"1706313600000\",\"1706400000000\",\"1706572800000\",\"1706745600000\",\"1706832000000\",\"1707004800000\",\"1707091200000\",\"1707177600000\",\"1707523200000\",\"1707696000000\",\"1707782400000\",\"1707868800000\",\"1708041600000\",\"1708128000000\",\"1708300800000\",\"1708473600000\",\"1708646400000\",\"1708819200000\",\"1708905600000\",\"1709337600000\",\"1709510400000\",\"1709596800000\",\"1709769600000\",\"1710720000000\",\"1710979200000\",\"1711065600000\",\"1711152000000\",\"1711238400000\",\"1711324800000\",\"1711411200000\",\"1711497600000\",\"1711584000000\",\"1721952000000\",\"1722038400000\",\"1722124800000\",\"1722211200000\",\"1722297600000\"]}";
		AttendanceInfoResponse message = JsonConvert.DeserializeObject<AttendanceInfoResponse>(value);
		return ResultUtil.CreateResult(message.ToByteArray(), 0);
	}

	[HttpPut]
	public GameActionResult PackEventStoryInfo()
	{
		PackEventStoryInfoResponse message = new PackEventStoryInfoResponse();
		return ResultUtil.CreateResult(message.ToByteArray(), 220);
	}

	[HttpPut]
	public GameActionResult PackEventBattleInfo()
	{
		PackEventBattleInfoResponse message = new PackEventBattleInfoResponse();
		return ResultUtil.CreateResult(message.ToByteArray(), 220);
	}

	[HttpPut]
	public GameActionResult PictorialBookInfo()
	{
		PictorialBookInfoResponse pictorialBookInfoResponse = new PictorialBookInfoResponse();
		pictorialBookInfoResponse.PictorialbookInfo.Add(new PictorialBookDBInfo
		{
			Id = 4,
			GroupId = 2
		});
		pictorialBookInfoResponse.PictorialbookInfo.Add(new PictorialBookDBInfo
		{
			Id = 5,
			GroupId = 1
		});
		return ResultUtil.CreateResult(pictorialBookInfoResponse);
	}

	[HttpPut]
	public GameActionResult AllCharRefresh()
	{
		AllCharRefreshResponse allCharRefreshResponse = new AllCharRefreshResponse();
		allCharRefreshResponse.BuffStatInfo.Add(new PictorialBuffStatDBInfo
		{
			StatType = Define_CharStatOption.HealthPercent,
			StatValue = 0.8
		});
		allCharRefreshResponse.BuffStatInfo.Add(new PictorialBuffStatDBInfo
		{
			StatType = Define_CharStatOption.AttackPercent,
			StatValue = 0.8
		});
		allCharRefreshResponse.BuffStatInfo.Add(new PictorialBuffStatDBInfo
		{
			StatType = Define_CharStatOption.MagicPercent,
			StatValue = 0.8
		});
		return ResultUtil.CreateResult(allCharRefreshResponse.ToByteArray(), 165);
	}

	[HttpPut]
	public GameActionResult SaveTotalBattlePower([FromBody] string data)
	{
		SaveTotalBattlePowerRequest saveTotalBattlePowerRequest = ProtoUtil.Deserialize<SaveTotalBattlePowerRequest>(data);
		SaveTotalBattlePowerResponse data2 = new SaveTotalBattlePowerResponse
		{
			HighestTotalBattlePower = saveTotalBattlePowerRequest.TotalBattlePower
		};
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public GameActionResult MiniGameRunInfo()
	{
		string text = "Co4mCMIDKID1xgUyCemKgOeZveeLkDjTqwFC9CV7IlRpbWVsaW5lRGF0YXMiOlt7IlRpbWUiOjAuMCwiQmVoYXZpb3VyIjoxLCJQb3NpdGlvbiI6eyJ4IjotMTMuOTY5LCJ5IjowLjIzMDAwMDAxOSwieiI6LTE0LjgzOX0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6MC4wfSx7IlRpbWUiOjAuMCwiQmVoYXZpb3VyIjowLCJQb3NpdGlvbiI6eyJ4IjotMTMuOTY5LCJ5IjowLjIzMDAwMDAxOSwieiI6LTE0LjgzOX0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6MS43NX0seyJUaW1lIjowLjEwMTExNzUyOSwiQmVoYXZpb3VyIjowLCJQb3NpdGlvbiI6eyJ4IjotMTMuMzY1MDA2NCwieSI6MC4yMzAwMDAwMTksInoiOi0xNC4yMzUwMDYzfSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjB9LHsiVGltZSI6MC4xMDExMTc1MjksIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6LTEzLjM2NTAwNjQsInkiOjAuMjMwMDAwMDE5LCJ6IjotMTQuMjM1MDA2M30sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjAuODA4Njg0NiwiQmVoYXZpb3VyIjo0LCJQb3NpdGlvbiI6eyJ4IjotOC4zNjM4NSwieSI6MC4yMzAwMDAwNzksInoiOi05LjIzMzg1fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjB9LHsiVGltZSI6MS41NTAwNDMsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6LTMuMTIxNjQ5NzQsInkiOjAuMjMwMDAwMDE5LCJ6IjotMy45OTE2NDg2N30sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjIuMzI1MDYzLCJCZWhhdmlvdXIiOjAsIlBvc2l0aW9uIjp7IngiOjIuNTU5NjMsInkiOjAuMjMwMDAwMDE5LCJ6IjoxLjY4OTYzfSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6Mi4zMjUwNjMsIkJlaGF2aW91ciI6MywiUG9zaXRpb24iOnsieCI6Mi41NTk2MywieSI6MC4yMzAwMDAwMTksInoiOjEuNjg5NjN9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuNX0seyJUaW1lIjoyLjQ1OTg2NTgxLCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjMuNDk5ODA2LCJ5IjowLjIzLCJ6IjoyLjYyOTgwNX0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjMuMzY5Njg5NDYsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6MTEuNTQxNTksInkiOjAuMjMwMDAwMDE5LCJ6IjoxMC42NzE1OTE4fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6My45NzYyNTM1MSwiQmVoYXZpb3VyIjo2LCJQb3NpdGlvbiI6eyJ4IjoxNy4zNjQ2ODUxLCJ5IjowLjIyOTk5OTk1OSwieiI6MTYuNDk0Njl9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuNX0seyJUaW1lIjo0LjMxMzIzMjQyLCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjE5Ljg4MTQwMywieSI6MC4yMjk5OTk5NTksInoiOjE5LjAxMTQwNzl9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuNX0seyJUaW1lIjo1LjA4ODI4MywiQmVoYXZpb3VyIjo0LCJQb3NpdGlvbiI6eyJ4IjoyNi43MzE5MjIxLCJ5IjowLjIzMDAwMDAxOSwieiI6MjUuODYxOTI3fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6NS4zOTE1NTg2NSwiQmVoYXZpb3VyIjowLCJQb3NpdGlvbiI6eyJ4IjoyOS40MTI1MTc1LCJ5IjowLjIzMDAwMDAxOSwieiI6MjguNTQyNTIyNH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjUuODYzMzQsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6MzIuNzY5NjMsInkiOjAuMjMwMDAwMDE5LCJ6IjozMS44OTk2MzUzfSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjB9LHsiVGltZSI6Ny4xMTAxNTEyOSwiQmVoYXZpb3VyIjo0LCJQb3NpdGlvbiI6eyJ4Ijo0MS41ODU5MTQ2LCJ5IjowLjIzMDAwMDA3OSwieiI6NDAuNzE1OTJ9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuMH0seyJUaW1lIjo4LjA4NzQwOSwiQmVoYXZpb3VyIjo0LCJQb3NpdGlvbiI6eyJ4Ijo0OC40OTYxOTI5LCJ5IjowLjIzMDAwMDA3OSwieiI6NDcuNjI2MTk3OH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjguODYyNTIyLCJCZWhhdmlvdXIiOjAsIlBvc2l0aW9uIjp7IngiOjU0LjI2NjgwNzYsInkiOjAuMjMwMDAwMDE5LCJ6Ijo1My4zOTY4MTI0fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6OC44NjI1MjIsIkJlaGF2aW91ciI6MywiUG9zaXRpb24iOnsieCI6NTQuMjY2ODA3NiwieSI6MC4yMzAwMDAwMTksInoiOjUzLjM5NjgxMjR9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuNX0seyJUaW1lIjo4Ljk5NzM0MywiQmVoYXZpb3VyIjo0LCJQb3NpdGlvbiI6eyJ4Ijo1NS4wOTYzMiwieSI6MC4yMzAwMDAwMTksInoiOjU0LjIyNjMyNn0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjkuNzcyNDIsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6NjEuOTQ3MDcsInkiOjAuMjMwMDAwMDE5LCJ6Ijo2MS4wNzcwOH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjEwLjYxNDkwNDQsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6NjkuMzkzNjIsInkiOjAuMjMwMDAwMDc5LCJ6Ijo2OC41MjM2M30sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjExLjQyMzY5MTcsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6NzYuNTQyMzQsInkiOjAuMjMwMDAwMDc5LCJ6Ijo3NS42NzIzNX0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjExLjU5MjE2ODgsIkJlaGF2aW91ciI6NiwiUG9zaXRpb24iOnsieCI6NzguMzUyNTU0MywieSI6MC4yMzAwMDAwMTksInoiOjc3LjQ4MjU2fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6MTEuOTI5MTQxLCJCZWhhdmlvdXIiOjAsIlBvc2l0aW9uIjp7IngiOjgxLjAwOTkyLCJ5IjowLjIzMDAwMDAxOSwieiI6ODAuMTM5OTJ9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuMH0seyJUaW1lIjoxMi40NjgzNTUyLCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjg0Ljg2NTcxLCJ5IjowLjIzMDAwMDAxOSwieiI6ODMuOTk1NzJ9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuMH0seyJUaW1lIjoxMy41MTMwMzQ4LCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjkyLjI1MjU1NTgsInkiOjAuMjMwMDAwMDE5LCJ6Ijo5MS4zODI1N30sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjE0LjUyNDAxNTQsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6OTkuNDAxMTUsInkiOjAuMjMwMDAwMDE5LCJ6Ijo5OC41MzExNn0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjE1LjQ2NzY0MzcsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6MTA2LjA3MzQ4NiwieSI6MC4yMjk5OTk5NTksInoiOjEwNS4yMDM0OTF9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuMH0seyJUaW1lIjoxNS43MDM1MzQxLCJCZWhhdmlvdXIiOjYsIlBvc2l0aW9uIjp7IngiOjEwOC4wOTQ2NzMsInkiOjAuMjMwMDAwMDE5LCJ6IjoxMDcuMjI0Njc4fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjB9LHsiVGltZSI6MTYuMzQzODIyNSwiQmVoYXZpb3VyIjo0LCJQb3NpdGlvbiI6eyJ4IjoxMTIuMjY4ODc1LCJ5IjowLjIzMDAwMDAxOSwieiI6MTExLjM5ODg4fSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjB9LHsiVGltZSI6MTcuNzkyODgsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6MTIyLjUxNTA2LCJ5IjowLjIzMDAwMDA3OSwieiI6MTIxLjY0NTA1OH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjE3Ljk5NTA3OSwiQmVoYXZpb3VyIjowLCJQb3NpdGlvbiI6eyJ4IjoxMjQuMTgxMDIzLCJ5IjowLjIzMDAwMDAxOSwieiI6MTIzLjMxMTAyOH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjE3Ljk5NTA3OSwiQmVoYXZpb3VyIjozLCJQb3NpdGlvbiI6eyJ4IjoxMjQuMTgxMDIzLCJ5IjowLjIzMDAwMDAxOSwieiI6MTIzLjMxMTAyOH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi41fSx7IlRpbWUiOjE4LjU2NzkzMjEsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6MTI4Ljk0OTExMiwieSI6MC4yMzAwMDAwNzksInoiOjEyOC4wNzkwODZ9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuNX0seyJUaW1lIjoxOS4zMDkzMzU3LCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjEzNS41MDI0NTcsInkiOjAuMjMwMDAwMDE5LCJ6IjoxMzQuNjMyNDMxfSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6MjAuMDUwNzYsIkJlaGF2aW91ciI6NCwiUG9zaXRpb24iOnsieCI6MTQyLjA1NTk2OSwieSI6MC4yMzAwMDAwMTksInoiOjE0MS4xODU5NDR9LCJTcGVlZE1hZ25pZmljYXRpb24iOjIuNX0seyJUaW1lIjoyMC44OTMxOTYxLCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjE0OS41MDIyODksInkiOjAuMjMwMDAwMDc5LCJ6IjoxNDguNjMyMjYzfSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjV9LHsiVGltZSI6MjEuMDYxNjk1MSwiQmVoYXZpb3VyIjowLCJQb3NpdGlvbiI6eyJ4IjoxNTAuOTkxNjY5LCJ5IjowLjIzMDAwMDAxOSwieiI6MTUwLjEyMTY0M30sIlNwZWVkTWFnbmlmaWNhdGlvbiI6Mi4wfSx7IlRpbWUiOjIxLjYzNDU2LCJCZWhhdmlvdXIiOjQsIlBvc2l0aW9uIjp7IngiOjE1NS4wNzIwNjcsInkiOjAuMjMwMDAwMDE5LCJ6IjoxNTQuMjAyMDQyfSwiU3BlZWRNYWduaWZpY2F0aW9uIjoyLjB9LHsiVGltZSI6MjEuOTcxNTYsIkJlaGF2aW91ciI6MCwiUG9zaXRpb24iOnsieCI6MTU3Ljg1NzU1OSwieSI6MC4yMzAwMDAwMTksInoiOjE1Ni45ODc1MzR9LCJTcGVlZE1hZ25pZmljYXRpb24iOjAuMH0seyJUaW1lIjoyMS45NzE1NiwiQmVoYXZpb3VyIjo3LCJQb3NpdGlvbiI6eyJ4IjoxNTcuODU3NTU5LCJ5IjowLjIzMDAwMDAxOSwieiI6MTU2Ljk4NzUzNH0sIlNwZWVkTWFnbmlmaWNhdGlvbiI6MC4wfV19";
		return new GameActionResult
		{
			packetCode = 0,
			length = text.Length,
			data = AesUtils.AesEncrypt(text)
		};
	}

	[HttpPut]
	public async Task<GameActionResult> PackPreviewInfo([FromBody] string data)
	{
		PackPreviewInfoRequest req = ProtoUtil.Deserialize<PackPreviewInfoRequest>(data);
		return await _gameUserService.PackPreviewInfo(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult TodayQuestInfo()
	{
		string text = "GNvJASK0AbWNBr+NBsmNBo+OBpmOBqOOBv2OBoePBpGPBs2PBtePBuGPBsWQBs+QBtmQBpWRBrORBr2RBo2SBpeSBqGSBvGSBvuSBoWTBsuTBt+TBumTBp2VBqeVBrGVBveVBoGWBouWBtuWBuWWBu+WBr+XBsmXBtOXBq2YBreYBsGYBqmMPbOMPb2MPZGUPZuUPaWUPfmbPYOcPY2cPeGjPeujPfWjPcmrPdOrPd2rPc+zPdmzPeOzPQ==";
		return ResultUtil.CreateResult(new TodayQuestInfoResponse());
	}

	[HttpPut]
	public GameActionResult FieldDeckInfo()
	{
		return ResultUtil.CreateResult(new FieldDeckInfoResponse());
	}

	[HttpPut]
	public GameActionResult HuntingGroundInfo([FromBody] string data)
	{
		int[] array = new int[5] { 201, 202, 203, 204, 205 };
		HuntingGroundDBInfo huntingGroundInfo = new HuntingGroundDBInfo
		{
			CurrentId = 1,
			HighestId = 3
		};
		HuntingGroundInfoResponse data2 = new HuntingGroundInfoResponse
		{
			HuntingGroundInfo = huntingGroundInfo
		};
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public async Task<GameActionResult> SaveUserPosition([FromBody] string data)
	{
		SaveUserPositionRequest position = ProtoUtil.Deserialize<SaveUserPositionRequest>(data);
		return await _gameUserService.SaveUserPosition(position, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> ActiveMap([FromBody] string data)
	{
		ActiveMapRequest map = ProtoUtil.Deserialize<ActiveMapRequest>(data);
		return await _gameUserService.ActiveMap(map, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> DeckSave([FromBody] string data)
	{
		DeckSaveRequest deck = ProtoUtil.Deserialize<DeckSaveRequest>(data);
		return await _gameUserService.DeckSave(deck, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult WaypointUse([FromBody] string data)
	{
		WaypointUseRequest waypointUseRequest = ProtoUtil.Deserialize<WaypointUseRequest>(data);
		_logger.LogInformation("WaypointUse: {}", waypointUseRequest);
		return ResultUtil.CreateResult(new WaypointUseResponse().ToByteArray(), 10);
	}

	[HttpPut]
	public GameActionResult AchievementUpdate([FromBody] string data)
	{
		AchievementUpdateRequest achievementUpdateRequest = ProtoUtil.Deserialize<AchievementUpdateRequest>(data);
		_logger.LogInformation("更新成就: {}", achievementUpdateRequest);
		return ResultUtil.CreateResult(new AchievementUpdateResponse().ToByteArray(), 11);
	}

	[HttpPut]
	public GameActionResult MailHistoryInfo()
	{
		return _gameUserService.MailHistoryInfo();
	}

	[HttpPut]
	public GameActionResult PackBuy([FromBody] string data)
	{
		PackBuyRequest packBuyRequest = ProtoUtil.Deserialize<PackBuyRequest>(data);
		PackBuyResponse data2 = new PackBuyResponse
		{
			PackInfo = new PackDBInfo
			{
				Id = packBuyRequest.PackId
			}
		};
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public GameActionResult UserPortraitChange()
	{
		return ResultUtil.CreateResult(new UserPortraitChangeResponse());
	}

	[HttpPut]
	public GameActionResult FieldDeckSave()
	{
		return ResultUtil.CreateResult(new FieldDeckSaveResponse());
	}

	[HttpPut]
	public async Task<GameActionResult> TutorialClear([FromBody] string data)
	{
		TutorialClearRequest tutorial = ProtoUtil.Deserialize<TutorialClearRequest>(data);
		return await _gameUserService.TutorialClear(tutorial, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> PresetInfoChange([FromBody] string data)
	{
		PresetInfoChangeRequest req = ProtoUtil.Deserialize<PresetInfoChangeRequest>(data);
		return await _gameUserService.PresetInfoChange(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> PresetSave([FromBody] string data)
	{
		PresetSaveRequest req = ProtoUtil.Deserialize<PresetSaveRequest>(data);
		return await _gameUserService.PresetSave(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> PresetDelete([FromBody] string data)
	{
		PresetDeleteRequest req = ProtoUtil.Deserialize<PresetDeleteRequest>(data);
		return await _gameUserService.PresetDelete(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult Cooking()
	{
		CookingResponse cookingResponse = new CookingResponse();
		cookingResponse.AddTalentExp = 1;
		cookingResponse.ItemInfo.Add(new ItemDBInfo
		{
			Count = 1,
			Id = 2002,
			InvenIndex = 38617900L,
			Type = 5,
			TimeValue = 1722972375000L
		});
		return ResultUtil.CreateResult(cookingResponse);
	}

	[HttpPut]
	public GameActionResult TrapDamage()
	{
		return ResultUtil.CreateResult(new TrapDamageResponse());
	}

	[HttpPut]
	public GameActionResult CharImmortal([FromBody] string data)
	{
		CharImmortalRequest charImmortalRequest = ProtoUtil.Deserialize<CharImmortalRequest>(data);
		List<CharInfoDao> source = _db.Queryable<CharInfoDao>().In<RepeatedField<long>>(charImmortalRequest.InvenIndex).ToList();
		CharImmortalResponse charImmortalResponse = new CharImmortalResponse();
		charImmortalResponse.CharInfo.AddRange(_mapper.Map<List<CharDBInfo>>(source));
		return ResultUtil.CreateResult(charImmortalResponse);
	}

	[HttpPut]
	public async Task<GameActionResult> WaypointSave([FromBody] string data)
	{
		WaypointSaveRequest req = ProtoUtil.Deserialize<WaypointSaveRequest>(data);
		return await _gameUserService.WaypointSave(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public GameActionResult EatFood([FromBody] string data)
	{
		EatFoodRequest eatFoodRequest = ProtoUtil.Deserialize<EatFoodRequest>(data);
		CharInfoDao source = _db.Queryable<CharInfoDao>().Single((CharInfoDao x) => x.InvenIndex == eatFoodRequest.InvenIndex);
		EatFoodResponse eatFoodResponse = new EatFoodResponse();
		eatFoodResponse.CharInfo.Add(_mapper.Map<CharDBInfo>(source));
		return ResultUtil.CreateResult(eatFoodResponse);
	}

	[HttpPut]
	public GameActionResult EatFoodAuto([FromBody] string data)
	{
		EatFoodAutoRequest eatFoodAutoRequest = ProtoUtil.Deserialize<EatFoodAutoRequest>(data);
		List<EatFoodAutoResponse.Types.RecoveryCharInfo> list = new List<EatFoodAutoResponse.Types.RecoveryCharInfo>();
		foreach (EatFoodAutoRequest.Types.FoodUseInfo foodUseInfo in eatFoodAutoRequest.EatFoodInfo)
		{
			CharInfoDao charInfoDao = _db.Queryable<CharInfoDao>().Single((CharInfoDao x) => x.InvenIndex == foodUseInfo.InvenIndex);
			list.Add(new EatFoodAutoResponse.Types.RecoveryCharInfo
			{
				CharInvenIndex = charInfoDao.InvenIndex,
				Hp = charInfoDao.Hp
			});
		}
		EatFoodAutoResponse eatFoodAutoResponse = new EatFoodAutoResponse();
		eatFoodAutoResponse.RecoveryCharInfo.AddRange(list);
		return ResultUtil.CreateResult(eatFoodAutoResponse);
	}

	[HttpPut]
	public GameActionResult InnOpen()
	{
		InnOpenResponse data = new InnOpenResponse
		{
			ReputationState = 1
		};
		return ResultUtil.CreateResult(data);
	}

	[HttpPut]
	public GameActionResult CharAllRevival([FromBody] string data)
	{
		CharAllRevivalRequest charAllRevivalRequest = ProtoUtil.Deserialize<CharAllRevivalRequest>(data);
		List<CharInfoDao> source = _db.Queryable<CharInfoDao>().In<RepeatedField<long>>(charAllRevivalRequest.CharIndex).ToList();
		CharAllRevivalResponse charAllRevivalResponse = new CharAllRevivalResponse();
		charAllRevivalResponse.CharInfo.AddRange(_mapper.Map<List<CharDBInfo>>(source));
		return ResultUtil.CreateResult(charAllRevivalResponse);
	}

	[HttpPut]
	public GameActionResult FieldObjectPositionUpdate([FromBody] string data)
	{
		FieldObjectPositionUpdateRequest fieldObjectPositionUpdateRequest = ProtoUtil.Deserialize<FieldObjectPositionUpdateRequest>(data);
		_logger.LogInformation("场景道具:{}", fieldObjectPositionUpdateRequest);
		return ResultUtil.CreateResult(new FieldObjectPositionUpdateResponse());
	}

	[HttpPut]
	public GameActionResult PopularCostumeInfo([FromBody] string data)
	{
		PopularCostumeInfoRequest popularCostumeInfoRequest = ProtoUtil.Deserialize<PopularCostumeInfoRequest>(data);
		PopularCostumeInfoResponse data2 = new PopularCostumeInfoResponse();
		if (popularCostumeInfoRequest.ContentType == 3)
		{
			data2 = PopularCostumeInfoResponse.Parser.ParseFrom(Convert.FromBase64String("Cg0IytwDEPtSGKRFIMBOCg0IiekDEPRRGOBEIIJPCg0I8okEEO1PGPZCIO1MCgsIZxDZThjaQSCmSwoNCPHXAxDuTBj/PiC7SAoNCNrfAxCnSBiUOiCOQgoMCNIIEORHGLI5ILVCCgwIsQkQpkMYmzkgnkMKCwhpENU9GOUxILQ5CgwInRgQozkY0DEg4DkKDQjO4wMQ9DMYsiwgyTMKDQiinAEQtzAY6ygg6S4KDAj4AxDNLxjNKSCOMAoMCKQGEMQsGPYkIOkoCgwIsgIQpiwYySYgxisKDQjP/AMQ+CsYoCIgpygKDQiBjQQQhSoY3CIg0ScKDAi2CRCwKRjbIiD6JgoMCLcJENUiGOQdILMlCg0I4oYEELMiGJYgIPskCg0I+d4DEJQgGJwcILYfCgwIrRsQ0B4Y2BsgrR8KDQihnAEQvRwY5hYg9RsKDQiFnQEQqBsYlRcg4BsKDAj2AxCFGxiyGCCeHAoNCPLXAxDdGRjaFCDxFwoNCOWNBBDRGRjXEyDQFAoNCPPXAxDmFxj8FCDBGAoMCK8CENUXGLkUIJAXCgwI7AcQ1hYY7hIggRQKDQjxiQQQ+xQY2BIglBYKDAiBGRCDFBjDECDzFAoMCK0CENATGIESIPYUCg0IotoDEJoTGMUOIL8QCg0IzvwDEJYSGL8NIIkRCg0I9t4DEMkQGLgOIIwRCgsIZRCODxjPCyCCDgoMCJIDEPYNGMMKIK8LCgwIkwMQ1A0YtAsgwwwKDQjW2AMQ7wwY8gkgiAwKDQjhhgQQ0wwYgQ0g1Q4KDAjCBRCwDBjqCiCWDAoMCOESEPQLGOcLIMAOCg0IntoDENMLGKQJINYKCgwIsAIQnQsYsQog7AsKDQi7iwQQrQoYvwcgkwgKDAj6AxDYCRjGCCDxCQoMCL0FEN4GGJQGIK4GCgwI6gcQ0gYY8AYg4wYKDQiKggQQzgYYlQUgswUKDQiu3QMQiAYY0wQgzAQKDQit3QMQ9gUYkAQgyAQKDQiGnQEQ7gUYzgUgzgYKDAiCGRDSBRjdBCC7BgoLCGoQlQUYqQQgxgUKDQjF1QMQ4gQYwwQgqwUKDQjV2AMQvgQY0AMgxQQKDAimBhCsBBi4BCCQBQoNCLqLBBC8AxiKAyD4AwoLCGYQrAMYqwIgkQMKDQid2gMQnAMY0wIgrAMKDAihBhCFAxj5AiC/AwoNCJXlAxCFAxi6AyDdAwoMCOYZEPgCGN0CIJUDCg0IgdsDEKUCGKwCIP0CCgwIzAEQkQIYjwIgwgIKDQjN4wMQ/AEY8wEgyAIKDAjJARDkARjcASC4AgoMCMoBENEBGN0BIPEBCgwIkQMQtAEY0QEg3AEKDAiWAxCBARiGASCUAQoFCNMIEHkKCQjLARBYGGIgdAoKCI3XAxBMGEAgYwoKCILbAxBLGEIgSQoJCLkXEEQYWSBxCgoI44YEEDYYLCBVCgkIzgEQHBgbIBsKCgjG1QMQHBgbICYKCgjJ3AMQGxgWICEKCgiO1wMQGhgdICoKCQj1AxAWGAwgDgoKCJnsAxAVGBkgHwoKCKXoAxAUGAogDQoJCOYSEBEYCSAPCgkIlQoQEBgSIBAKCgju6QMQChgKIAYKCgjaigQQChgLIAYKCQjpBxAGGAsgBwoJCM0IEAYYCiAJCgoIluUDEAUYBSALCgkI3QsQBBgDIAQKCgjR6gMQBBgBIAEKCgip7wMQBBgBIAEKCQiFBxADGAkgCQoKCKnWAxADGAMgDQoICLHkAxADIAMKCAhoEAIYAiADCgkI2QQQAhgBIAcKCgiF+wMQAhgFIAQKCgjp+wMQAhgBIAMKBgidjAQQAgoJCMEMEAEYAiACCggIutkDEAEYAgoKCMXuAxABGAQgBQoKCNaKBBABGAIgAwoKCJ6MBBABGAEgAgoHCNwEGAEgAQoHCPkKGAIgAQoICObbAxgBIAEKBgiy5AMgAQoICO3pAxgBIAEKBgjS6gMgBAoICKH6AxgCIAY="));
		}
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public GameActionResult PackJamEvent([FromBody] string data)
	{
		return ResultUtil.CreateResult(new PackJamEventResponse());
	}

	[HttpPut]
	public async Task<GameActionResult> MonsterInfo([FromBody] string data)
	{
		MonsterInfoRequest req = ProtoUtil.Deserialize<MonsterInfoRequest>(data);
		return await _gameUserService.MonsterInfo(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> FieldMonsterRegen([FromBody] string data)
	{
		await Task.CompletedTask;
		return ResultUtil.CreateResult(new FieldMonsterRegenResponse());
	}

	[HttpPut]
	public async Task<GameActionResult> TimePause([FromBody] string data)
	{
		await Task.CompletedTask;
		TimePauseRequest timePauseRequest = ProtoUtil.Deserialize<TimePauseRequest>(data);
		_logger.LogInformation("暂停时间:{}", timePauseRequest);
		return ResultUtil.CreateResult(new TimePauseResponse());
	}

	[HttpPut]
	public async Task<GameActionResult> UseRandomBox([FromBody] string data)
	{
		UseRandomBoxRequest req = ProtoUtil.Deserialize<UseRandomBoxRequest>(data);
		return await _gameUserService.UseRandomBox(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> HuntDispatch([FromBody] string data)
	{
		HuntDispatchRequest req = ProtoUtil.Deserialize<HuntDispatchRequest>(data);
		return await _gameUserService.HuntDispatch(req, base.User.FindFirst("user").Value);
	}

	[HttpPut]
	public async Task<GameActionResult> MiniGameFieldInfo([FromBody] string data)
	{
		await Task.CompletedTask;
		MiniGameBingoInfoResponse data2 = MiniGameBingoInfoResponse.Parser.ParseFrom(Convert.FromBase64String("ChwI7QMoqLDdBTIP6LKe55S344GV44Gg44GKOJBO"));
		return ResultUtil.CreateResult(data2);
	}

	[HttpPut]
	public async Task<GameActionResult> MailOpen([FromBody] string data)
	{
		MailOpenRequest req = ProtoUtil.Deserialize<MailOpenRequest>(data);
		return await _gameUserService.MailOpen(req);
	}
}
