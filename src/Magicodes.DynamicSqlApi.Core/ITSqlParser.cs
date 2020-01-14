using Magicodes.DynamicSqlApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core
{
    /// <summary>
    /// Sql解析器
    /// </summary>
    public interface ITSqlParser
    {
        /// <summary>
        /// 获取TSQL执行参数
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        IEnumerable<TSqlParameterInfo> GetParameters(string sqlText);

        /// <summary>
        /// 获取TSQL输出字段列表
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>

        IEnumerable<TSqlOutputFieldInfo> GetOutputFieldList(string sqlText);
    }
}
