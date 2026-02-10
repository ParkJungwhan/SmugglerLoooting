using LoootCreate.Models;
using Newtonsoft.Json;

namespace LoootCreate.Services.Network;

public class LottoWebManager
{
    private WebHttp web = new WebHttp();

    public Lottery2026Info GetLottoData(int idx)
    {
        Lottery2026Info myDeserializedClass = null;

        if (web is null)
        {
            Console.WriteLine("WebHttp instance is null.");
            return myDeserializedClass;
        }

        try
        {
            var webresult = web.GetHttp(string.Format(LOTTERYCONSTANTS.KR2026_LOTTERY_RANGE_URL, idx));

            myDeserializedClass = JsonConvert.DeserializeObject<Lottery2026Info>(webresult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching or deserializing data for index {idx}: {ex.Message}");
        }

        return myDeserializedClass;
    }

    public Lottery2026Info GetLottoRangeData(int startIdx, int endIdx)
    {
        Lottery2026Info myDeserializedClass = null;

        if (web is null)
        {
            Console.WriteLine("WebHttp instance is null.");
            return myDeserializedClass;
        }

        try
        {
            var webresult = web.GetHttp(string.Format(LOTTERYCONSTANTS.KR2026_LOTTERY_RANGE_URL, startIdx, endIdx));
            myDeserializedClass = JsonConvert.DeserializeObject<Lottery2026Info>(webresult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching or deserializing data for index {startIdx} ~ {endIdx}: {ex.Message}");
        }

        return myDeserializedClass;
    }

    public Lottery2026Info GetLottoLatestData()
    {
        Lottery2026Info myDeserializedClass = null;
        if (web is null)
        {
            Console.WriteLine("WebHttp instance is null.");
            return myDeserializedClass;
        }
        try
        {
            var webresult = web.GetHttp(LOTTERYCONSTANTS.KR2026_LOTTERY_LASTEST_URL);
            myDeserializedClass = JsonConvert.DeserializeObject<Lottery2026Info>(webresult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching or deserializing latest lottery data: {ex.Message}");
        }
        return myDeserializedClass;
    }
}