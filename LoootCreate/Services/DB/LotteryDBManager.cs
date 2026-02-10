using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
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

            // 회차별 정렬
            lottos.Sort((a, b) => a.lotteryid.CompareTo(b.lotteryid));

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

    internal void InsertLotteryAllInfoNumbers(List<Lottery2026Data> list)
    {
        ConnManager.IsConnected();

        try
        {
            ConnManager.Conn.Open();

            string insertsql = "insert into lottery2026 ";
            insertsql += "(wintype0,wintype1,wintype2,wintype3,gmsqno,ltepsd,tm1wnno,tm2wnno,tm3wnno,tm4wnno,tm5wnno,tm6wnno,bnswnno,ltrflymd,rnk1wnnope,rnk1wnamt,rnk1sumwnamt,rnk2wnnope,rnk2wnamt,rnk2sumwnamt,rnk3wnnope,rnk3wnamt,rnk3sumwnamt,rnk4wnnope,rnk4wnamt,rnk4sumwnamt,rnk5wnnope,rnk5wnamt,rnk5sumwnamt,sumwnnope,rlvtepsdsumntslamt,wholepsdsumntslamt,excelrnk) VALUES ";

            int nLimit = 100;

            string sql = insertsql;

            // 회차별 정렬
            list.Sort((a, b) => a.ltEpsd.CompareTo(b.ltEpsd));

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                sql +=
                    $"( {item.winType0},{item.winType1},{item.winType2},{item.winType3}," +
                    $"{item.gmSqNo},{item.ltEpsd},{item.tm1WnNo},{item.tm2WnNo},{item.tm3WnNo},{item.tm4WnNo},{item.tm5WnNo},{item.tm6WnNo},{item.bnsWnNo},{item.ltRflYmd}," +
                    $"{item.rnk1WnNope},{item.rnk1WnAmt},{item.rnk1SumWnAmt}," +
                    $"{item.rnk2WnNope},{item.rnk2WnAmt},{item.rnk2SumWnAmt}," +
                    $"{item.rnk3WnNope},{item.rnk3WnAmt},{item.rnk3SumWnAmt}," +
                    $"{item.rnk4WnNope},{item.rnk4WnAmt},{item.rnk4SumWnAmt}," +
                    $"{item.rnk5WnNope},{item.rnk5WnAmt},{item.rnk5SumWnAmt}," +
                    $"{item.sumWnNope},{item.rlvtEpsdSumNtslAmt},{item.wholEpsdSumNtslAmt},'{item.excelRnk}'),";

                if (i >= nLimit ||              // 100개 단위로 insert
                    i == list.Count - 1)        // 마지막 남은거 insert
                {
                    sql = sql.Remove(sql.Length - 1, 1) + ";";  // 마지막 , 제거
                    int nRow = ConnManager.Conn.QuerySingleOrDefault<int>(sql);

                    nLimit += 100;

                    sql = insertsql;
                }
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