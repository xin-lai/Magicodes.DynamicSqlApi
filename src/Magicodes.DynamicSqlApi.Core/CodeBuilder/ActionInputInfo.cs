using System.Collections.Generic;

namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    public class ActionInputInfo
    {
        public string BindFrom { get; set; }

        public ActionInputTypes ActionInputType { get; set; } = ActionInputTypes.None;

        public List<ActionFieldInfo> ActionFieldInfos { get; set; }
    }
}
