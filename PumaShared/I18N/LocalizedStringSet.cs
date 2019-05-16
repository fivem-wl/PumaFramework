/*
 * This file is part of PumaFramework.
 *
 * PumaFramework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * PumaFramework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with PumaFramework.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CitizenFX.Core.Native;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace PumaFramework.Shared.I18N {

public class LocalizedStringSet
{
	readonly IDictionary<string, string> _stringSet = new Dictionary<string, string>();
	
	
	public LocalizedStringSet(string featureName, Language language)
	{
		string languageCode = LanguageDescription.Get(language).Code;
		string yamlData = API.LoadResourceFile(API.GetCurrentResourceName(), $"{featureName}/I18N/{languageCode}.yml");
		if (yamlData == null) return;
		
		var deserializer = new DeserializerBuilder().Build();
		var rawYaml = deserializer.Deserialize(new StringReader(yamlData));
		
		// use recursion to walk though to flatten it
		void VisitNode(object node, string path = "")
		{
			switch (node)
			{
				case YamlMappingNode dict:
					foreach (var entry in dict) VisitNode(entry.Value, (path.Length == 0) ? entry.Key.ToString() : $"{path}.{entry.Key}");
					break;
			
				case YamlSequenceNode seq:
					foreach (var entry in seq.Select((child, idx) => (child, idx))) VisitNode(entry, $"{path}[{entry.idx}]");
					break;

				default:
					_stringSet[path] = node.ToString();
					break;
			}
		}
		VisitNode(rawYaml);
	}

	public string Get(string path)
	{
		if (!_stringSet.TryGetValue(path, out var str)) return path;
		return str;
	}

	public string Format(string path, params object[] args)
	{
		return string.Format(Get(path), args);
	}
}

}