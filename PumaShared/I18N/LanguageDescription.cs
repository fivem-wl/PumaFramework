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
		// TODO: ...
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