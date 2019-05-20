namespace PumaFramework.Shared.I18N
{

public interface ILanguageService
{
	string Get(string path);

	string Format(string path, params object[] args);
}


}