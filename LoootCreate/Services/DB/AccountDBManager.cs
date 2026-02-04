using System.Data;
using System.Diagnostics;
using Dapper;

namespace LoootCreate.Services.DB;

internal class AccountDBManager
{
    private readonly IDbConnection conn;

    public AccountDBManager(IDbConnection connManager)
    {
        conn = connManager;
    }

    public string GetTableKey(string keyname, string keydiv)
    {
        string result = null;
        try
        {
            conn.Open();

            result = conn.QuerySingleOrDefault<string>(
               "SELECT fn_get_keyvalue(@p_keyname, @p_keydiv)",
               new { p_keyname = keyname, p_keydiv = keydiv });
        }
        catch (Exception ee)
        {
            Debug.WriteLine(ee.Message);
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
}