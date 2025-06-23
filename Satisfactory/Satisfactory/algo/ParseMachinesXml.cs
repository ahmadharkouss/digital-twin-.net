using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Satisfactory
{
    internal class ParseMachinesXml
    {
        public static IEnumerable<Machines> ParseMachines(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var l_xmlDoc = XDocument.Load(path);
            var l_lMachineList = new List<Machines>();

            l_lMachineList.Add(new Machines("start", 1, null,
                new[] { l_xmlDoc.Descendants("step").First().Attribute("id").Value }, 0));

            var l_lRest = l_xmlDoc.Descendants("step").Select(element =>
            {
                var l_elmId = element.Attribute("id")!.Value;
                var l_next = element.Attribute("next")?.Value;
                IEnumerable<string> l_str = l_next != null ? new List<string>() { l_next } : Enumerable.Empty<string>();
                int.TryParse(element.Attribute("capacity")!.Value, out var l_cap);
                return new Machines(l_elmId, l_cap, null, l_str, 0);
            }).Concat(l_xmlDoc.Descendants("exchange").Select(element =>
            {
                var l_elmId = element.Attribute("id")!.Value;
                var l_timeS = element.Attribute("time_s")!.Value;
                var l_connections = element.Descendants("connection").Select(x =>
                {
                    var l_id = x.Attribute("id")!.Value;
                    return l_id;
                });
                return new Machines(l_elmId, 1, null, l_connections, float.Parse(l_timeS, CultureInfo.InvariantCulture.NumberFormat));
            }));

            return l_lMachineList.Concat(l_lRest);
        }
    }
}