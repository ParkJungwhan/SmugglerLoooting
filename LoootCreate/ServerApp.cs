using LoootCreate.Services;
using LoootCreate.Services.DB;

namespace LoootCreate;

public class ServerApp
{
    private LottoMaker maker;

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
        maker = new LottoMaker();

        return true;
    }

    public bool StartApp()
    {
        TeleBot bot = new TeleBot(maker);
        bot.StartBot();
        return true;
    }
}