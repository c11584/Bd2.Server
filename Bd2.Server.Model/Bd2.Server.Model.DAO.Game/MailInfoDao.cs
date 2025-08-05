using SqlSugar;

namespace Bd2.Server.Model.DAO.Game;

[SugarTable("MailInfo")]
public class MailInfoDao
{
	[SugarColumn(ColumnName = "InvenIndex", IsPrimaryKey = true, IsIdentity = true)]
	public long InvenIndex { get; set; }

	[SugarColumn(ColumnName = "Uid")]
	public long Uid { get; set; }

	[SugarColumn(ColumnName = "Type")]
	public int Type { get; set; }

	[SugarColumn(ColumnName = "MailId")]
	public int MailId { get; set; }

	[SugarColumn(ColumnName = "SenderText")]
	public string? SenderText { get; set; }

	[SugarColumn(ColumnName = "TitleText")]
	public string? TitleText { get; set; }

	[SugarColumn(ColumnName = "MessageText")]
	public string? MessageText { get; set; }

	[SugarColumn(ColumnName = "RewardExpireTime")]
	public long RewardExpireTime { get; set; }

	[SugarColumn(ColumnName = "ItemType")]
	public int ItemType { get; set; }

	[SugarColumn(ColumnName = "ItemId")]
	public int ItemId { get; set; }

	[SugarColumn(ColumnName = "ItemCount")]
	public int ItemCount { get; set; }

	[SugarColumn(ColumnName = "IsOpen")]
	public int IsOpen { get; set; }

	[SugarColumn(ColumnName = "OpenTime")]
	public long OpenTime { get; set; }

	[SugarColumn(ColumnName = "CreateTime")]
	public long CreateTime { get; set; }

	[SugarColumn(ColumnName = "HistoryDeleteTime")]
	public long HistoryDeleteTime { get; set; }

	[SugarColumn(ColumnName = "IsCash")]
	public int IsCash { get; set; }
}
