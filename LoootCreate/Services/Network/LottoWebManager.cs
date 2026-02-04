using LoootCreate.Models;
using LoootCreate.Services.DB;

namespace LoootCreate.Services.Network;

public class LottoWebManager
{
    private WebHttp web = new WebHttp();

    private LotteryDBManager lottoDB;

    public LottoWebManager(LotteryDBManager lottodb)
    {
        lottoDB = lottodb;
    }

    public void GetLottoData(int idx)
    {
        if (web is null)
        {
            Console.WriteLine("WebHttp instance is null.");
            return;
        }

        var webresult = web.GetHttp(string.Format(LOTTERYCONSTANTS.KR2026_LOTTERY_RANGE_URL, idx));
    }
}