using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;

namespace PumaFramework.Shared.I18N
{

public class LanguageService : ILanguageService
{
	readonly Dictionary<int, Language> _gameLanguageIdDict = new Dictionary<int, Language>
	{
		{0, Language.English},
		{1, Language.French},
		{2, Language.German},
		{3, Language.Italian},
		{4, Language.Spanish},
		{5, Language.Portuguese},
		{6, Language.Polish},
		{7, Language.Russian},
		{8, Language.Korean},
		{9, Language.ChineseTraditional},
		{10, Language.Japanese},
		// Mexico
		{11, Language.Spanish},
		{12, Language.ChineseSimplified}
	};
	
	readonly LocalizedStringSet _localizedStringSet;

	readonly List<LocalizedStringSet> _fallbackStringSets;

	#if CLIENT
	public LanguageService(string feature)
	{
		_localizedStringSet = new LocalizedStringSet(feature, _gameLanguageIdDict[API.GetCurrentLanguageId()]);
		var fallbacks = LanguageDescription.Get(_gameLanguageIdDict[API.GetCurrentLanguageId()]).Fallbacks;
		_fallbackStringSets = fallbacks.Select(fallback => new LocalizedStringSet(feature, fallback)).ToList();
	}
	#endif
	
	public string Get(string path)
	{
		var str = _localizedStringSet.Get(path);
		if (!path.Equals(str)) return str;
		foreach (var fallbackStringSet in _fallbackStringSets)
		{
			str = fallbackStringSet.Get(path);
			if (!path.Equals(str)) return str;
		}

		// return #+path+# if both are empty
		return $"#{str}#";
	}

	public string Format(string path, params object[] args)
	{
		var str = _localizedStringSet.Format(path, args);
		if (!path.Equals(str)) return str;
		foreach (var fallbackStringSet in _fallbackStringSets)
		{
			str = fallbackStringSet.Format(path, args);
			if (!path.Equals(str)) return str;
		}

		// return #+path+# if both are empty
		return $"#{str}#";
	}
}

}