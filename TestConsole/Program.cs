using System.Security.Cryptography;
using Dapper;
using LoootCreate.Models;
using LoootCreate.Services;
using Npgsql;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppSetting.LoadConfig();

            using var conn = new NpgsqlConnection(AppConfig.ConnectionString);
            conn.Open();

            string sql = "select drwno, drwtno1,drwtno2,drwtno3,drwtno4,drwtno5,drwtno6 from lottery;";
            var result = conn.Query<(int, int, int, int, int, int, int)>(sql);

            conn.Close();

            int nRow = result.Count();

            HashSet<string> hash = new HashSet<string>(nRow);

            foreach (var item in result)
            {
                string sKey = $"{item.Item2},{item.Item3},{item.Item4},{item.Item5},{item.Item6},{item.Item7}";

                hash.Add(sKey);
            }

            // 여기서 만든 값으로 hashset에 있는지 검출

            var allspan = LottoMaker.All_Number.AsSpan();
            int nSameValue = 0;
            for (int i = 0; i < 1000000; i++)
            {
                RandomNumberGenerator.Shuffle(allspan);
                int[] rand6num = allspan.ToArray().Take(6).Order().ToArray();
                string newKey = $"{rand6num[0]},{rand6num[1]},{rand6num[2]},{rand6num[3]},{rand6num[4]},{rand6num[5]}";
                if (false == hash.Contains(newKey))
                {
                    //Console.WriteLine($"New number found: {newKey}");
                    //break;
                }
                else
                {
                    Console.WriteLine($"{i}\t Same number found!!!!!!!\t{newKey}");
                    nSameValue++;
                    i = 0;
                    if (10 <= nSameValue)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine($"Total same values found: {nSameValue}");

            //string newKey = $"{newRand6num[0]},{newRand6num[1]},{newRand6num[2]},{newRand6num[3]},{newRand6num[4]},{newRand6num[5]}";

            return;
        }
    }
}