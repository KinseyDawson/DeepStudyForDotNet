namespace AsyncLocalStudy;
public class Program
{
    private static AsyncLocal<object> m_test = new ();
    

    public static async Task Main(string[] args)
    {
        m_test.Value = "Out";
        Console.WriteLine($"MainBegin:{m_test.Value}:{Thread.CurrentThread.ManagedThreadId}");
        //await TestChangeValueAsync();
        await TestNewAsync();
        Console.WriteLine($"MainEnd:{m_test.Value??"null"}:{Thread.CurrentThread.ManagedThreadId}");
        Console.ReadLine();
    }

    private static async Task TestChangeValueAsync()
    {
        Thread.Sleep(TimeSpan.FromSeconds(3));
        m_test.Value = "Inner";
        Console.WriteLine($"TestAsync:{m_test.Value}:{Thread.CurrentThread.ManagedThreadId}");
        await Task.CompletedTask;
    }

    private static async Task TestNewAsync()
    {
        Thread.Sleep(TimeSpan.FromSeconds(3));
        m_test = new AsyncLocal<object>
        {
            Value = "Inner"
        };
        Console.WriteLine($"TestAsync:{m_test.Value}:{Thread.CurrentThread.ManagedThreadId}");
        await Task.CompletedTask;
    }

}