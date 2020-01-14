using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Magicodes.DynamicSqlApi.SqlServer
{
    public static class SqlServerDbTypeMapperHelper
    {
        /// <summary>
        /// 根据数据库类型获取C#类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string GetCsTypeByDbType(this string dbType)
        {
            string type;
            switch (dbType.Split('(')[0])
            {
                case "bit":
                    type = "bool";
                    break;
                case "tinyint":
                case "smallint":
                    type = "short";
                    break;
                case "int":
                    type = "int";
                    break;
                case "bigint":
                    type = "long";
                    break;
                case "smallmoney":
                case "money":
                case "float":
                case "real":
                case "numeric":
                    type = "decimal";
                    break;
                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "datetimeoffset":
                    type = "DateTime";
                    break;
                default:
                    type = "string";
                    break;
            }

            return type;
        }
    }
}
