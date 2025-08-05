using Bd2.Server.Model.Config;
using Newtonsoft.Json;
using SqlSugar;

namespace Bd2.Server.Model.DAO.Game;

[SugarTable("CostumeInfo")]
public class CostumeInfoDao
{
	[SugarColumn(ColumnName = "InvenIndex", IsPrimaryKey = true, IsIdentity = true)]
	public long InvenIndex { get; set; }

	[SugarColumn(ColumnName = "Uid")]
	public long Uid { get; set; }

	[SugarColumn(ColumnName = "Id")]
	public int Id { get; set; }

	[SugarColumn(ColumnName = "Level")]
	public int Level { get; set; }

	[SugarColumn(ColumnName = "UseChar")]
	public long? UseChar { get; set; }

	[SugarColumn(ColumnName = "SortId")]
	public int? SortId { get; set; }

	[SugarColumn(ColumnName = "UseMyRoomCount")]
	public int? UseMyRoomCount { get; set; }

	[SugarColumn(ColumnName = "PotentialId")]
	[JsonConverter(typeof(StringArrayConverter))]
	public string? PotentialId { get; set; }
}
