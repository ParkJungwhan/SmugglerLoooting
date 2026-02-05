using System.Diagnostics;
using Dapper;

namespace LoootCreate.Services.DB;

public class AccountDBManager
{
    private readonly DBConnection conn;

    public AccountDBManager(DBConnection connManager)
    {
        conn = connManager;
    }

    public string GetTableKey(string keyname, string keydiv)
    {
        string result = null;
        try
        {
            conn.Conn.Open();

            result = conn.Conn.QuerySingleOrDefault<string>(
               "SELECT fn_get_keyvalue(@p_keyname, @p_keydiv)",
               new { p_keyname = keyname, p_keydiv = keydiv });
        }
        catch (Exception ee)
        {
            Debug.WriteLine(ee.Message);
        }
        finally
        {
            conn.Conn.Close();
        }

        return result;
    }
}