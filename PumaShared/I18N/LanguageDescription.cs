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

namespace PumaFramework.Shared.I18N {

public struct LanguageDescription
{
	static readonly IDictionary<Language, LanguageDescription> Descriptions = new Dictionary<Language, LanguageDescription>()
	{
		{
			Language.English,
			new LanguageDescription("en", "English", "English")
		},
		{
			Language.ChineseSimplified,
			new LanguageDescription("zh-cn", "Chinese (Simplified)", "中文（简体）", Language.ChineseTraditional, Language.English)
		},
		{
			Language.ChineseTraditional,
			new LanguageDescription("zh-tw", "Chinese (Traditional)", "中文（繁體）", Language.English)
		},
		{
			Language.Russian,
			new LanguageDescription("ru", "Russian", "Русский", Language.English)
		},
		{
			Language.French,
			new LanguageDescription("fr", "French", "Français", Language.English)
		},
		{
			Language.German,
			new LanguageDescription("de", "German", "Deutsch", Language.English)
		},
		{
			Language.Spanish,
			new LanguageDescription("es", "Spanish", "Español", Language.English)
		},
		{
			Language.Italian,
			new LanguageDescription("it", "Italian", "Italiano", Language.English)
		},
		{
			Language.Portuguese,
			new LanguageDescription("pr", "Portuguese", "Português", Language.English)
		},
		{
			Language.Dutch,
			new LanguageDescription("nl", "Dutch", "Nederlands", Language.English)
		},
		{
			Language.Polish,
			new LanguageDescription("pl", "Polish", "Polski", Language.English)
		},
		{
			Language.Swedish,
			new LanguageDescription("sv", "Swedish", "Svenska", Language.English)
		},
		{
			Language.Norwegian,
			new LanguageDescription("no", "Norwegian", "Norsk", Language.English)	
		},
		{
			Language.Danish,
			new LanguageDescription("da", "Danish", "Dansk", Language.English)
		},
		{
			Language.Czech,
			new LanguageDescription("sc", "Czech", "Čeština", Language.English)
		},
		{
			Language.Japanese,
			new LanguageDescription("ja", "Japanese", "日本語", Language.English)
		},
		{
			Language.Korean,
			new LanguageDescription("ko", "Korean", "한국어", Language.English)
		},
		{
			Language.Thai,
			new LanguageDescription("th", "Thai", "ภาษาไทย", Language.English)
		}
	};
	
	static readonly IDictionary<string, Language> CodeDict = new Dictionary<string, Language>();
	static LanguageDescription()
	{
		foreach (var entry in Descriptions) CodeDict[entry.Value.Code] = entry.Key;
	}

	public static Language? Get(string code) => CodeDict.TryGetValue(code, out var lang) ? lang : (Language?) null;
	public static LanguageDescription Get(Language language) => Descriptions[language];


	/// ISO 639-1 code
	public readonly string Code;
	
	public readonly string Name;
	
	public readonly string NativeName;
	
	public readonly Language[] Fallbacks;
	
	
	public LanguageDescription(string code, string name, string nativeName, params Language[] fallbacks)
	{
		Code = code;
		Name = name;
		NativeName = nativeName;
		Fallbacks = fallbacks;
	}
}

}