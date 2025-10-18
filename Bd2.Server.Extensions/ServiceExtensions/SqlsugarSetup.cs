using System;
using System.Reflection;
using Bd2.Server.Common.DB;
using Bd2.Server.Common;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace Bd2.Server.Extensions.ServiceExtensions;

public static class SqlsugarSetup
{
    public static void AddSqlsugarSetup(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (!string.IsNullOrEmpty(AppSettings.app("MainDB")))
        {
            MainDb.CurrentDbConnId = AppSettings.app("MainDB");
        }

        BaseDBConfig.MutiConnectionString.allDbs.ForEach(m =>
        {
            var connectionConfig = new ConnectionConfig
            {
                ConfigId = m.ConnId.ObjToString().ToLower(),
                ConnectionString = m.Connection,
                DbType = (DbType)m.DbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true,
                    SqlServerCodeFirstNvarchar = true
                }
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

        services.AddSingleton<ISqlSugarClient>(provider =>
        {
            var scope = new SqlSugarScope(BaseDBConfig.AllConfigs, db =>
            {
                BaseDBConfig.ValidConfig.ForEach(config =>
                {
                    db.GetConnectionScope(config.ConfigId);
                });
            });

            InitTablesFromNamespace(scope, "Bd2.Server.Model.DAO");
            InitTablesFromNamespace(scope, "Models");

            return scope;
        });
    }

    private static void InitTablesFromNamespace(SqlSugarScope sqlSugar, string targetNamespace)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null)!;
                }
            })
            .Where(t =>
                t != null &&
                t.IsClass &&
                !t.IsAbstract &&
                t.GetCustomAttribute<SugarTable>() != null &&
                t.Namespace != null &&
                (t.Namespace == targetNamespace || t.Namespace.StartsWith(targetNamespace + ".")))
            .ToList();

        int createdCount = 0;
        var createdTables = new List<string>();

        foreach (var type in types)
        {
            try
            {
                sqlSugar.CodeFirst.InitTables(type);
            }
            catch
            {
                // ignored
            }
        }
    }
}
