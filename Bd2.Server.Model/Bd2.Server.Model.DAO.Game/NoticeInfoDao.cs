using SqlSugar;

namespace Bd2.Server.Model.DAO.Game;

[SugarTable("NoticeInfo")]
public class NoticeInfoDao
{
	[SugarColumn(ColumnName = "Id")]
	public int Id { get; set; }

	[SugarColumn(ColumnName = "NoticeType")]
	public int? NoticeType { get; set; }

	[SugarColumn(ColumnName = "Title")]
	public string? Title { get; set; }

	[SugarColumn(ColumnName = "Thumbnail")]
	public string? Thumbnail { get; set; }

	[SugarColumn(ColumnName = "StartTime")]
	public long? StartTime { get; set; }

	[SugarColumn(ColumnName = "EndTime")]
	public long? EndTime { get; set; }

	[SugarColumn(ColumnName = "NoticeContent")]
	public string? NoticeContent { get; set; }

	[SugarColumn(ColumnName = "PromotionBannerId")]
	public int? PromotionBannerId { get; set; }

	[SugarColumn(ColumnName = "IsPin")]
	public bool? IsPin { get; set; }

	[SugarColumn(ColumnName = "Sort")]
	public int? Sort { get; set; }

	[SugarColumn(ColumnName = "SubType")]
	public int? SubType { get; set; }
}
