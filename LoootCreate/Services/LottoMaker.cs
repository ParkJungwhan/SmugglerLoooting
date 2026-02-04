using System.Data;
using System.Diagnostics;
using Dapper;
using LoootCreate.Models;
using Npgsql;

namespace LoootCreate.Services;

public class LottoMaker
{
    public static int[] All_Number = Enumerable.Range(1, 45).ToArray();
    private int[] number;
    private Random my_rand;

    public LottoMaker()
    {
        number = new int[6];
        my_rand = new Random();
    }

    private void MakeNumber()
    {
        int a;
        for (int i = 0; i < number.Length; i++)
        {
            a = my_rand.Next(1, 46);
            if (true == CheckSame(i, a))
            {
                number[i] = a;
            }
            else
            {
                i--;
            }
        }
    }

    private bool CheckSame(int index, int value)
    {
        for (int i = 0; i < index; i++)
        {
            if (value == number[i])
            {
                return false;
            }
        }
        return true;
    }

    public List<int> Make()
    {
        try
        {
            using var db = new NpgsqlConnection(DBInfo.ConnectionString);

            db.Open();

            int rowcount = -1;

            do
            {
                MakeNumber();

                var result = number.ToList();
                result.Sort();

                string query =
                    $"select count(*) as count from lottery where drwtno1={result[0]} and drwtno2={result[1]} and drwtno3={result[2]} and drwtno4={result[3]} and drwtno5={result[4]} and drwtno6={result[5]}";

                var output = db.QuerySingle<int>(query, commandType: CommandType.Text);

                //if (output == 0)
                //{
                //    // 그럼 history에 추가한다.
                //    // 일단 History에 있는지 확인 후
                //    query = $"select count(*) as count from history where no1={result[0]} and no2={result[1]} and no3={result[2]} and no4={result[3]} and no5={result[4]} and no6={result[5]}";
                //    output = db.QuerySingle<int>(query, commandType: CommandType.Text);
                //    if(output == 0)
                //    {
                //        // 0이면 없는거니 데이터를 넣어준다
                //        query = $"update MDB set no1={result[0]}, no2={result[1]}, no3={result[2]}, no4={result[3]}, no5={result[4]},no6={result[5]}, getdate={} ";
                //        output = db.QuerySingle<int>(query, commandType: CommandType.Text);

                //    }

                //}
            } while (rowcount == 0);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return number.ToList();
    }
}