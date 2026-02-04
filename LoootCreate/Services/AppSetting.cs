using LoootCreate.Models;
using LoootCreate.Services.DB;
using LoootCreate.Services.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoootCreate.Services
{
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

                    DBInfo.ConnectionString = cfg;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read config: {ex.Message}. Using defaults.");
                bRet = false;
            }

            return bRet;
        }

        public static string DBInit(DBConnection DBconn)
        {
            if (false == DBconn.IsConnected())
            {
                Console.WriteLine("DB connection failed.");
                return null;
            }

            AccountDBManager accountdb = new AccountDBManager(DBconn.Conn);
            var keyvalue = accountdb.GetTableKey("telegrambot", "lottery");
            if (true == string.IsNullOrEmpty(keyvalue))
            {
                Console.WriteLine("[DB] No result value!!!");
            }

            return keyvalue;
        }
    }
}