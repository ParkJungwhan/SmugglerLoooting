using LoootCreate.Services.DB;

namespace LoootCreate.Services.Lottery;

public class LottoManager
{
    private HashSet<string> hash = null;

    private LotteryDBManager lottoDB;

    public LottoManager(LotteryDBManager lottodb)
    {
        lottoDB = lottodb;
    }

    public void AllHistoryLoad()
    {
        if (lottoDB is null)
        {
            hash = new HashSet<string>();
        }

        hash = lottoDB.AllHistory();
    }
}