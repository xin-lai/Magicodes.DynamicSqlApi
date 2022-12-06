namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionBuilderInfo
    {
        public string Name { get; set; }

        public string HttpRoute { get; set; }

        public ActionOutputInfo ActionOutputInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ActionInputInfo ActionInputInfo { get; set; }
        public string SqlTpl { get; set; }

        public string Comment { get; set; }
        public string HttpMethod { get; internal set; }

        /// <summary>
        /// 是否存在返回值
        /// </summary>
        public bool HasReturnValue { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
