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

	// if it is client side code, auto detect game language
	public LanguageService(string feature, Language language = Language.English )
	{
		#if CLIENT
		language = _gameLanguageIdDict[API.GetCurrentLanguageId()];
		#endif
		_localizedStringSet = new LocalizedStringSet(feature, language);
		var fallbacks = LanguageDescription.Get(language).Fallbacks;
		_fallbackStringSets = fallbacks.Select(fallback => new LocalizedStringSet(feature, fallback)).ToList();
	}
	
	/*
	 since LocalizedStringSet.Get returns path if not found
	 if fallback strings also returns path, it means either:
	 1) value is the key 2) not defined in anyway
	 hence default to path(key)
	 */
	public string Get(string path)
	{
		var str = _localizedStringSet.Get(path);
		foreach (var fallbackStringSet in _fallbackStringSets)
		{
			if (fallbackStringSet.Get(path) != str) return fallbackStringSet.Get(path);
		}

		return str;
	}

	// same logic as Get()
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