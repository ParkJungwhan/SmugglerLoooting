using Dapper;

namespace LoootCreate.Services.DB;

public class LotteryDBManager
{
    private readonly DBConnection ConnManager;

    private int LastestDrawNo = 0;

    public LotteryDBManager(DBConnection connManager)
    {
        ConnManager = connManager;
    }

    // 함수1 : lottery 테이블에 로또번호만 저장
    // 함수2 : lottery2026 테이블에 당첨금 일체 저장

    public HashSet<string> AllHistory()
    {
        ConnManager.IsConnected();

        //ConnManager.
        ConnManager.Conn.Open();

        //string sql = "select drwno, drwtno1,drwtno2,drwtno3,drwtno4,drwtno5,drwtno6 from lottery;";
        string sql = "select lotteryid, num1,num2,num3,num4,num5,num6 from lottery;";
        var result = ConnManager.Conn.Query<(int, int, int, int, int, int, int)>(sql);

        ConnManager.Conn.Close();

        HashSet<string> hash = new HashSet<string>(result.Count());

        foreach (var item in result)
        {
            string sKey = $"{item.Item2},{item.Item3},{item.Item4},{item.Item5},{item.Item6},{item.Item7}";

            LastestDrawNo = (item.Item1 > LastestDrawNo) ? item.Item1 : LastestDrawNo;

            hash.Add(sKey);
        }

        return hash;
    }
}