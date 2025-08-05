using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bd2.Server.Common.Proto.Design.common;
using Newtonsoft.Json;

namespace Bd2.Server.Common.Managers;

public class RewardManager
{
	private static List<RewardGroupTable> _rewardGroupTables;

	private static List<RandomBoxTable> _randomBoxTables;

	static RewardManager()
	{
		try
		{
			_rewardGroupTables = JsonConvert.DeserializeObject<List<RewardGroupTable>>(File.ReadAllText("data/RewardGroupTable.txt"));
			_randomBoxTables = JsonConvert.DeserializeObject<List<RandomBoxTable>>(File.ReadAllText("data/RandomBoxTable.txt"));
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}

	public static RandomBoxTable GetRandomBoxTableByRandom()
	{
		int[] ids = new int[10] { 504145, 504143, 504144, 504146, 504148, 504153, 504155, 507001, 508001, 504149 };
		int index = new Random().Next(ids.Length);
		return _randomBoxTables.Single((RandomBoxTable x) => x.Id == ids[index]);
	}

	public static RandomBoxTable GetRandomBoxTableById(int id)
	{
		return _randomBoxTables.Single((RandomBoxTable x) => x.Id == id);
	}

	public static RewardGroupTable GetRewardGroupTableById(int id)
	{
		return _rewardGroupTables.Single((RewardGroupTable x) => x.Id == id);
	}

	public static RewardGroupTable GetRewardGroupTableByRandomBoxId(int id)
	{
		return GetRewardGroupTableById(GetRandomBoxTableById(id).RewardGroupId);
	}
}
