using SqlSugar;

namespace Bd2.Server.Model.DAO.Game;

[SugarTable("PositionInfo")]
public class PositionInfoDao
{
	[SugarColumn(ColumnName = "Index", IsPrimaryKey = true, IsIdentity = true, ColumnDataType = "INTEGER")]
	public long Index { get; set; }

	[SugarColumn(ColumnName = "Uid")]
	public long Uid { get; set; }

	[SugarColumn(ColumnName = "PackId")]
	public int PackId { get; set; }

	[SugarColumn(ColumnName = "Position")]
	public string Position { get; set; }
}
