using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using W8lessLabs.ScriptVersions;

namespace ClassicMVC
{
    public class UIScripts
    {
        private static ScriptVersionsFilePersist _scriptVersionsPersist;
        private static ScriptVersionsFile _scriptVersions;
        private static DateTimeOffset _lastLoaded = default(DateTimeOffset);

        public static async Task Load()
        {
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "scriptversions.json");
            _scriptVersionsPersist = new ScriptVersionsFilePersist(filePath);
            _scriptVersions = await _LoadLatest();
        }

        private static async Task<ScriptVersionsFile> _LoadLatest()
        {
            if (await _scriptVersionsPersist.IsNewer(_lastLoaded))
                return _scriptVersionsPersist.Load();
            else
                return _scriptVersions;
        }
    }
}