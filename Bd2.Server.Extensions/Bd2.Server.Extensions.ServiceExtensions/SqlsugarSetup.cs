using System;
using System.Text.RegularExpressions;
using Bd2.Server.Common;
using Bd2.Server.Common.DB;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.Extensions;

namespace Bd2.Server.Extensions.ServiceExtensions;

public static class SqlsugarSetup
{
	public static void AddSqlsugarSetup(this IServiceCollection services)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (!string.IsNullOrEmpty(AppSettings.app("MainDB")))
		{
			MainDb.CurrentDbConnId = AppSettings.app("MainDB");
		}
		BaseDBConfig.MutiConnectionString.allDbs.ForEach(delegate(MutiDBOperate m)
		{
			ConnectionConfig connectionConfig = new ConnectionConfig
			{
				ConfigId = m.ConnId.ObjToString().ToLower(),
				ConnectionString = m.Connection,
				DbType = (DbType)m.DbType,
				IsAutoCloseConnection = true,
				MoreSettings = new ConnMoreSettings
				{
					IsAutoRemoveDataCache = true,
					SqlServerCodeFirstNvarchar = true
				},
				InitKeyType = InitKeyType.Attribute
			};
			if ("Log".ToLower().Equals(m.ConnId.ToLower()))
			{
				BaseDBConfig.LogConfig = connectionConfig;
			}
			else
			{
				BaseDBConfig.ValidConfig.Add(connectionConfig);
			}
			BaseDBConfig.AllConfigs.Add(connectionConfig);
		});
		if (BaseDBConfig.LogConfig == null)
		{
			throw new ApplicationException("未配置Log库连接");
		}
		services.AddSingleton((Func<IServiceProvider, ISqlSugarClient>)((IServiceProvider o) => new SqlSugarScope(BaseDBConfig.AllConfigs, delegate(SqlSugarClient db)
		{
			BaseDBConfig.ValidConfig.ForEach(delegate(ConnectionConfig config)
			{
				db.GetConnectionScope((string)config.ConfigId);
			});
		})));
	}

	private static string ExtractTableName(string sql)
	{
		Match match = new Regex("(?i)(?:FROM|UPDATE|DELETE\\s+FROM)\\s+`(.+?)`", RegexOptions.IgnoreCase).Match(sql);
		if (match.Success)
		{
			return match.Groups[1].Value;
		}
		return string.Empty;
	}
}
