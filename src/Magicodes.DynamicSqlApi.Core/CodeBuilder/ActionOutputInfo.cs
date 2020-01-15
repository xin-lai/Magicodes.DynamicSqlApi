using System.Collections.Generic;

namespace Magicodes.DynamicSqlApi.Core.CodeBuilder
{
    public class ActionOutputInfo
    {
        public ActionOutputTypes ActionOutputType { get; set; } = ActionOutputTypes.None;

        public List<ActionFieldInfo> ActionFieldInfos { get; set; }
    }

    public enum ActionOutputTypes
    {
        None,
        List,
    }
}