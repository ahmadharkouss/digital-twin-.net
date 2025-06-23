using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Satisfactory;

internal static class ParseTypesXml
{
    public static IEnumerable<Type> ParseTypes(string path)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        var l_xmlDoc = XDocument.Load(path);
        return l_xmlDoc.Descendants("type").Select(element =>
        {
            var l_elmId = element.Attribute("id")!.Value;
            var l_elmName = element.Attribute("name")!.Value;
            var l_steps = element.Descendants("step").Select(x =>
            {
                var l_id = x.Attribute("id")!.Value;
                var l_description = x.Attribute("description")!.Value;
                double.TryParse(x.Attribute("time_s")!.Value, out var l_time);
                return new Step(l_id, l_description, l_time);
            });
            return new Type(l_elmId, l_elmName, l_steps);
        });
    }
}