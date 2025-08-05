using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bd2.Server.Common.Proto.Design.common;
using Newtonsoft.Json;

namespace Bd2.Server.Common.Managers;

public class TalentManager
{
	private static List<TalentTable> _talentList;

	private static List<TalentSkillTable> _talentSkillList;

	private static List<TalentGrowthTable> _talentGrowthList;

	private static List<AlchemyTable> _alchemyTables;

	static TalentManager()
	{
		try
		{
			_talentList = JsonConvert.DeserializeObject<List<TalentTable>>(File.ReadAllText("data/TalentTable.txt"));
			_talentSkillList = JsonConvert.DeserializeObject<List<TalentSkillTable>>(File.ReadAllText("data/TalentSkillTable.txt"));
			_talentGrowthList = JsonConvert.DeserializeObject<List<TalentGrowthTable>>(File.ReadAllText("data/TalentGrowthTable.txt"));
			_alchemyTables = JsonConvert.DeserializeObject<List<AlchemyTable>>(File.ReadAllText("data/AlchemyTable.txt"));
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}

	public static TalentTable GetTalent(int id)
	{
		return _talentList.Single((TalentTable x) => x.Id == id);
	}

	public static TalentSkillTable GetTalentSkill(int groupId, int level)
	{
		return _talentSkillList.Single((TalentSkillTable x) => x.Id == level && x.GroupId == groupId);
	}

	public static TalentGrowthTable GetTalentGrowth(int groupId, int level)
	{
		return _talentGrowthList.Single((TalentGrowthTable x) => x.Id == level && x.GroupId == groupId);
	}

	public static AlchemyTable GetAlchemyTableById(int id)
	{
		return _alchemyTables.Single((AlchemyTable x) => x.Id == id);
	}
}
