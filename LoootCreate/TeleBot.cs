using System.Diagnostics;
using LoootCreate.Models;
using LoootCreate.Services.Lottery;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace LoootCreate;

internal class TeleBot
{
    private TelegramBotClient botClient;
    private LottoManager LootManager;

    //public TeleBot(DBConnection dbmgr, string botkey)
    public TeleBot(LottoManager lootmanager)
    {
        Debug.Assert(lootmanager != null);
        LootManager = lootmanager;

        try
        {
            //LotteryDB = new LotteryDBManager(dbmgr);

            Debug.Assert(false == string.IsNullOrEmpty(AppConfig.TelegramBotToken));

            botClient = new TelegramBotClient(AppConfig.TelegramBotToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Telegram Bot Client Creation Failed: {ex.Message}");
            throw;
        }
    }

    public async void StartBot()
    {
        //Lotto = new LottoMaker();

        try
        {
            // await botClient.DeleteWebhook(true);
            //var info = await botClient.GetWebhookInfo();
            //if (info is not null)
            //{
            //    Console.WriteLine($"Webhook URL: {info.Url}");
            //}

            if (botClient != null) return;

            var me = await botClient.GetMe();
            Console.WriteLine($"{DateTime.Now} Start listening for @{me.Username}");

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Telegram Bot Init Failed: {e.Message}");
            return;
        }

        //Console.Title = me.Username;
        Console.ReadLine();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is Message message)
        {
            Console.WriteLine($"Recv Chat : {message.Text}");

            if (message.Text == "/lotto")
            {
                // make new lotto number
                //await botClient.SendTextMessageAsync(message.Chat, "새로운 로또 번호를 추출합니다(시간이 걸릴수 있습니다).");
                await botClient.SendMessage(message.Chat, "새로운 로또 번호를 추출합니다(시간이 걸릴수 있습니다).");

                await Task.Delay(1000);
                await botClient.SendMessage(message.Chat, "Test : Bot Test 입니다");

                // 여기서 로또 생성 로직 호출
                //for (int i = 0; i < 5; i++)
                //{
                //    var result = Lotto.Make();
                //    result.Sort();

                //    string strMsg = string.Empty;
                //    foreach (var seq in result)
                //        strMsg += $",{seq}";

                //    //await botClient.SendTextMessageAsync(message.Chat, strMsg.Substring(1));
                //    await botClient.SendMessage(message.Chat, strMsg.Substring(1));
                //}
            }
            else
            {
                await botClient.SendMessage(message.Chat, "존재하지 않는 메뉴입니다.");
            }

            await botClient.SendMessage(message.Chat, "전송완료");
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException apiRequestException)
        {
            //await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
            //await botClient.SendMessage(123, apiRequestException.ToString());
        }
    }
}