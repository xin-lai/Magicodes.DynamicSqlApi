using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    /// <summary>
    /// 代码构建信息
    /// </summary>
    public class CodeBuilderInfo
    {
        /// <summary>
        /// 命名空间列表
        /// </summary>
        public List<string> Namespaces { get; set; } = new List<string>()
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Threading.Tasks",
            "Microsoft.AspNetCore.Mvc",
            "Microsoft.Extensions.Logging",
            typeof(ISqlExecutor).Namespace,
            typeof(IConfiguration).Namespace,
            typeof(DynamicApiControllerAttribute).Namespace
        };

        /// <summary>
        /// 控制器构建信息
        /// </summary>
        public List<ControllerBuilderInfo> ControllerBuilderInfos { get; set; } = new List<ControllerBuilderInfo>();

    }
}
