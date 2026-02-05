using System.Collections.Generic;
using LoootCreate.Models;
using LoootCreate.Services.DB;
using LoootCreate.Services.Network;

namespace LoootCreate.Services.Lottery;

public class LottoManager
{
    private HashSet<string> hash = null;

    private int lastestDrawNo = 0;

    private LottoWebManager webManager;
    private DateOnly lastestWeekDate;

    public LottoManager()
    {
        webManager = new LottoWebManager();
    }

    public void AllHistoryLoad()
    {
        var nowDate = DateOnly.FromDateTime(DateTime.Now);
        if (lastestWeekDate == null)
        {
        }
        else if (lastestWeekDate > nowDate)
        {
        }

        // DB에서 모든 로또 히스토리 가져오기
        hash = DBManager.Instance.LotteryDB.AllHistory();
        lastestDrawNo = DBManager.Instance.LotteryDB.LastestDrawNo;

        //Web에서 가져오기
        var lastweeklottodata = webManager.GetLottoLatestData();    //1209
        if (lastweeklottodata.IsSuccess)
        {
            // 데이터를 정상적으로 받아옴(이걸 기준으로 그동안의 데이터 가져오기)
            int lastweekIdx = lastweeklottodata.data.list[0].ltEpsd;

            if (false == DateTime.TryParseExact(lastweeklottodata.data.list[0].ltRflYmd, "yyyyMMdd",
                null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                Console.WriteLine("날짜 파싱 실패");
                return;
            }

            lastestWeekDate = DateOnly.FromDateTime(parsedDate);

            if (lastestDrawNo == 0) lastestDrawNo++;

            // 차이만큼 데이터 가져오기
            if (lastweekIdx >= lastestDrawNo)
            {
                var addlottoInfo = webManager.GetLottoRangeData(lastestDrawNo, lastweekIdx);

                if (addlottoInfo.IsSuccess)
                {
                    // 받아온 데이터의 가장 최신 회차
                    lastestDrawNo = addlottoInfo.data.list.Max(x => x.ltEpsd);

                    List<LotteryNumber> lstLotto = new List<LotteryNumber>(addlottoInfo.data.list.Count());

                    foreach (var item in addlottoInfo.data.list)
                    {
                        lstLotto.Add(new LotteryNumber()
                        {
                            lotteryid = (byte)item.ltEpsd,
                            Num1 = (byte)item.tm1WnNo,
                            Num2 = (byte)item.tm2WnNo,
                            Num3 = (byte)item.tm3WnNo,
                            Num4 = (byte)item.tm4WnNo,
                            Num5 = (byte)item.tm5WnNo,
                            Num6 = (byte)item.tm6WnNo,
                            bonusnum = (byte)item.bnsWnNo
                        });
                    }

                    // DB에 넣기
                    DBManager.Instance.LotteryDB.InsertLotteryNumbers(lstLotto);

                    foreach (var item in lstLotto)
                    {
                        string sKey = $"{item.Num1},{item.Num2},{item.Num3},{item.Num4},{item.Num5},{item.Num6}";
                        hash.Add(sKey);
                    }
                }
            }
        }
    }
}