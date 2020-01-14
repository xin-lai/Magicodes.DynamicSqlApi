namespace Magicodes.DynamicSqlApi.Core.Models
{
    /// <summary>
    /// TSQL输出字段信息
    /// </summary>
    public class TSqlOutputFieldInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool AllowNullable { get; set; }

        /// <summary>
        /// 数据库字段类型
        /// </summary>
        public string SqlTypeName { get; set; }

        /// <summary>
        /// C#中的参数类型
        /// </summary>
        public string CsTypeName { get; set; }
    }
}
