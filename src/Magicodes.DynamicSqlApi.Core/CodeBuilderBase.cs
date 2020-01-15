using Magicodes.DynamicSqlApi.Core.CodeBuilder;
using Magicodes.DynamicSqlApi.Core.DynamicApis;
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
                    Name = group.Key + "Controller",
                    Route = "api/[controller]",
                    Key = group.Key
                };

                foreach (var sqlApi in group.GetSection("SqlApi").GetChildren())
                {
                    var actionBuilderInfo = CreateActionBuilderInfo(sqlApi, out var sqlTpl);

                    //设置参数绑定方式
                    SetBindFrom(sqlApi, actionBuilderInfo);
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
                }
            }
        }

        /// <summary>
        /// 从SQL语句提取输出字段
        /// </summary>
        /// <param name="sqlTpl"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetActionOutputFieldInfosFromSql(string sqlTpl, ActionBuilderInfo actionBuilderInfo) => actionBuilderInfo.ActionOutputInfo.ActionFieldInfos = TSqlParser.GetOutputFieldList(sqlTpl).Select(p => new ActionFieldInfo()
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
                }
            }
        }

        /// <summary>
        /// 从SQL中提取输入参数
        /// </summary>
        /// <param name="sqlTpl"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetActionInputFieldInfosFromSql(string sqlTpl, ActionBuilderInfo actionBuilderInfo) => actionBuilderInfo.ActionInputInfo.ActionFieldInfos = TSqlParser.GetParameters(sqlTpl).Select(p => new ActionFieldInfo()
        {
            TypeName = p.CsTypeName,
            Name = p.Name?.TrimStart('@')
        }).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlApi"></param>
        /// <param name="sqlTpl"></param>
        /// <returns></returns>
        protected virtual ActionBuilderInfo CreateActionBuilderInfo(IConfigurationSection sqlApi, out string sqlTpl)
        {
            sqlTpl = sqlApi["SqlTpl"];
            return new ActionBuilderInfo()
            {
                Name = sqlApi.Key,
                HttpRoute = $"[HttpGet(\"{sqlApi.Key}\")]",
                ActionInputInfo = new ActionInputInfo()
                {
                    BindFrom = "[FromQuery]"
                },
                ActionOutputInfo = new ActionOutputInfo(),
                SqlTpl = sqlTpl
            };
        }

        /// <summary>
        /// 设置输入参数的绑定方式
        /// </summary>
        /// <param name="sqlApi"></param>
        /// <param name="actionBuilderInfo"></param>
        protected virtual void SetBindFrom(IConfigurationSection sqlApi, ActionBuilderInfo actionBuilderInfo)
        {
            //[FromQuery] - 从查询字符串中获取值。
            //[FromRoute] - 从路由数据中获取值。
            //[FromForm] - 从发布的表单域中获取值。
            //[FromBody] - 从请求正文中获取值。
            //[FromHeader] - 从 HTTP 标头中获取值。
            if (sqlApi.Key.StartsWith("update", StringComparison.OrdinalIgnoreCase) || sqlApi.Key.StartsWith("create", StringComparison.OrdinalIgnoreCase) || sqlApi.Key.StartsWith("put", StringComparison.OrdinalIgnoreCase) || sqlApi.Key.StartsWith("post", StringComparison.OrdinalIgnoreCase))
            {
                actionBuilderInfo.ActionInputInfo.BindFrom = "[FromBody]";
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
            ActionClassBuild();
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
                    CodeSb.AppendLine($"        [HttpGet(\"{actionBuilderInfos.Name}\")]");
                    CodeSb.Append("        public ");
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
                    CodeSb.Append($" {actionBuilderInfos.Name}(");
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
                    CodeSb.Append($"          return  SqlExecutor.Query<{actionBuilderInfos.Name}Output>(sqlText{(actionBuilderInfos.ActionInputInfo.ActionInputType == ActionInputTypes.None ? string.Empty : ",input")});").AppendLine();
                    CodeSb.AppendLine("        }");
                }
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
        protected virtual void ActionClassBuild()
        {
            foreach (var controllerBuilderInfo in CodeBuilderInfo.ControllerBuilderInfos)
            {
                foreach (var actionBuilderInfo in controllerBuilderInfo.ActionBuilderInfos)
                {
                    if (actionBuilderInfo.ActionInputInfo.ActionInputType != ActionInputTypes.None)
                    {
                        CodeSb.Append($"    public class {actionBuilderInfo.Name}Input").AppendLine();
                        CodeSb.AppendLine("    {");
                        foreach (var parameter in actionBuilderInfo.ActionInputInfo.ActionFieldInfos)
                        {
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
                    if (actionBuilderInfo.ActionOutputInfo.ActionOutputType != ActionOutputTypes.None)
                    {
                        CodeSb.Append($"    public class {actionBuilderInfo.Name}Output").AppendLine();
                        CodeSb.AppendLine("    {");
                        foreach (var parameter in actionBuilderInfo.ActionOutputInfo.ActionFieldInfos)
                        {
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
}
