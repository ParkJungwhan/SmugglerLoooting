using System.Diagnostics;
using LoootCreate.Models;
using LoootCreate.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace LoootCreate;

internal class TeleBot
{
    private TelegramBotClient botClient;
    private LottoMaker maker;

    public TeleBot(LottoMaker lootmanager)
    {
        Debug.Assert(lootmanager != null);
        maker = lootmanager;

        try
        {
            maker.Init();

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
        try
        {
            //await botClient.DeleteWebhook(true);
            //var info = await botClient.GetWebhookInfo();
            //if (info is not null)
            //{
            //    Console.WriteLine($"Webhook URL: {info.Url}");
            //}

            if (botClient == null) throw new InvalidOperationException("Bot client is not initialized.");

            var me = await botClient.GetMe();
            Console.WriteLine($"{DateTime.Now} Start listening for @{me.Username}");

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Telegram Bot Init Failed: {e.Message}");
            return;
        }

        Console.ReadLine();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is Message message)
        {
            Console.WriteLine($"Recv Chat : {message.Text}");

            if (message.Text == "/lotto")
            {
                await botClient.SendMessage(message.Chat, $"{DateTime.Now} 로또 번호생성중...");

                // 여기서 로또 생성 로직 호출
                for (int i = 0; i < 5; i++)
                {
                    // make new lotto number
                    var result = maker.MakeNumber();
                    await Task.Delay(100);

                    string strMsg = string.Empty;
                    foreach (var seq in result)
                        strMsg += $", {seq}";

                    await botClient.SendMessage(message.Chat, $"{i + 1}: {strMsg.Substring(1)}");
                }
            }
            else
            {
                await botClient.SendMessage(message.Chat, "존재하지 않는 메뉴입니다.");
            }

            await botClient.SendMessage(message.Chat, $"{DateTime.Now} 번호 전송완료");
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException apiRequestException)
        {
            //await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
            await botClient.SendMessage(123, apiRequestException.ToString());
        }
    }
}