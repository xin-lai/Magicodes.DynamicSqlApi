using System.Collections.Generic;

namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    /// <summary>
    /// 控制器构建信息列表
    /// </summary>
    public class ControllerBuilderInfo
    {
        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 原始Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Action列表
        /// </summary>
        public List<ActionBuilderInfo> ActionBuilderInfos { get; set; } = new List<ActionBuilderInfo>();
    }
}
