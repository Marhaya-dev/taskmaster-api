using Newtonsoft.Json.Linq;
using System.IO;
using TaskMaster.Domain.Settings;

namespace TaskMaster.Domain.Utils
{
    public static class JsonUtils
    {
        public static DbConnectionSettings ReadDbConnection(
            string fileName = "appsettings.json", string sectionName = "DbConnectionSettings")
        {
            var json = JObject.Parse(File.ReadAllText(fileName));

            if (json.ContainsKey(sectionName))
            {
                return json[sectionName].ToObject<DbConnectionSettings>();
            }

            return new DbConnectionSettings();
        }
    }
}
