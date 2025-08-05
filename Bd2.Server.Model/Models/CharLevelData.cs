using SqlSugar;

namespace Models;

[SugarTable("CharLevelData")]
public class CharLevelData
{
	[SugarColumn(ColumnName = "GroupId")]
	public int? GroupId { get; set; }

	[SugarColumn(ColumnName = "Level")]
	public int? Level { get; set; }

	[SugarColumn(ColumnName = "LevelupExp")]
	public int? LevelupExp { get; set; }

	[SugarColumn(ColumnName = "Health")]
	public float? Health { get; set; }
}
