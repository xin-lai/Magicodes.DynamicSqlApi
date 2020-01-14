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
    public class DefaultCodeBuilder : ICodeBuilder
    {
        protected List<string> Namespaces = new List<string>()
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Threading.Tasks",
            "Microsoft.AspNetCore.Mvc",
            "Microsoft.Extensions.Logging",
            typeof(ISqlExecutor).Namespace,
            typeof(IConfiguration).Namespace
        };

        public DefaultCodeBuilder(IConfiguration configuration, ITSqlParser tSqlParser)
        {
            Configuration = configuration;
            TSqlParser = tSqlParser;
        }

        public IConfiguration Configuration { get; }
        public ITSqlParser TSqlParser { get; }

        public virtual void AddNamespace(params string[] namespaces) => Namespaces.AddRange(namespaces);
        public virtual string ApiInputClassBuild()
        {
            var codeSb = new StringBuilder();
            var sections = Configuration.GetSection("SqlApis:SqlApi").GetChildren();
            foreach (var item in sections)
            {
                var sqlTpl = item["SqlTpl"];
                //默认根据SQL语句自动生成参数
                if (!CreateInputParsBySql(codeSb, item, sqlTpl))
                    CreateInputParsByConfig(codeSb, item);
            }
            return codeSb.ToString();
        }

        /// <summary>
        /// 自动根据SQL语句创建输入参数
        /// </summary>
        /// <param name="codeSb"></param>
        /// <param name="item"></param>
        /// <param name="sqlTpl"></param>
        /// <returns></returns>
        protected virtual bool CreateInputParsBySql(StringBuilder codeSb, IConfigurationSection item, string sqlTpl)
        {
            var parameters = TSqlParser.GetParameters(sqlTpl);
            if (!parameters.Any()) return false;
            codeSb.Append("    public class ").Append(item.Key).Append("Input").AppendLine();
            codeSb.AppendLine("    {");
            foreach (var parameter in parameters)
            {
                codeSb.Append("        public ").Append(parameter.CsTypeName).Append(" ").Append(parameter.Name.TrimStart('@')).AppendLine(" {get;set;}");
            }
            codeSb.AppendLine("    }");
            return true;
        }

        protected virtual bool CreateOutputParsBySql(StringBuilder codeSb, IConfigurationSection item, string sqlTpl)
        {
            var parameters = TSqlParser.GetOutputFieldList(sqlTpl);
            if (!parameters.Any()) return false;

            codeSb.Append("    public class ").Append(item.Key).Append("Output").AppendLine();
            codeSb.AppendLine("    {");
            foreach (var parameter in parameters)
            {
                codeSb.Append("        public ").Append(parameter.CsTypeName).Append(parameter.AllowNullable ? (parameter.CsTypeName != "string" ? "?" : string.Empty) : string.Empty).Append(" ").Append(parameter.Name).AppendLine(" {get;set;}");
            }
            codeSb.AppendLine("    }");
            return true;
        }

        private static void CreateOutputParsByConfig(StringBuilder codeSb, IConfigurationSection item)
        {
            if (item.GetSection("Output:Parameter").Exists())
            {
                codeSb.Append("    public class ").Append(item.Key).Append("Output").AppendLine();
                codeSb.AppendLine("    {");
                foreach (var par in item.GetSection("Output:Parameter").GetChildren())
                {
                    codeSb.Append("        public ").Append(par["Type"] ?? "string").Append(" ").Append(par["Name"]).AppendLine(" {get;set;}");
                }
                codeSb.AppendLine("    }");
            }
        }

        private static void CreateInputParsByConfig(StringBuilder codeSb, IConfigurationSection item)
        {
            if (item.GetSection("Input:Parameter").Exists())
            {
                codeSb.Append("    public class ").Append(item.Key).Append("Input").AppendLine();
                codeSb.AppendLine("    {");
                foreach (var par in item.GetSection("Input:Parameter").GetChildren())
                {
                    codeSb.Append("        public ").Append(par["Type"]).Append(" ").Append(par["Name"]).AppendLine(" {get;set;}");
                }
                codeSb.AppendLine("    }");
            }
        }

        public virtual string ApiOutputClassBuild()
        {
            var codeSb = new StringBuilder();
            var sections = Configuration.GetSection("SqlApis:SqlApi").GetChildren();
            foreach (var item in sections)
            {
                var sqlTpl = item["SqlTpl"];
                //默认根据SQL语句自动生成输出参数
                if (!CreateOutputParsBySql(codeSb, item, sqlTpl))
                    CreateOutputParsByConfig(codeSb, item);
            }
            return codeSb.ToString();
        }

        public virtual string ControllerClassBuild()
        {
            var codeSb = new StringBuilder();
            var sections = Configuration.GetSection("SqlApis:SqlApi").GetChildren();
            codeSb.AppendLine("    [DynamicApiController(\"api/dyapi\")]");
            codeSb.AppendLine("    [ApiController]");
            codeSb.AppendLine("    [Route(\"[controller]\")]");
            codeSb.AppendLine("    public class DyApiController : ControllerBase");
            codeSb.AppendLine("    {");
            codeSb.AppendLine("        public DyApiController(DbAccesstor dbAccesstor, IConfiguration configuration)");
            codeSb.AppendLine("        {");
            codeSb.AppendLine("            DbAccesstor = dbAccesstor;");
            codeSb.AppendLine("            Configuration = configuration;");
            codeSb.AppendLine("        }");
            codeSb.AppendLine("        public DbAccesstor DbAccesstor { get; }");
            codeSb.AppendLine("        public IConfiguration Configuration { get; }");
            foreach (var item in sections)
            {
                codeSb.AppendLine();
                codeSb.AppendFormat("        [HttpGet(\"{0}\")]", item.Key).AppendLine();
                codeSb.AppendFormat("        public IEnumerable<{0}Output> {0}([FromQuery]{0}Input input)", item.Key).AppendLine();
                codeSb.AppendLine("        {");
                codeSb.AppendFormat("          return  DbAccesstor.Query<{0}Output>(Configuration[\"SqlApis:SqlApi:{0}:SqlTpl\"],input);", item.Key).AppendLine();
                codeSb.AppendLine("        }");
            }
            codeSb.AppendLine("    }");
            return codeSb.ToString();
        }

        public string NamespacesBuild()
        {
            var codeSb = new StringBuilder();
            foreach (var ns in Namespaces.Distinct())
            {
                codeSb.AppendFormat("using {0};", ns).AppendLine();
            }
            codeSb.AppendLine();
            return codeSb.ToString();
        }

        public string Build()
        {
            var codeSb = new StringBuilder();
            codeSb.AppendLine(NamespacesBuild())
                .AppendLine(ControllerClassBuild())
                .AppendLine(ApiInputClassBuild())
                .AppendLine(ApiOutputClassBuild());
            return codeSb.ToString();
        }
    }
}
