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
    }
}
