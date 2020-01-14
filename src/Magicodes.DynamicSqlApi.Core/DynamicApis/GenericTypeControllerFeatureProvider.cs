using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Magicodes.DynamicSqlApi.Core.DynamicApis
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public GenericTypeControllerFeatureProvider(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var candidates = Assembly.GetExportedTypes().Where(x => x.GetCustomAttributes<DynamicApiControllerAttribute>().Any());

            foreach (var candidate in candidates)
            {
                feature.Controllers.Add(
                    candidate.GetTypeInfo()

                );
            }
        }
    }
}
