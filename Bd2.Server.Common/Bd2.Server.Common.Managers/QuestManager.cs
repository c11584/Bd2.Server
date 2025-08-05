using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bd2.Server.Common.Proto.Design.common;
using Newtonsoft.Json;

namespace Bd2.Server.Common.Managers;

public class QuestManager
{
	private static Dictionary<int, List<QuestTable1>> _dictionary;

	private static readonly int[] PackIds;

	static QuestManager()
	{
		_dictionary = new Dictionary<int, List<QuestTable1>>();
		PackIds = new int[27]
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 1001, 1002, 1003, 1004, 1005, 1006,
			2001, 2002, 3001, 3002, 3003, 3004, 3005
		};
		try
		{
			int[] packIds = PackIds;
			for (int i = 0; i < packIds.Length; i++)
			{
				int key = packIds[i];
				List<QuestTable1> value = JsonConvert.DeserializeObject<List<QuestTable1>>(File.ReadAllText("data/Quest/QuestTable" + key + ".txt"));
				_dictionary.Add(key, value);
			}
		}
		catch (Exception value2)
		{
			Console.WriteLine(value2);
			throw;
		}
	}

	public static QuestTable1 GetQuestTable(int packId, int questId)
	{
		return _dictionary[packId].FirstOrDefault((QuestTable1 x) => x.Id == questId);
	}
}
