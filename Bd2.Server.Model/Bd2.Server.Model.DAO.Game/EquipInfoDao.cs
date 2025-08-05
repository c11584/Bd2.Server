using SqlSugar;

namespace Bd2.Server.Model.DAO.Game;

[SugarTable("EquipInfo")]
public class EquipInfoDao
{
	[SugarColumn(ColumnName = "InvenIndex", IsPrimaryKey = true, IsIdentity = true)]
	public long InvenIndex { get; set; }

	[SugarColumn(ColumnName = "Uid")]
	public long Uid { get; set; }

	[SugarColumn(ColumnName = "UseChar")]
	public long UseChar { get; set; }

	[SugarColumn(ColumnName = "KeepFlag")]
	public int KeepFlag { get; set; }

	[SugarColumn(ColumnName = "LockFlag")]
	public int LockFlag { get; set; }

	[SugarColumn(ColumnName = "SortId")]
	public int SortId { get; set; }

	[SugarColumn(ColumnName = "SlotType")]
	public int SlotType { get; set; }

	[SugarColumn(ColumnName = "ID")]
	public int Id { get; set; }

	[SugarColumn(ColumnName = "Level")]
	public int Level { get; set; }

	[SugarColumn(ColumnName = "MainOption")]
	public string? MainOption { get; set; }

	[SugarColumn(ColumnName = "SubOption")]
	public string? SubOption { get; set; }

	[SugarColumn(ColumnName = "PrivateOption")]
	public string? PrivateOption { get; set; }

	[SugarColumn(ColumnName = "Rank")]
	public string Rank { get; set; }
}
