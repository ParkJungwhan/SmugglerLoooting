namespace LoootCreate;

internal class Program
{
    private static void Main(string[] args)
    {
        // Console.WriteLine("Hello, World!");
        // monitoring 서버에 재시작 알림 필요

        ServerApp app = new ServerApp();

        if (false == app.InitApp())
        {
            Console.WriteLine("ServerApp Init Fail. Exit");
            Console.ReadLine();
            return;
        }

        if (false == app.StartApp())
        {
            Console.WriteLine("ServerApp Start Fail. Exit");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"{DateTime.Now}\tPress Enter to exit");
        Console.ReadLine();
    }
}