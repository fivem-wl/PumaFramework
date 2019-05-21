namespace PumaFramework.Shared.I18N
{

public interface ILanguageService
{
	#if CLIENT
	Language GetCurrentGameLanguage();
	#endif
	
	string Get(string path);

	string Format(string path, params object[] args);
}


}