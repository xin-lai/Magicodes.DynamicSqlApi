using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    /// <summary>
    /// 默认的代码构建
    /// </summary>
    public class DefaultCodeBuilder : CodeBuilderBase
    {
        public DefaultCodeBuilder(IConfiguration configuration, ITSqlParser tSqlParser) : base(configuration, tSqlParser)
        {
        }
    }
}
