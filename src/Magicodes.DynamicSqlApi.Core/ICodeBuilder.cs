using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core
{
    public interface ICodeBuilder
    {
        /// <summary>
        /// 添加命名空间
        /// </summary>
        /// <param name="namespaces"></param>
        void AddNamespace(params string[] namespaces);

        string NamespacesBuild();

        /// <summary>
        /// 控制器类构建
        /// </summary>
        /// <returns></returns>
        string ControllerClassBuild();

        /// <summary>
        /// 输入参数类构建
        /// </summary>
        /// <returns></returns>
        string ApiInputClassBuild();

        /// <summary>
        /// 输出参数类构建
        /// </summary>
        /// <returns></returns>
        string ApiOutputClassBuild();

        /// <summary>
        /// 构建所有
        /// </summary>
        /// <returns></returns>
        string Build();
    }
}
