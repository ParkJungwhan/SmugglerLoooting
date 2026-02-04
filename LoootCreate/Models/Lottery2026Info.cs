namespace LoootCreate.Models;

/// <summary>
/// Lottery2026Info myDeserializedClass = JsonConvert.DeserializeObject<Lottery2026Info>(myJsonResponse);
/// </summary>
public class Lottery2026Info
{
    public int resultCode { get; set; }
    public string resultMessage { get; set; }
    public Data data { get; set; }
}

public class Data
{
    public List<LotteryInfo> list { get; set; }
}

public class LotteryInfo
{
    public int winType0 { get; set; }
    public int winType1 { get; set; }   // 1등 자동 당첨자수
    public int winType2 { get; set; }   // 1등 수동 당첨자수
    public int winType3 { get; set; }   // 1등 반자동 당첨자수
    public int gmSqNo { get; set; }
    public int ltEpsd { get; set; }     // 회차번호
    public int tm1WnNo { get; set; }
    public int tm2WnNo { get; set; }
    public int tm3WnNo { get; set; }
    public int tm4WnNo { get; set; }
    public int tm5WnNo { get; set; }
    public int tm6WnNo { get; set; }
    public int bnsWnNo { get; set; }        // 보너스 번호
    public string ltRflYmd { get; set; }    //추첨일 string format "YYYYMMDD"
    public int rnk1WnNope { get; set; }     // 1등 당첨게임 수
    public int rnk1WnAmt { get; set; }      // 1게임당 당첨금
    public long rnk1SumWnAmt { get; set; }  // 등위별 총 당첨금(1등)
    public int rnk2WnNope { get; set; }     // 2등
    public int rnk2WnAmt { get; set; }
    public long rnk2SumWnAmt { get; set; }
    public int rnk3WnNope { get; set; }     // 3등
    public int rnk3WnAmt { get; set; }
    public long rnk3SumWnAmt { get; set; }
    public int rnk4WnNope { get; set; }     // 4등
    public int rnk4WnAmt { get; set; }
    public long rnk4SumWnAmt { get; set; }
    public int rnk5WnNope { get; set; }     // 5등
    public int rnk5WnAmt { get; set; }
    public long rnk5SumWnAmt { get; set; }
    public int sumWnNope { get; set; }      // 총 당첨게임 수
    public long rlvtEpsdSumNtslAmt { get; set; }
    public long wholEpsdSumNtslAmt { get; set; }        // 총 판매금액
    public string excelRnk { get; set; }    //비고란 출력
}