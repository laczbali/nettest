using System.Net.NetworkInformation;

namespace ConsoleApp;
public class Tester
{
    private readonly TimeSpan testInterval;
    private readonly Uri targetSite;
    private readonly string logFilePath;

    private Ping pinger = new Ping();
    private int lastConsoleLineLength = 0;

    public Tester(TimeSpan testInterval, Uri targetSite, string logFilePath)
    {
        this.testInterval = testInterval;
        this.targetSite = targetSite;
        this.logFilePath = logFilePath;

        if (!File.Exists(logFilePath))
        {
            File.Create(logFilePath).Close();
        }

        if (!PingTarget())
        {
            throw new ArgumentException("Target site is not reachable");
        }
    }

    public void Start()
    {
        while (true)
        {
            var pingResult = PingTarget() ? "OK" : "FAIL";
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            LogToConsole($"{currentTime} - {pingResult}");
            File.AppendAllText(logFilePath, $"{currentTime};{pingResult}" + Environment.NewLine);
            Thread.Sleep(testInterval);
        }
    }

    private bool PingTarget()
    {
        try
        {
            var reply = pinger.Send(targetSite.Host);
            return reply.Status == IPStatus.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void LogToConsole(string message)
    {
        if (message.Length < lastConsoleLineLength)
        {
            message += new string(' ', lastConsoleLineLength - message.Length);
        }

        Console.Write(message);
        Console.SetCursorPosition(0, Console.CursorTop);
        lastConsoleLineLength = message.Length;
    }
}
