namespace Magicodes.DynamicSqlApi.Core.Models
{
    /// <summary>
    /// TSQL参数信息
    /// </summary>
    public class TSqlParameterInfo
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string SqlTypeName { get; set; }

        /// <summary>
        /// C#中的参数类型
        /// </summary>
        public string CsTypeName { get; set; }
    }
}
