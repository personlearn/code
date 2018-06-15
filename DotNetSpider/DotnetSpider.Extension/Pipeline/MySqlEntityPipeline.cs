﻿using DotnetSpider.Extension.Infrastructure;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.Extension.Pipeline
{
    /// <summary>
	/// 把解析到的爬虫实体数据存到MySql中
	/// </summary>
	public class MySqlEntityPipeline : DbModelPipeline
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connectString">数据库连接字符串, 如果为空框架会尝试从配置文件中读取</param>
        /// <param name="pipelineMode">数据管道模式</param>
        public MySqlEntityPipeline(string connectString = null, PipelineMode pipelineMode = PipelineMode.InsertAndIgnoreDuplicate) : base(connectString, pipelineMode)
        {
        }

        protected override IDbConnection CreateDbConnection(string connectString)
        {
            return new MySqlConnection(connectString);
        }

        protected override Sqls GenerateSqls(IModel model)
        {
            var sqls = new Sqls();
            sqls.InsertSql = GenerateInsertSql(model, false);
            sqls.InsertAndIgnoreDuplicateSql = GenerateInsertSql(model, true);
            sqls.InsertNewAndUpdateOldSql = GenerateInsertNewAndUpdateOldSql(model);
            sqls.UpdateSql = GenerateUpdateSql(model);
            sqls.SelectSql = GenerateSelectSql(model);
            return sqls;
        }

        protected override void InitDatabaseAndTable(IDbConnection conn, IModel model)
        {
            var database = IgnoreColumnCase ? model.TableInfo.Database.ToLower() : model.TableInfo.Database;
            conn.MyExecute($"CREATE SCHEMA IF NOT EXISTS `{database}` DEFAULT CHARACTER SET utf8mb4;");
            conn.MyExecute(GenerateCreateTableSql(model));
        }

        /// <summary>
        /// 构造创建数据表的SQL语句
        /// </summary>
        /// <param name="adapter">数据库管道使用的实体中间信息</param>
        /// <returns>SQL语句</returns>
        private string GenerateCreateTableSql(IModel model)
        {
            var tableName = IgnoreColumnCase ? model.TableInfo.FullName.ToLower() : model.TableInfo.FullName;
            var database = IgnoreColumnCase ? model.TableInfo.Database.ToLower() : model.TableInfo.Database;

            var fields = model.Fields;

            StringBuilder builder = new StringBuilder($"CREATE TABLE IF NOT EXISTS `{database}`.`{tableName}` (");

            var singleAutoIncrementPrimary = fields.Count(f => f.IsPrimary && (f.DataType == DataType.Int || f.DataType == DataType.Long)) == 1;

            foreach (var field in fields)
            {
                var columnSql = GenerateColumn(field);

                if (singleAutoIncrementPrimary && field.IsPrimary)
                {
                    builder.Append($"{columnSql} AUTO_INCREMENT, ");
                }
                else
                {
                    builder.Append($"{columnSql}, ");
                }
            }
            builder.Remove(builder.Length - 2, 2);

            if (AutoTimestamp)
            {
                builder.Append($", `creation_time` TIMESTAMP DEFAULT CURRENT_TIMESTAMP, `creation_date` DATE");
            }

            if (fields.Any(f => f.IsPrimary))
            {
                builder.Append($", PRIMARY KEY ({string.Join(", ", fields.Where(f => f.IsPrimary).Select(field => IgnoreColumnCase ? field.Name.ToLower() : field.Name))})");
            }

            if (model.TableInfo.Indexs != null)
            {
                foreach (var index in model.TableInfo.Indexs)
                {
                    var columns = index.Split(',');
                    string name = string.Join("_", columns.Select(c => c));
                    string indexColumNames = string.Join(", ", columns.Select(c => $"`{c}`"));
                    builder.Append($", KEY `index_{name}` ({indexColumNames})");
                }
            }
            if (model.TableInfo.Uniques != null)
            {
                foreach (var unique in model.TableInfo.Uniques)
                {
                    var columns = unique.Split(',');
                    string name = string.Join("_", columns.Select(c => c));
                    string uniqueColumNames = string.Join(", ", columns.Select(c => $"`{c}`"));
                    builder.Append($", UNIQUE KEY `unique_{name}` ({uniqueColumNames})");
                }
            }
            builder.Append(")");
            string sql = builder.ToString();
            return sql;
        }

        private string GenerateInsertSql(IModel model, bool ignoreDuplicate)
        {
            var fields = model.Fields;
            var singleAutoIncrementPrimary = fields.Count(f => f.IsPrimary && (f.DataType == DataType.Int || f.DataType == DataType.Long)) == 1;

            // 如果是单自增主键, 则不需要插入值
            var insertColumns = fields.Where(f => !f.IgnoreStore && (singleAutoIncrementPrimary ? !f.IsPrimary : true));

            string columnsSql = string.Join(", ", insertColumns.Select(p => $"`{(IgnoreColumnCase ? p.Name.ToLower() : p.Name)}`"));

            if (AutoTimestamp)
            {
                columnsSql = $"{columnsSql},`creation_time`, `creation_date`";
            }
            string columnsParamsSql = string.Join(", ", insertColumns.Select(p => $"@{p.Name}"));

            if (AutoTimestamp)
            {
                columnsParamsSql = $"{columnsParamsSql}, NOW(), CURRENT_DATE()";
            }

            var tableName = IgnoreColumnCase ? model.TableInfo.FullName.ToLower() : model.TableInfo.FullName;
            var database = IgnoreColumnCase ? model.TableInfo.Database.ToLower() : model.TableInfo.Database;
            var sql =
                $"INSERT {(ignoreDuplicate ? "IGNORE" : "")} INTO `{database}`.`{tableName}` ({columnsSql}) VALUES ({columnsParamsSql});";
            return sql;
        }

        private string GenerateInsertNewAndUpdateOldSql(IModel model)
        {
            var fields = model.Fields;
            var singleAutoIncrementPrimary = fields.Count(f => f.IsPrimary && (f.DataType == DataType.Int || f.DataType == DataType.Long)) == 1;

            // 如果是单自增主键, 则不需要插入值
            var insertColumns = fields.Where(f => !f.IgnoreStore && (singleAutoIncrementPrimary ? !f.IsPrimary : true));

            string columnsSql = string.Join(", ", insertColumns.Select(p => $"`{(IgnoreColumnCase ? p.Name.ToLower() : p.Name)}`"));

            if (AutoTimestamp)
            {
                columnsSql = $"{columnsSql},`creation_time`, `creation_date`";
            }
            string columnsParamsSql = string.Join(", ", insertColumns.Select(p => $"@{p.Name}"));
            if (AutoTimestamp)
            {
                columnsParamsSql = $"{columnsParamsSql}, NOW(), CURRENT_DATE()";
            }

            var tableName = IgnoreColumnCase ? model.TableInfo.FullName.ToLower() : model.TableInfo.FullName;
            var database = IgnoreColumnCase ? model.TableInfo.Database.ToLower() : model.TableInfo.Database;

            string setParams = string.Join(", ", insertColumns.Select(p => $"`{(IgnoreColumnCase ? p.Name.ToLower() : p.Name)}`=@{p.Name}"));

            var sql =
                $"INSERT INTO `{model.TableInfo.Database}`.`{tableName}` ({columnsSql}) VALUES ({columnsParamsSql}) ON DUPLICATE KEY UPDATE {setParams};";

            return sql;
        }

        private string GenerateUpdateSql(IModel model)
        {
            // 无主键, 无更新字段都无法生成更新SQL
            if (model.TableInfo.UpdateColumns == null || model.TableInfo.UpdateColumns.Count() == 0 || !model.Fields.Any(f => f.IsPrimary))
            {
                if (model.TableInfo.UpdateColumns == null || model.TableInfo.UpdateColumns.Count() == 0)
                {
                    Log.Logger.Warning("Can't generate update sql, in table info, the count of update columns is zero.");
                }
                else
                {
                    Log.Logger.Warning("Can't generate update sql, because in table info, because primary key is missing.");
                }
                return null;
            }

            var tableName = IgnoreColumnCase ? model.TableInfo.FullName.ToLower() : model.TableInfo.FullName;
            var database = IgnoreColumnCase ? model.TableInfo.Database.ToLower() : model.TableInfo.Database;

            var primaryKeys = model.Fields.Where(f => f.IsPrimary);
            string where = "";
            foreach (var field in model.Fields.Where(f => f.IsPrimary))
            {
                var primary = IgnoreColumnCase ? field.Name.ToLower() : field.Name;
                where += $" `{primary}` = @{field.Name} AND";
            }
            where = where.Substring(0, where.Length - 3);

            string setCols = string.Join(", ", model.TableInfo.UpdateColumns.Select(p => $"`{p.ToLower()}`=@{p}"));
            var sql = $"UPDATE `{database}`.`{tableName}` SET {setCols} WHERE {where};";
            return sql;
        }

        private string GenerateSelectSql(IModel model)
        {
            if (!model.Fields.Any(f => f.IsPrimary))
            {
                return null;
            }

            var tableName = IgnoreColumnCase ? model.TableInfo.FullName.ToLower() : model.TableInfo.FullName;
            var database = IgnoreColumnCase ? model.TableInfo.Database.ToLower() : model.TableInfo.Database;

            string where = "";
            foreach (var field in model.Fields.Where(f => f.IsPrimary))
            {
                var primary = IgnoreColumnCase ? field.Name.ToLower() : field.Name;
                where += $" `{primary}` = @{field.Name}";
            }

            var sql = $"SELECT * FROM `{database}`.`{tableName}` WHERE {where}";
            return sql;
        }

        private string GenerateColumn(Field field)
        {
            var columnName = IgnoreColumnCase ? field.Name.ToLower() : field.Name;
            var dataType = GetDataTypeSql(field.DataType, field.Length);
            if (field.IsPrimary)
            {
                dataType = $"{dataType} NOT NULL";
            }
            return $"`{columnName}` {dataType}";
        }

        private string GetDataTypeSql(DataType type, int length)
        {
            var dataType = "LONGTEXT";

            switch (type)
            {
                case DataType.Bool:
                    {
                        dataType = "BOOL";
                        break;
                    }
                case DataType.DateTime:
                    {
                        dataType = "TIMESTAMP DEFAULT CURRENT_TIMESTAMP";
                        break;
                    }
                case DataType.Date:
                    {
                        dataType = "DATE";
                        break;
                    }
                case DataType.Decimal:
                    {
                        dataType = "DECIMAL(18,2)";
                        break;
                    }
                case DataType.Double:
                    {
                        dataType = "DOUBLE";
                        break;
                    }
                case DataType.Float:
                    {
                        dataType = "FLOAT";
                        break;
                    }
                case DataType.Int:
                    {
                        dataType = "INT";
                        break;
                    }
                case DataType.Long:
                    {
                        dataType = "BIGINT";
                        break;
                    }
                default:
                    {
                        dataType = (length <= 0) ? "LONGTEXT" : $"VARCHAR({length})";
                        break;
                    }
            }
            return dataType;
        }
    }
}
