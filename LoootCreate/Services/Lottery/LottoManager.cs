using LoootCreate.Models;
using LoootCreate.Services.DB;
using LoootCreate.Services.Network;

namespace LoootCreate.Services.Lottery;

public class LottoManager
{
    public HashSet<string> hash { get; private set; }

    private int lastestDrawNo = 0;

    private LottoWebManager webManager;
    private DateOnly lastestWeekDate;

    public LottoManager()
    {
        webManager = new LottoWebManager();

        Timer timer = new Timer((e) =>
        {
            UpdateLastestWeekDateLottery();
        }, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        //}, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    public void AllHistoryLoad()
    {
        // DB에서 모든 로또 히스토리 가져오기
        hash = DBManager.Instance.LotteryDB.AllHistory();
        lastestDrawNo = DBManager.Instance.LotteryDB.LastestDrawNo;

        //Web에서 가져오기
        var lastweeklottodata = webManager.GetLottoLatestData();    //1209
        if (lastweeklottodata.IsSuccess)
        {
            // 데이터를 정상적으로 받아옴(이걸 기준으로 그동안의 데이터 가져오기)
            int lastweekIdx = lastweeklottodata.data.list[0].ltEpsd;
            Console.WriteLine($"최신 로또 데이터 회차: {lastweekIdx}");

            if (false == DateTime.TryParseExact(lastweeklottodata.data.list[0].ltRflYmd, "yyyyMMdd",
                null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                Console.WriteLine("날짜 파싱 실패");
                return;
            }

            lastestWeekDate = DateOnly.FromDateTime(parsedDate);

            if (lastestDrawNo == 0) lastestDrawNo++;

            // 차이만큼 데이터 가져오기
            if (lastweekIdx > lastestDrawNo)
            {
                var addlottoInfo = webManager.GetLottoRangeData(lastestDrawNo, lastweekIdx);
                Console.WriteLine($"로또 데이터 조회: {lastestDrawNo} ~ {lastweekIdx}");

                if (addlottoInfo.IsSuccess)
                {
                    // 받아온 데이터의 가장 최신 회차
                    lastestDrawNo = addlottoInfo.data.list.Max(x => x.ltEpsd);

                    List<LotteryNumber> lstLotto = new List<LotteryNumber>(addlottoInfo.data.list.Count());

                    foreach (var item in addlottoInfo.data.list)
                    {
                        lstLotto.Add(new LotteryNumber()
                        {
                            lotteryid = item.ltEpsd,
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
                    Console.WriteLine($"로또 데이터 추가 완료: {lstLotto.Count()}개");

                    DBManager.Instance.LotteryDB.InsertLotteryAllInfoNumbers(addlottoInfo.data.list);
                    Console.WriteLine($"로또 데이터 추가 완료: {addlottoInfo.data.list.Count()}개");

                    foreach (var item in lstLotto)
                    {
                        string sKey = $"{item.Num1},{item.Num2},{item.Num3},{item.Num4},{item.Num5},{item.Num6}";
                        hash.Add(sKey);
                    }
                }
            }
        }
    }

    private void UpdateLastestWeekDateLottery()
    {
        // 현재의 시간이 마지막 로또 날짜보다 일주일 뒤 이후인지 확인
        var nowDate = DateOnly.FromDateTime(DateTime.Now);

        if (lastestWeekDate == DateOnly.MinValue) return;

        var nextweek = lastestWeekDate.AddDays(7);
        if (nextweek < nowDate) // 일주일 뒤다
        {
            Console.WriteLine($"{DateTime.Now}\t최신 로또 갱신");
            AllHistoryLoad();
        }
    }
}