using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;


namespace Mod.ModMenu
{
    internal class LangHelper
    {
        static ResourceManager _rm;

        static LangHelper()
        {
            _rm = new ResourceManager("Mod.ModMenu.Locales.Strings", Assembly.GetExecutingAssembly());
        }

        public static string GetString(string name)
        {
            return _rm.GetString(name);
        }

        public static void ChangeLanguage(string language)
        {
            var cultureInfo = new CultureInfo(language);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
