using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core
{
    /// <summary>
    /// 动态代码编译器
    /// </summary>
    public interface ICodeCompiler
    {
        /// <summary>
        /// 将代码编译成程序集
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Assembly CompileCode(string code);
    }
}
