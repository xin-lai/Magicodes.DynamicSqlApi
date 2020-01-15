using System;
using System.Collections.Generic;
using System.Text;

namespace Magicodes.DynamicSqlApi.Core.DynamicApis
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DynamicApiControllerAttribute : Attribute
    {
    }
}
