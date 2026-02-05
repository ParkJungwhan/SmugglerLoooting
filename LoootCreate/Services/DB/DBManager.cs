using LoootCreate.Models;

namespace LoootCreate.Services.DB;

public class DBManager
{
    // singleton으로 구현하기 위한 선언부
    private static readonly Lazy<DBManager> _instance = new Lazy<DBManager>(() => new DBManager());

    public static DBManager Instance => _instance.Value;

    private DBManager()
    {
        // private 생성자: 외부에서 인스턴스를 생성하지 못하도록 방지
    }

    private string DBConnectionString { get; set; } = string.Empty;
    private DBConnection DBConn { get; set; }

    public AccountDBManager AccountDB { get; set; }

    public LotteryDBManager LotteryDB { get; set; }

    public void InitializeDBManagers()
    {
        try
        {
            // DB 설정
            DBConn = new DBConnection();
            DBConn.SetConnection(AppConfig.ConnectionString);

            if (DBConn == null)
            {
                throw new InvalidOperationException("DBConn must be set before initializing DB managers.");
            }

            // DB 초기화
            LotteryDB = new LotteryDBManager(DBConn);
            AccountDB = new AccountDBManager(DBConn);
            LotteryDB = new LotteryDBManager(DBConn);

            DBInit(DBConn);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing DB managers: {ex.Message}");
            throw;
        }
    }

    private bool DBInit(DBConnection DBconn)
    {
        if (false == DBconn.IsConnected())
        {
            Console.WriteLine("DB connection failed.");
            return false;
        }

        if (AccountDB != null) return false;

        var keyvalue = AccountDB.GetTableKey("telegrambot", "lottery");
        if (true == string.IsNullOrEmpty(keyvalue)) Console.WriteLine("[DB] No result value!!!");

        AppConfig.TelegramBotToken = keyvalue;

        return true;
    }
}