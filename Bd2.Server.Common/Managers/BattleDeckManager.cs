using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bd2.Server.Common.Proto.Design.common;
using Bd2.Server.Common.Proto.Design.pack;
using Bd2.Server.Common.Proto.Net;
using Google.Protobuf.Collections;
using Newtonsoft.Json;

namespace Bd2.Server.Common.Managers;

public class BattleDeckManager
{
	private static Dictionary<int, List<BattleDeckTable>> _dictionary;

	private static Dictionary<int, List<FieldMonsterTable>> _monsterDictionary;

	private static readonly int[] PackIds;

	static BattleDeckManager()
	{
		_dictionary = new Dictionary<int, List<BattleDeckTable>>();
		_monsterDictionary = new Dictionary<int, List<FieldMonsterTable>>();
		PackIds = new int[20]
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 1001, 1002, 1003, 1004, 1005, 1006
		};
		try
		{
			int[] packIds = PackIds;
			for (int i = 0; i < packIds.Length; i++)
			{
				int key = packIds[i];
				List<BattleDeckTable> value = JsonConvert.DeserializeObject<List<BattleDeckTable>>(File.ReadAllText("data/PACK/" + key + "/BattleDeckTable.txt"));
				_dictionary.Add(key, value);
				List<FieldMonsterTable> value2 = JsonConvert.DeserializeObject<List<FieldMonsterTable>>(File.ReadAllText("data/PACK/" + key + "/FieldMonsterTable.txt"));
				_monsterDictionary.Add(key, value2);
			}
		}
		catch (Exception value3)
		{
			Console.WriteLine(value3);
			throw;
		}
	}

	public static BattleDeckTable GetBattleDeckTable(int packId, int id)
	{
		return _dictionary[packId].FirstOrDefault((BattleDeckTable x) => x.Id == id);
	}

	public static List<MonsterDBInfo> GetMonsterInfoListByGroupIds(int packId, int[] groupId)
	{
		List<MonsterDBInfo> list = new List<MonsterDBInfo>();
		List<FieldMonsterTable> source = _monsterDictionary[packId];
		foreach (int id in groupId)
		{
			foreach (FieldMonsterTable item in source.Where((FieldMonsterTable x) => x.RegenId == id).ToList())
			{
				if (item.BattleDeckId.Count == 0 && item.QuestRange.Count == 0)
				{
					list.Add(new MonsterDBInfo
					{
						ActiveFlag = true,
						GroupId = id,
						MonsterId = item.Id
					});
				}
			}
		}
		return list;
	}

	public static List<MonsterDBInfo> GetMonsterInfoListByPackId(int packId)
	{
		List<MonsterDBInfo> list = new List<MonsterDBInfo>();
		foreach (FieldMonsterTable item in _monsterDictionary[packId])
		{
			if (item.BattleDeckId.Count > 0)
			{
				RepeatedField<int> questRange = item.QuestRange;
				if (questRange != null && questRange.Count == 1 && questRange[0] == 0)
				{
					list.Add(new MonsterDBInfo
					{
						ActiveFlag = true,
						BattleDeck = item.BattleDeckId[0],
						GroupId = item.RegenId,
						MonsterId = item.Id
					});
				}
			}
		}
		return list;
	}
}
