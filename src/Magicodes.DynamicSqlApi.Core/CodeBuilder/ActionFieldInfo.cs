namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    public class ActionFieldInfo
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public bool? AllowNullable { get; set; }

        public object DefaultValue { get; set; }
    }
}
