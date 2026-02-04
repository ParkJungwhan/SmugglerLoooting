using LoootCreate.Models;
using LoootCreate.Services;
using LoootCreate.Services.DB;

namespace LoootCreate;

internal class Program
{
    private static void Main(string[] args)
    {
        // Console.WriteLine("Hello, World!");
        // monitoring 서버에 재시작 알림 필요

        bool bConfig = AppSetting.LoadConfig();
        if (!bConfig)
        {
            Console.WriteLine("Config Load Fail. Exit");
            Console.ReadLine();
            return;
        }

        DBConnection dbConnection = new DBConnection();
        dbConnection.SetConnection(DBInfo.ConnectionString);
        // DB Init

        string key = AppSetting.DBInit(dbConnection);

        TeleBot bot = new TeleBot(dbConnection, key);
        bot.StartBot();

        Console.WriteLine($"{DateTime.Now}\tPress Enter to exit");
        Console.ReadLine();
    }
}