using System.Diagnostics;
using System.Security.Cryptography;
using LoootCreate.Services.Lottery;

namespace LoootCreate.Services;

public class LottoMaker
{
    public static int[] All_Number = Enumerable.Range(1, 45).ToArray();
    private int[] number;
    private Random my_rand;

    private LottoManager LootManager;

    public LottoMaker()
    {
        number = new int[6];
        my_rand = new Random();
        LootManager = new LottoManager();
    }

    internal void Init()
    {
        Debug.Assert(LootManager != null);
        LootManager.AllHistoryLoad();
    }

    public int[] MakeNumber()
    {
        if (LootManager.hash == null) LootManager.AllHistoryLoad();

        var allspan = All_Number.AsSpan();

        int[] rand6num = null;

        for (int i = 0; i < 1000000; i++)
        {
            RandomNumberGenerator.Shuffle(allspan);
            rand6num = allspan.ToArray().Take(6).Order().ToArray();
            string newKey = $"{rand6num[0]},{rand6num[1]},{rand6num[2]},{rand6num[3]},{rand6num[4]},{rand6num[5]}";

            // 중복없으니 리턴해야할듯
            if (false == LootManager.hash.Contains(newKey)) break;
        }
        return rand6num;
    }
}