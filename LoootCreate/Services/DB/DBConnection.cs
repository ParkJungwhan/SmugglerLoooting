using System.Data;
using System.Diagnostics;
using Npgsql;

namespace LoootCreate.Services.DB;

public class DBConnection
{
    public IDbConnection Conn;
    private string connstring;

    public void SetConnection(string connectionstring)
    {
        CloseConnection();

        if (string.IsNullOrEmpty(connectionstring))
        {
            throw new ArgumentException("Connection string is null or empty");
        }

        connstring = connectionstring;

        try
        {
            Conn = new NpgsqlConnection(connectionstring);
        }
        catch (Exception e)
        {
            throw new Exception("DB connection failed", e);
        }
    }

    public bool IsConnected()
    {
        bool result = false;
        if (Conn == null) return result;

        try
        {
            Conn.Open();
            result = true;
        }
        catch (Exception ee)
        {
            Debug.WriteLine(ee.Message);
            result = false;
        }
        finally
        {
            Conn.Close();
        }
        return result;
    }

    public void CloseConnection()
    {
        if (Conn != null)
        {
            Conn.Close();
            Conn.Dispose();
        }
    }

    //public bool CallProcedure(string procedurename, Dictionary<string, string> dicParam)
    //{
    //    bool result = false;
    //    try
    //    {
    //        conn.Open();

    //        conn.Execute(
    //            $"CALL {procedurename}();",
    //            parameters
    //        );
    //        result = true;
    //    }
    //    catch (Exception ee)
    //    {
    //        Debug.WriteLine(ee.Message);
    //        result = false;
    //    }
    //    finally
    //    {
    //        conn.Close();
    //    }
    //    return result;
    //}
}