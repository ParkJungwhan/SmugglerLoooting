using LoootCreate.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoootCreate.Services;

public class AppSetting
{
    public static bool LoadConfig()
    {
        const string file = "appsettings.json";

        bool bRet = false;

        try
        {
            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);

                var jsondata = JsonConvert.DeserializeObject<JObject>(json);
                var connValue = jsondata["ConnectionString"];
                var cfg = connValue.Value<string>();
                if (cfg != null) bRet = true;

                AppConfig.ConnectionString = cfg;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read config: {ex.Message}. Using defaults.");
            bRet = false;
        }

        return bRet;
    }
}