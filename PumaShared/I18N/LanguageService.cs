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

	public LanguageService(string feature, Language language = Language.English )
	{
		#if CLIENT
		language = _gameLanguageIdDict[API.GetCurrentLanguageId()];
		#endif
		_localizedStringSet = new LocalizedStringSet(feature, language);
		var fallbacks = LanguageDescription.Get(language).Fallbacks;
		_fallbackStringSets = fallbacks.Select(fallback => new LocalizedStringSet(feature, fallback)).ToList();
	}
	
	
	public string Get(string path)
	{
		var str = _localizedStringSet.Get(path);
		foreach (var fallbackStringSet in _fallbackStringSets)
		{
			if (fallbackStringSet.Get(path) != str) return fallbackStringSet.Get(path);
		}

		return str;
	}

	public string Format(string path, params object[] args)
	{
		var str = _localizedStringSet.Format(path, args);
		foreach (var fallbackStringSet in _fallbackStringSets)
		{
			if (fallbackStringSet.Format(path, args) != str) return fallbackStringSet.Format(path, args);
		}

		return str;
	}
}

}