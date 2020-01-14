namespace Magicodes.DynamicSqlApi.SqlServer
{
    public class GetSqlServerParametersOutput
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string suggested_system_type_name { get; set; }
    }
}