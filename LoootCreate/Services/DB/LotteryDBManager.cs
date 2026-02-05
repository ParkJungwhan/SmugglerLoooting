using Dapper;
using LoootCreate.Models;

namespace LoootCreate.Services.DB;

public class LotteryDBManager
{
    private readonly DBConnection ConnManager;

    public int LastestDrawNo = 0;

    public LotteryDBManager(DBConnection connManager)
    {
        ConnManager = connManager;
    }

    // 함수1 : lottery 테이블에 로또번호만 저장
    // 함수2 : lottery2026 테이블에 당첨금 일체 저장

    public HashSet<string> AllHistory()
    {
        ConnManager.IsConnected();

        IEnumerable<LotteryNumber> result = null;

        try
        {
            ConnManager.Conn.Open();

            string sql = "select lotteryid,num1,num2,num3,num4,num5,num6 from lottery;";
            result = ConnManager.Conn.Query<LotteryNumber>(sql);
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        finally
        {
            ConnManager.Conn.Close();
        }

        HashSet<string> hash = new HashSet<string>(result.Count());

        foreach (var item in result)
        {
            string sKey = $"{item.Num1},{item.Num2},{item.Num3},{item.Num4},{item.Num5},{item.Num6}";
            LastestDrawNo = (item.lotteryid > LastestDrawNo) ? item.lotteryid : LastestDrawNo;
            hash.Add(sKey);
        }

        return hash;
    }

    public void InsertLotteryNumbers(List<LotteryNumber> lottos)
    {
        ConnManager.IsConnected();

        try
        {
            ConnManager.Conn.Open();

            string sql = "CALL sp_insert_lottery(@p_lotteryid, @p_num1, @p_num2, @p_num3, @p_num4, @p_num5, @p_num6, @p_bonus)";

            foreach (var lotto in lottos)
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_lotteryid", lotto.lotteryid);
                parameters.Add("p_num1", lotto.Num1);
                parameters.Add("p_num2", lotto.Num2);
                parameters.Add("p_num3", lotto.Num3);
                parameters.Add("p_num4", lotto.Num4);
                parameters.Add("p_num5", lotto.Num5);
                parameters.Add("p_num6", lotto.Num6);
                parameters.Add("p_bonus", lotto.bonusnum);
                int i = ConnManager.Conn.Execute(sql, parameters);
            }
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        finally
        {
            ConnManager.Conn.Close();
        }
    }
}