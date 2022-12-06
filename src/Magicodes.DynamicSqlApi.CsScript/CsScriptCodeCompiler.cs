using CSScriptLib;
using Magicodes.DynamicSqlApi.Core;
using System;
using System.Reflection;

namespace Magicodes.DynamicSqlApi.CsScript
{
    /// <summary>
    /// CSScript代码编译器
    /// </summary>
    public class CsScriptCodeCompiler : ICodeCompiler
    {
        /// <summary>
        /// 使用CSScript Roslyn编译器编译代码，无需安装SDK，但不支持命名空间
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Assembly CompileCode(string code) => CSScript.RoslynEvaluator.CompileCode(code);
    }
}
