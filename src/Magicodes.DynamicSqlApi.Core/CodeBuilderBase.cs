using Magicodes.DynamicSqlApi.Core.CodeBuilder;
using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Magicodes.DynamicSqlApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core
{
    /// <summary>
    /// 代码构建器基类
    /// </summary>
    public abstract class CodeBuilderBase
    {
        public CodeBuilderBase(IConfiguration configuration, ITSqlParser tSqlParser)
        {
            Configuration = configuration;
            TSqlParser = tSqlParser;
        }

        public IConfiguration Configuration { get; }
        public ITSqlParser TSqlParser { get; }
        public CodeBuilderInfo CodeBuilderInfo { get; set; }

        /// <summary>
        /// 读取配置创建构建对象
        /// </summary>
        /// <returns></returns>
        public virtual CodeBuilderInfo CreateCodeBuilderInfo()
        {
            var codeBuilderInfo = new CodeBuilderInfo();
            CodeBuilderInfo = codeBuilderInfo;
            foreach (var group in Configuration.GetSection("SqlApis").GetChildren())
            {
                var controllerBuilderInfo = new ControllerBuilderInfo()
                {
                    Name = group.Key,
                    Route = "api/" + group.Key.ToCamelCase(),
                    Key = group.Key,
                    Comment = group["Comment"],
                };

                foreach (var sqlApi in group.GetSection("SqlApi").GetChildren())
                {
                    var actionBuilderInfo = CreateActionBuilderInfo(sqlApi, out var sqlTpl);

                    //设置参数绑定方式
                    SetHttpMethodAndBindFrom(sqlApi, actionBuilderInfo);
                    controllerBuilderInfo.ActionBuilderInfos.Add(actionBuilderInfo);

                    SetActionInputFieldInfosFromSql(sqlTpl, actionBuilderInfo);
                    SetActionInputFieldInfosFromConfig(sqlApi, actionBuilderInfo);

                    if (actionBuilderInfo.ActionInputInfo.ActionFieldInfos.Any())
                    {
                        actionBuilderInfo.ActionInputInfo.ActionInputType = ActionInputTypes.Object;
                    }

                    SetActionOutputFieldInfosFromSql(sqlTpl, actionBuilderInfo);
                    SetActionOutputFieldInfosFromConfig(sqlApi, actionBuilderInfo);

                    if (actionBuilderInfo.ActionOutputInfo.ActionFieldInfos.Any())
                    {
                        actionBuilderInfo.ActionOutputInfo.ActionOutputType = ActionOutputTypes.List;
                    }

                }

                codeBuilderInfo.ControllerBuilderInfos.Add(controllerBuilderInfo);
            }
            return codeBuilderInfo;
        }

        /// <summary>
        /// 从配置中获取或重写输出参数
        /// </summary>
        /// <param name="sqlApi"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetActionOutputFieldInfosFromConfig(IConfigurationSection sqlApi, ActionBuilderInfo actionBuilderInfo)
        {
            if (sqlApi.GetSection("Output:Parameter").Exists())
            {
                foreach (var par in sqlApi.GetSection("Output:Parameter").GetChildren())
                {
                    var name = par["Name"];
                    if (string.IsNullOrWhiteSpace(name))
                        continue;
                    var comment = par["Comment"];

                    var type = par["Type"];
                    var defaultValue = par["DefaultValue"];
                    var allowNullable = par["AllowNullable"];
                    var actionFieldInfo = actionBuilderInfo.ActionOutputInfo.ActionFieldInfos.FirstOrDefault(p => p.Name == name);
                    if (actionFieldInfo == null)
                    {
                        actionFieldInfo = new ActionFieldInfo() { Name = name };
                        actionBuilderInfo.ActionOutputInfo.ActionFieldInfos.Add(actionFieldInfo);
                    }

                    if (!string.IsNullOrWhiteSpace(type))
                        actionFieldInfo.TypeName = type;

                    if (!string.IsNullOrWhiteSpace(allowNullable))
                        actionFieldInfo.AllowNullable = Convert.ToBoolean(allowNullable);

                    if (!string.IsNullOrWhiteSpace(defaultValue))
                        actionFieldInfo.DefaultValue = defaultValue;
                    if (!comment.IsNullOrWhiteSpace()) actionFieldInfo.Comment = comment;
                }
            }
        }

        /// <summary>
        /// 从SQL语句提取输出字段
        /// </summary>
        /// <param name="sqlTpl"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetActionOutputFieldInfosFromSql(string sqlTpl, ActionBuilderInfo actionBuilderInfo) => actionBuilderInfo.ActionOutputInfo.ActionFieldInfos = TSqlParser.GetOutputFieldList(sqlTpl,actionBuilderInfo.ConnectionString).Select(p => new ActionFieldInfo()
        {
            TypeName = p.CsTypeName,
            Name = p.Name,
            AllowNullable = p.AllowNullable,
        }).ToList();

        /// <summary>
        /// 从配置文件获取或重写输入参数
        /// </summary>
        /// <param name="sqlApi"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetActionInputFieldInfosFromConfig(IConfigurationSection sqlApi, ActionBuilderInfo actionBuilderInfo)
        {
            if (sqlApi.GetSection("Input:Parameter").Exists())
            {
                foreach (var par in sqlApi.GetSection("Input:Parameter").GetChildren())
                {
                    var name = par["Name"];
                    var comment = par["Comment"];

                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    var type = par["Type"];
                    var defaultValue = par["DefaultValue"];
                    var allowNullable = par["AllowNullable"];
                    var actionFieldInfo = actionBuilderInfo.ActionInputInfo.ActionFieldInfos.FirstOrDefault(p => p.Name == name);

                    if (actionFieldInfo == null)
                    {
                        actionFieldInfo = new ActionFieldInfo() { Name = name };
                        actionBuilderInfo.ActionInputInfo.ActionFieldInfos.Add(actionFieldInfo);
                    }

                    if (!string.IsNullOrWhiteSpace(type))
                        actionFieldInfo.TypeName = type;

                    if (!string.IsNullOrWhiteSpace(allowNullable))
                        actionFieldInfo.AllowNullable = Convert.ToBoolean(allowNullable);

                    if (!string.IsNullOrWhiteSpace(defaultValue))
                        actionFieldInfo.DefaultValue = defaultValue;

                    if (!string.IsNullOrWhiteSpace(comment)) actionFieldInfo.Comment = comment;
                }
            }
        }

        /// <summary>
        /// 从SQL中提取输入参数
        /// </summary>
        /// <param name="sqlTpl"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetActionInputFieldInfosFromSql(string sqlTpl, ActionBuilderInfo actionBuilderInfo)
        {
            actionBuilderInfo.ActionInputInfo.ActionFieldInfos = TSqlParser.GetParameters(sqlTpl, actionBuilderInfo.ConnectionString).Select(p => new ActionFieldInfo()
            {
                TypeName = p.CsTypeName,
                Name = p.Name?.TrimStart('@')
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlApi"></param>
        /// <param name="sqlTpl"></param>
        /// <returns></returns>
        protected virtual ActionBuilderInfo CreateActionBuilderInfo(IConfigurationSection sqlApi, out string sqlTpl)
        {
            sqlTpl = sqlApi["SqlTpl"];
            var comment = sqlApi["Comment"];
            var httpMethod = sqlApi["HttpMethod"];
            return new ActionBuilderInfo()
            {
                Name = sqlApi.Key,
                HttpMethod = httpMethod,
                HttpRoute = sqlApi["HttpRoute"],
                ActionInputInfo = new ActionInputInfo()
                {
                    BindFrom = "[FromQuery]"
                },
                ActionOutputInfo = new ActionOutputInfo(),
                SqlTpl = sqlTpl,
                Comment = comment,
                HasReturnValue = sqlTpl.IndexOf("select", StringComparison.CurrentCultureIgnoreCase) != -1,
                ConnectionString = sqlApi["ConnectionString"]
            };
        }

        /// <summary>
        /// 设置输入参数的绑定方式
        /// </summary>
        /// <param name="sqlApi"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetHttpMethodAndBindFrom(IConfigurationSection sqlApi, ActionBuilderInfo actionBuilderInfo)
        {
            //[FromQuery] - 从查询字符串中获取值。
            //[FromRoute] - 从路由数据中获取值。
            //[FromForm] - 从发布的表单域中获取值。
            //[FromBody] - 从请求正文中获取值。
            //[FromHeader] - 从 HTTP 标头中获取值。
            var keys = new Dictionary<string, string[]>()
            {
                { "HttpPost" ,new string[]{ "create", "post", "insert" } },
                { "HttpGet" ,new string[]{ "get","select"} },
                { "HttpPut" ,new string[]{ "put", "update" } },
                { "HttpDelete" ,new string[]{ "delete", "drop","remove" } },
            };
            foreach (var item in keys.Keys)
            {
                if (keys[item].Any(p => sqlApi.Key.StartsWith(p, StringComparison.CurrentCultureIgnoreCase)))
                {
                    actionBuilderInfo.HttpMethod = item;
                    if(actionBuilderInfo.HttpRoute.IsNullOrWhiteSpace())
                        actionBuilderInfo.HttpRoute = $"[{item}(\"{sqlApi.Key.ToCamelCase()}\")]";
                }
            }
            switch (actionBuilderInfo.HttpMethod)
            {
                case "HttpGet":
                    actionBuilderInfo.ActionInputInfo.BindFrom = "[FromQuery]";
                    break;
                case "HttpPost":
                case "HttpPut":
                    actionBuilderInfo.ActionInputInfo.BindFrom = "[FromBody]";
                    break;
                case "HttpDelete":
                    actionBuilderInfo.ActionInputInfo.BindFrom = "[FromQuery]";
                    break;
                default:
                    break;
            }
        }

        protected StringBuilder CodeSb = new StringBuilder();
        public string Build()
        {
            CreateCodeBuilderInfo();
            if (CodeBuilderInfo == null)
            {
                return null;
            }

            CodeSb = new StringBuilder();
            NamespacesBuild();
            ControllersBuild();
            return CodeSb.ToString();
        }

        /// <summary>
        /// 添加命名空间
        /// </summary>
        /// <param name="namespaces"></param>
        public virtual void AddNamespace(params string[] namespaces) => CodeBuilderInfo.Namespaces.AddRange(namespaces);

        protected virtual void NamespacesBuild()
        {
            foreach (var ns in CodeBuilderInfo.Namespaces.Distinct())
            {
                CodeSb.AppendFormat("using {0};", ns).AppendLine();
            }
            CodeSb.AppendLine();
        }

        /// <summary>
        /// 构建控制器
        /// </summary>
        protected virtual void ControllersBuild()
        {
            foreach (var controllerBuilderInfo in CodeBuilderInfo.ControllerBuilderInfos)
            {
                CodeSb.AppendLine("    [DynamicApiController()]");
                CodeSb.AppendLine("    [ApiController]");
                CodeSb.AppendLine($"    [Route(\"{controllerBuilderInfo.Route}\")]");
                if (controllerBuilderInfo.Comment != null)
                    CodeSb.AppendLine($"    [SwaggerTag(\"{controllerBuilderInfo.Comment}\")]");
                CodeSb.AppendLine($"    public class {controllerBuilderInfo.Name}Controller : ControllerBase");
                CodeSb.AppendLine("    {");
                CodeSb.AppendLine($"        public {controllerBuilderInfo.Name}Controller(ISqlExecutor sqlExecutor, IConfiguration configuration)");
                CodeSb.AppendLine("        {");
                CodeSb.AppendLine("            SqlExecutor = sqlExecutor;");
                CodeSb.AppendLine("            Configuration = configuration;");
                CodeSb.AppendLine("        }");
                CodeSb.AppendLine("        public ISqlExecutor SqlExecutor { get; }");
                CodeSb.AppendLine("        public IConfiguration Configuration { get; }");
                foreach (var actionBuilderInfos in controllerBuilderInfo.ActionBuilderInfos)
                {
                    CodeSb.AppendLine();
                    CodeSb.AppendLine($"        {actionBuilderInfos.HttpRoute}");
                    if (!actionBuilderInfos.Comment.IsNullOrWhiteSpace())
                        CodeSb.AppendLine($"[SwaggerOperation(Summary = \"{actionBuilderInfos.Comment}\")]");
                    CodeSb.Append("        public async Task<");
                    switch (actionBuilderInfos.ActionOutputInfo.ActionOutputType)
                    {
                        case ActionOutputTypes.None:
                            CodeSb.Append("IActionResult");
                            break;
                        case ActionOutputTypes.List:
                            CodeSb.Append($"IEnumerable<{actionBuilderInfos.Name}Output>");
                            break;
                        default:
                            CodeSb.Append("IActionResult");
                            break;
                    }
                    CodeSb.Append($"> {actionBuilderInfos.Name}(");
                    switch (actionBuilderInfos.ActionInputInfo.ActionInputType)
                    {
                        case ActionInputTypes.None:
                            break;
                        case ActionInputTypes.Object:
                            CodeSb.Append($"{actionBuilderInfos.ActionInputInfo.BindFrom}{actionBuilderInfos.Name}Input input");
                            break;
                        default:
                            break;
                    }
                    CodeSb.Append(")").AppendLine();
                    CodeSb.AppendLine("        {");
                    CodeSb.AppendLine($"          var sqlText = Configuration[\"SqlApis:{controllerBuilderInfo.Key}:SqlApi:{actionBuilderInfos.Name}:SqlTpl\"];");
                    //SqlApis:AuditLogs:SqlApi:GetAbpAuditLogById:SqlTpl
                    //Console.WriteLine("SQL:" + Configuration[$"SqlApis:{controllerBuilderInfo.Name}:SqlApi:{actionBuilderInfos.Name}:SqlTpl"]);
                    if (actionBuilderInfos.HasReturnValue)
                    {
                        CodeSb.Append($"          return await SqlExecutor.QueryAsync<{actionBuilderInfos.Name}Output>(sqlText{(actionBuilderInfos.ActionInputInfo.ActionInputType == ActionInputTypes.None ? string.Empty : ",input")},connectionString:@\"{actionBuilderInfos.ConnectionString}\");").AppendLine();
                    }
                    else
                    {
                        CodeSb.Append($"          await SqlExecutor.ExecuteAsync(sqlText{(actionBuilderInfos.ActionInputInfo.ActionInputType == ActionInputTypes.None ? string.Empty : ",input")},connectionString:@\"{actionBuilderInfos.ConnectionString}\");").AppendLine();
                        CodeSb.AppendLine("return Ok();");
                    }

                    CodeSb.AppendLine("        }");
                }
                ActionClassBuild(controllerBuilderInfo);
                CodeSb.AppendLine("    }");
                CodeSb.AppendLine();
            }
        }

        protected virtual string GetParameterTypeName(ActionFieldInfo actionFieldInfo)
        {
            if (actionFieldInfo.AllowNullable == true && !actionFieldInfo.TypeName.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                return "?";
            }
            return null;
        }

        /// <summary>
        /// Action输入输出类构建
        /// </summary>
        protected virtual void ActionClassBuild(ControllerBuilderInfo controllerBuilderInfo)
        {
            foreach (var actionBuilderInfo in controllerBuilderInfo.ActionBuilderInfos)
            {
                //构建输入参数类
                if (actionBuilderInfo.ActionInputInfo.ActionInputType != ActionInputTypes.None)
                {
                    CodeSb.Append($"    public class {actionBuilderInfo.Name}Input").AppendLine();
                    CodeSb.AppendLine("    {");
                    foreach (var parameter in actionBuilderInfo.ActionInputInfo.ActionFieldInfos)
                    {
                        if (!parameter.Comment.IsNullOrWhiteSpace())
                            CodeSb.AppendLine($"[SwaggerSchema(\"{parameter.Comment}\")]");
                        CodeSb.Append($"        public {parameter.TypeName}{GetParameterTypeName(parameter)} {parameter.Name} {{ get; set; }}");
                        if (parameter.DefaultValue != null)
                        {
                            if (parameter.TypeName == "string")
                                CodeSb.Append($" = \"{parameter.DefaultValue}\";");
                            else
                                CodeSb.Append($" = {parameter.DefaultValue};");
                        }
                        CodeSb.AppendLine();
                    }

                    CodeSb.AppendLine("    }");
                }

                //构建输出参数类
                if (actionBuilderInfo.ActionOutputInfo.ActionOutputType != ActionOutputTypes.None)
                {
                    CodeSb.Append($"    public class {actionBuilderInfo.Name}Output").AppendLine();
                    CodeSb.AppendLine("    {");
                    foreach (var parameter in actionBuilderInfo.ActionOutputInfo.ActionFieldInfos)
                    {
                        if (!parameter.Comment.IsNullOrWhiteSpace())
                            CodeSb.AppendLine($"[SwaggerSchema(\"{parameter.Comment}\")]");
                        CodeSb.Append($"        public {parameter.TypeName}{GetParameterTypeName(parameter)} {parameter.Name.TrimStart('@')} {{ get; set; }}");
                        if (parameter.DefaultValue != null)
                        {
                            if (parameter.TypeName == "string")
                                CodeSb.Append($" = \"{parameter.DefaultValue}\";");
                            else
                                CodeSb.Append($" = {parameter.DefaultValue};");
                        }
                        CodeSb.AppendLine();
                    }

                    CodeSb.AppendLine("    }");
                }
            }
        }
    }
}
