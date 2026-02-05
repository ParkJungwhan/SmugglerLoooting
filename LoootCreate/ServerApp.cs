using LoootCreate.Services;
using LoootCreate.Services.DB;
using LoootCreate.Services.Lottery;
using LoootCreate.Services.Network;

namespace LoootCreate;

public class ServerApp
{
    private LottoManager LootManager;

    public bool InitApp()
    {
        bool bConfig = AppSetting.LoadConfig();
        if (false == bConfig)
        {
            Console.WriteLine("Config Load Fail. Exit");
            Console.ReadLine();
            return false;
        }

        // DB 초기화
        DBManager.Instance.InitializeDBManagers();

        // 로또 History 가져오기
        // 1. DB에서 먼저 가져오기
        // 2. api에서 가져오기
        LootManager = new LottoManager();
        LootManager.AllHistoryLoad();

        return true;
    }

    public bool StartApp()
    {
        TeleBot bot = new TeleBot(LootManager);
        bot.StartBot();
        return true;
    }
}